using System.Text;

namespace PokeSave
{
	public static class TextTable
	{
		readonly static char[] _data = new[]
		{
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ',' ',' ',' ',' ',' ',
		
			' ','0','1','2','3','4','5','6',   '7','8','9','!','?','.','-',' ',
			'-','"','"','\'','\'','m','f',' ', ',',' ','/','A','B','C','D','E',
			'F','G','H','I','J','K','L','M',   'N','O','P','Q','R','S','T','U',
			'V','W','X','Y','Z','a','b','c',   'd','e','f','g','h','i','j','k',
			'l','m','n','o','p','q','r','s',   't','u','v','w','x','y','z',' ',
			' ',' ',' ',' ',' ',' ',' ',' ',   ' ',' ',' ','>',' ',' ','+','_'
		};

		public static string ConvertArrayRaw( GameSection b, int index, int length )
		{
			var sb = new StringBuilder();
			for( int i = index; i < index + length; i++ )
			{
				sb.Append( _data[b[i]] );
			}
			return sb.ToString();
		}

		public static string ConvertArray( GameSection b, int index, int length )
		{
			var sb = new StringBuilder();
			for( int i = index; i < index + length; i++ )
			{
				var c = b[i];
				if( c == 255 )
					break;
				sb.Append( _data[c] );
			}
			return sb.ToString();
		}
	}
}