namespace PokeSave
{
	public static class MoveList
	{
		static StringList _moves;

		public static string Get( uint index )
		{
			if( _moves == null )
				_moves = new StringList( "moves.bin" );
			return _moves.Get( index );
		}

		public static string[] All()
		{
			if( _moves == null )
				_moves = new StringList( "moves.bin" );
			return _moves.All();
		}

		public static uint First( string value )
		{
			if( _moves == null )
				_moves = new StringList( "moves.bin" );
			return _moves.First( value );
		}
	}
}