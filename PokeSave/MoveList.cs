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
	}
}