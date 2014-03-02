using System;
using System.ComponentModel;
using System.IO;

namespace PokeSave.Fourth
{
	public enum Games { Diamond, Pearl, Platinum, HeartGold, SoulSilver }
	public class SaveFile4G
	{
		public static int[] Offsets = new int[]{
			0,
			0xC100,
			0x20000,
			0x40000,
			0x4C100,
			0x60000
		};
	}

	public static class Shuffle
	{
		public static void BlocksForDecryption( SaveBlock _buffer, uint Personality )
		{
			ShuffleBlocks( _buffer, Personality, todecrypt );
		}
		public static void BlocksForEncryption( SaveBlock _buffer, uint Personality )
		{
			ShuffleBlocks( _buffer, Personality, toencrypt );
		}
		static void ShuffleBlocks( SaveBlock _buffer, uint Personality, int[] indexes )
		{
			var blocks = new byte[4][];
			for( int i = 0; i < 4; i++ )
			{
				blocks[i] = new byte[32];
				for( int j = 0; j < 32; j++ )
				{
					blocks[i][j] = _buffer[i * 32 + j];
				}
			}

			for( int i = 0; i < 4; i++ )
			{
				var toindex = ExtractShuffleIndexFromArray( i, Personality, indexes );
				for( int j = 0; j < 32; j++ )
				{
					_buffer[toindex * 32 + j] = blocks[i][j];
				}
			}

		}
		static int[] todecrypt = new[]{
0,1,2,3,
0,1,3,2,
0,2,1,3,
0,3,1,2,
0,2,3,1,
0,3,2,1,
1,0,2,3,
1,0,3,2,
2,0,1,3,
3,0,1,2,
2,0,3,1,
3,0,2,1,
1,2,0,3,
1,3,0,2,
2,1,0,3,
3,1,0,2,
2,3,0,1,
3,2,0,1,
1,2,3,0,
1,3,2,0,
2,1,3,0,
3,1,2,0,
2,3,1,0,
3,2,1,0};
		static int[] toencrypt = new[]{
0,1,2,3,
0,1,3,2,
0,2,1,3,
0,2,3,1,
0,3,1,2,
0,3,2,1,
1,0,2,3,
1,0,3,2,
1,2,0,3,
1,2,3,0,
1,3,0,2,
1,3,2,0,
2,0,1,3,
2,0,3,1,
2,1,0,3,
2,1,3,0,
2,3,0,1,
2,3,1,0,
3,0,1,2,
3,0,2,1,
3,1,0,2,
3,1,2,0,
3,2,0,1,
3,2,1,0};
		static int ExtractShuffleIndexFromArray( int blockindex, uint pid, int[] indexes )
		{
			var row = ( ( pid & 0x3E000 ) >> 0xD ) % 24;
			return indexes[row * 4 + blockindex];
		}
	}

	public class EncryptionSequence
	{
		const uint offset = 0x6073;
		const uint mask = 0xffffffff;
		const uint mutator = 0x41C64E6D;
		uint seed;
		public EncryptionSequence( uint seed )
		{
			this.seed = seed;
		}

		uint Next()
		{
			seed = ( ( seed * mutator ) + offset ) & mask;
			return ( seed >> 16 ) & 0xFFFF;
		}

		public void Run( SaveBlock buffer, int offset, int length )
		{
			for( int i = offset; i < offset + length; i += 2 )
			{
				//test this little endian conversion
				var bitmask = Next();
				buffer[i] = (byte) ( buffer[i] ^ ( ( ( bitmask & 0xff00 ) >> 8 ) ) );
				buffer[i + 1] = (byte) ( buffer[i + 1] ^ ( bitmask & 0xff ) );
			}
		}
	}
	public class NdsPkm
	{
		readonly bool _isStorage;
		readonly SaveBlock _buffer;
		PkmState _encryptionState;

		public PkmState EncryptionState
		{
			get { return _encryptionState; }
			private set
			{
				if( _encryptionState != value )
				{
					if( _encryptionState == PkmState.Decrypted )
						Encrypt();
					else
						Decrypt();
					_encryptionState = value;
				}
			}
		}

		void Decrypt()
		{
			var pkmDataEncryption = new EncryptionSequence( Checksum );
			pkmDataEncryption.Run( _buffer, 8, 128 );
			if( !_isStorage )
			{
				var battleDataEncryption = new EncryptionSequence( Personality );
				battleDataEncryption.Run( _buffer, 136, 100 );
			}
			Shuffle.BlocksForDecryption( _buffer, Personality );

		}

		void Encrypt()
		{
			var checksum = GenerateChecksum();
			if( !checksum.HasValue )
				throw new Exception( "Couldnt generate checksum from encrypted block" );
			Checksum = checksum.Value;
			Shuffle.BlocksForEncryption( _buffer, Personality );
			var pkmDataEncryption = new EncryptionSequence( checksum.Value );
			pkmDataEncryption.Run( _buffer, 8, 128 );
			if( !_isStorage )
			{
				var battleDataEncryption = new EncryptionSequence( Personality );
				battleDataEncryption.Run( _buffer, 136, 100 );
			}

		}

