using System.Text;

namespace PokeSave
{
	public static class Extensions
	{
		/// <summary>
		/// origin = 11111111
		/// mask   = 00001110
		/// value  = 01010101
		/// 
		/// result = 11110101
		/// </summary>
		public static uint Mask( this uint origin, uint mask, uint value )
		{
			return ( value & mask ) | ( origin & ~mask );
		}

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