using System.Text;

namespace PokeSave
{
	public static class Extensions
	{
		public static bool IsSet( this uint val, int pos )
		{
			return ( val & ( 1U << pos ) ) != 0;
		}


		public static uint SetBit( this uint val, int pos )
		{
			return val | ( 1U << pos );
		}

		public static uint ClearBit( this uint val, int pos )
		{
			return val & ~( 1U << pos );
		}

		public static bool AppendIfNotEmpty( this StringBuilder sb, string line, int index, string pre = "" )
		{
			if( !string.IsNullOrWhiteSpace( line ) )
			{
				if( !string.IsNullOrEmpty( pre ) )
					sb.AppendLine( pre );
				sb.AppendLine( "\t" + index + ": " + line );
				return true;
			}
			return false;
		}
	}
}