using System;
using System.IO;
using System.Text;

namespace PokeSave
{
	public class GameSection
	{
		#region Lookuptables
		static readonly string[] Names = new[]
		{
			"Trainer",
			"Team",
			"Unknown 1",
			"Unknown 2",
			"Rival",
			"PC Buffer A",
			"PC Buffer B",
			"PC Buffer C",
			"PC Buffer D",
			"PC Buffer E",
			"PC Buffer F",
			"PC Buffer G",
			"PC Buffer H",
			"PC Buffer I"
		};

		static readonly int[] Sizes = new[]
		{
			3884,
			3968,
			3968,
			3968,
			3848,
			3968,
			3968,
			3968,
			3968,
			3968,
			3968,
			3968,
			3968,
			2000
		};
		#endregion

		readonly byte[] _datasource;

		protected GameSection()
			: this( (byte[]) null )
		{
		}

		public GameSection( byte[] data )
		{
			_datasource = data;
			IsDirty = false;
		}

		public GameSection( Stream instream )
		{
			_datasource = new byte[4 * 1024];
			int count = instream.Read( _datasource, 0, _datasource.Length );
			if( count != _datasource.Length )
				throw new ArgumentException( "file too short" );
			IsDirty = false;
		}

		public bool IsDirty { get; private set; }

		public virtual byte this[int index]
		{
			get { return _datasource[index]; }
			set
			{
				IsDirty = true;
				_datasource[index] = value;
			}
		}

		public int Length
		{
			get { return Sizes[ID]; }
		}

		public string Name
		{
			get { return Names[ID]; }
		}

		public uint Checksum
		{
			get { return GetShort( 0xff6 ); }
			set { SetShort( 0xff6, value ); }
		}

		public uint ID
		{
			get { return GetShort( 0xff4 ); }
		}

		public uint SaveIndex
		{
			get { return GetShort( 0xffc ); }
		}

		public uint CalculatedChecksum
		{
			get
			{
				uint chk = 0;
				for( int i = 0; i < Length; i += 4 )
					chk += GetInt( i );

				uint upper = ( chk >> 16 ) & 0xFFFF;
				uint lower = chk & 0xFFFF;
				return ( upper + lower ) & 0xFFFF;
			}
		}

		public void SetText( int offset, int count, string data )
		{
			TextTable.WriteString( this, data, offset, count );
		}
		public void SetTextRaw( int offset, int count, string data )
		{
			TextTable.WriteStringRaw( this, data, offset, count );
		}

		public string GetText( int offset, int count )
		{
			return TextTable.ReadString( this, offset, count );
		}

		public string GetTextRaw( int offset, int count )
		{
			return TextTable.ReadStringRaw( this, offset, count );
		}

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

		public void FixChecksum()
		{
			Checksum = CalculatedChecksum;
			IsDirty = false;
		}

		public void Write( Stream stream )
		{
			if( IsDirty )
				FixChecksum();

			stream.Write( _datasource, 0, _datasource.Length );
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( "     Name: " + Name );
			sb.AppendLine( "       ID: " + ID );
			sb.AppendLine( " Checksum: " + Checksum );
			sb.AppendLine( "Checksum2: " + CalculatedChecksum );
			sb.AppendLine( "Saveindex: " + SaveIndex );
			sb.AppendLine( "   Length: " + Length );
			return sb.ToString();
		}
	}
}