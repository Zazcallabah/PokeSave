using System.Text;

namespace PokeSave
{
	public class MonsterEntry
	{
		readonly byte[] _data;
		readonly int _offset;
		readonly int _length;

		public MonsterEntry( byte[] data, int offset, int length )
		{
			_data = data;
			_offset = offset;
			_length = length;
		}

		public bool Empty
		{
			get
			{
				for( int i = _offset; i < _offset + _length; i++ )
					if( _data[i] != 0 )
						return false;
				return true;
			}
		}
		public override string ToString()
		{
			if( Empty )
				return string.Empty;
			var sb = new StringBuilder();
			sb.Append( "Entry" );
			return sb.ToString();
		}
	}
}