namespace PokeSave
{
	public static class ItemList
	{
		static StringList _items;

		public static string Get( uint index )
		{
			if( _items == null )
				_items = new StringList( "items.bin" );

			return _items.Get( index );
		}
	}
}