using System.Text;

namespace PokeSave
{
	public class MonsterEntry
	{
		readonly GameSection _data;
		readonly int _offset;
		readonly bool _storage;
		readonly Cipher _specificXor;

		public MonsterEntry( GameSection data, int offset, bool storage )
		{
			_data = data;
			_offset = offset;
			_storage = storage;
			_specificXor = new Cipher( Personality, OriginalTrainerId );
		}

		public uint Personality { get { return _data.GetInt( _offset ); } }
		public uint OriginalTrainerId { get { return _data.GetInt( _offset + 4 ); } }
		public string Name { get { return _data.GetText( _offset + 8, 10 ); } }
		public uint Language { get { return _data.GetShort( _offset + 18 ); } }
		public string OriginalTrainerName { get { return _data.GetText( _offset + 20, 7 ); } }
		public uint Mark { get { return _data.GetByte( _offset + 27 ); } }
		public uint Checksum { get { return _data.GetShort( _offset + 28 ); } }
		public uint CalculatedChecksum
		{
			get
			{
				uint sum = 0;
				for( int i = 0; i < 24; i++ )
				{
					var enc = _data.GetShort( _offset + 32 + ( 2 * i ) );
					sum += _specificXor.Run( enc );
				}
				uint upper = ( sum >> 16 ) & 0xFFFF;
				uint lower = sum & 0xFFFF;
				return ( upper + lower ) & 0xFFFF;
			}
		}

		public uint Unknown { get { return _data.GetShort( _offset + 30 ); } }

		public bool Empty
		{
			get
			{
				for( int i = _offset; i < _offset + 100; i++ )
					if( _data.GetByte( i ) != 0 )
						return false;
				return true;
			}
		}

		public override string ToString()
		{
			if( Empty )
				return string.Empty;
			var sb = new StringBuilder();
			sb.Append( string.Format( "{0} - {1} [{2}]\n", Name, Checksum, CalculatedChecksum ) );
			return sb.ToString();
		}
	}
}