		public uint Personality { get { return _buffer.GetInt( 0 ); } }

		public uint Checksum
		{
			get { return _buffer.GetShort( 6 ); }
			set
			{
				if( EncryptionState == PkmState.Encrypted )
					throw new InvalidOperationException( "Cant set checksum in encrypted pkm, that would destroy data" );
				_buffer.SetShort( 6, value );
			}
		}

		public uint AttackEV
		{
			get
			{
				EncryptionState = PkmState.Decrypted;
				return _buffer[0x19];
			}
			set { }
		}

		public uint? GenerateChecksum()
		{
			if( EncryptionState == PkmState.Encrypted )
				return null;
			uint sum = 0;
			for( int i = 8; i < 0x87; i += 2 )
			{
				sum += _buffer.GetShort( i );
			}
			return sum & 0xffff;
		}
		public NdsPkm( byte[] buffer, int offset, bool isStorage, bool fromPkm )
			: this( new PkmHolderSaveBlock( buffer, offset, isStorage ? 136 : 236 ), fromPkm )
		{
			_isStorage = isStorage;
		}

		public NdsPkm( SaveBlock buffer, bool fromPkm )
		{
			_buffer = buffer;
			EncryptionState = fromPkm ? PkmState.Decrypted : PkmState.Encrypted;


		}
	}

	public enum PkmState { Encrypted, Decrypted }

	public class ChecksumGenerator
	{
		public static int[] SeedTable;

		static ChecksumGenerator()
		{
			SeedTable = new int[256];

			for( var index = 0; index < SeedTable.Length; index++ )
			{
				var result = index << 8;
				for( var index2 = 0; index2 < 8; index2++ )
				{
					if( ( (byte) ( result >> 8 ) & 0x80 ) != 0 )
						result = ( 2 * result ) ^ 0x1021;
					else
						result *= 2;
				}


				SeedTable[index] = (ushort) ( result );
			}


		}

		public static ushort GetCheckSum( byte[] data, int offset, int length )
		{
			int sum = 0xFFFF;

			for( int i = offset; i < length; i++ )
				sum = ( sum << 8 ) ^ SeedTable[(byte) ( data[i] ^ (byte) ( sum >> 8 ) )];

			return (ushort) sum;
		}

	}
	public class PkmHolderSaveBlock : SaveBlock
	{
		public PkmHolderSaveBlock( byte[] data, int offset, int length )
			: base( data, offset, length ) { }

		public override byte this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				base[index] = value;
			}
		}
	}

	public class GeneralSaveBlock4G : SaveBlock
	{


		public GeneralSaveBlock4G( byte[] data, int offset, int length ) // length must be block without footer
			: base( data, offset, length ) { }

		public uint BlockSize
		{
			get { return GetInt( Length + 0x8 ); }
		}

		public uint Checksum
		{
			get { return GetShort( Length + 0x12 ); }
			set { SetShort( Length + 0x12, value ); }
		}

		protected override void FixChecksum()
		{
			Checksum = ChecksumGenerator.GetCheckSum( Data, BaseOffset, Length );
		}
	}

	public abstract class SaveBlock : INotifyPropertyChanged
	{
		protected readonly int BaseOffset;
		protected readonly int Length;
		protected readonly byte[] Data;

		bool _isDirty;

		protected SaveBlock( byte[] data, int offset, int length )
		{
			Data = data;
			BaseOffset = offset;
			Length = length;
		}

		public bool IsDirty
		{
			get { return _isDirty; }
			private set
			{
				if( _isDirty != value )
				{
					_isDirty = value;
					InvokePropertyChanged( "IsDirty" );
				}
			}
		}

		public virtual byte this[int index]
		{
			get { return Data[BaseOffset + index]; }
			set
			{
				IsDirty = true;
				Data[BaseOffset + index] = value;
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

		public uint GetInt( int offset )
		{
			return
				( (uint) this[offset + 3] << 24 )
					| ( (uint) this[offset + 2] << 16 )
						| ( (uint) this[offset + 1] << 8 )
							| ( this[offset] );
		}

		public uint GetShort( int offset )
		{
			return ( (uint) this[offset + 1] << 8 ) | this[offset];
		}

		public virtual void SetInt( int offset, uint data )
		{
			this[offset] = (byte) ( data & 0xff );
			this[offset + 1] = (byte) ( ( data >> 8 ) & 0xff );
			this[offset + 2] = (byte) ( data >> 16 & 0xff );
			this[offset + 3] = (byte) ( data >> 24 & 0xff );
		}

		public virtual void SetShort( int offset, uint data )
		{
			this[offset] = (byte) ( data & 0xff );
			this[offset + 1] = (byte) ( ( data >> 8 ) & 0xff );
		}

		protected virtual void FixChecksum()
		{
		}

		public void Save( Stream stream )
		{
			if( IsDirty )
				FixChecksum();

			stream.Write( Data, BaseOffset, Length );
		}

		public void InvokePropertyChanged( string e )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( e ) );
		}
	}
}
