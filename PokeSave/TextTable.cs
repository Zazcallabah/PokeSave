using System.Text;

namespace PokeSave
{
	public static class TextTable
	{
		static readonly char[] _data = new[]
		{
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ',
			' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!', '?', '.', '-', ' ',
			'-', '"', '"', '\'', '\'', 'm', 'f', ' ', ',', ' ', '/', 'A', 'B', 'C', 'D', 'E',
			'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
			'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
			'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', ' ',
			' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '>', ' ', ' ', '+', '_'
		};

		static byte IndexOf( char c )
		{
			for( int i = 0; i < _data.Length; i++ )
			{
				if( _data[i] == c )
					return (byte) i;
			}
			return 0xFF;
		}

		public static void WriteStringRaw( GameSection b, string data, int offset, int length )
		{
			WriteStringInternal( b, data, offset, length, false );
		}

		public static void WriteString( GameSection b, string data, int offset, int length )
		{
			WriteStringInternal( b, data, offset, length, true );
		}

		static void WriteStringInternal( GameSection b, string data, int offset, int length, bool guard )
		{
			for( int stringindex = 0; stringindex < length; stringindex++ )
			{
				var arrayindex = offset + stringindex;
				if( stringindex >= data.Length || ( guard && arrayindex == ( offset + length - 1 ) ) )
					b[arrayindex] = 0xFF;
				else
					b[arrayindex] = IndexOf( data[stringindex] );

			}
		}

		public static string ReadStringRaw( GameSection b, int offset, int length )
		{
			var sb = new StringBuilder();
			for( int i = offset; i < offset + length; i++ )
				sb.Append( _data[b[i]] );
			return sb.ToString();
		}

		public static string ReadString( GameSection b, int index, int length )
		{
			var sb = new StringBuilder();
			for( int i = index; i < index + length; i++ )
			{
				byte c = b[i];
				if( c == 255 )
					break;
				sb.Append( _data[c] );
			}
			return sb.ToString();
		}
	}
}