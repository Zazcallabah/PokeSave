namespace PokeSave
{
	public static class MoveList
	{
		static readonly StringList Moves;

		static MoveList()
		{
			if( Moves == null )
				Moves = new StringList( "moves.bin" );
		}

		public static string Get( uint index )
		{
			return Moves.Get( index );
		}

		public static string[] All()
		{
			return Moves.All();
		}

		public static uint First( string value )
		{
			return Moves.First( value );
		}
	}
}