namespace PokeSave
{
	public static class ItemList
	{
		static readonly StringList Items;

		static ItemList()
		{
			if( Items == null )
				Items = new StringList( "items.bin" );
		}

		public static string Get( uint index )
		{
			return Items.Get( index );
		}

		public static uint First( string value )
		{
			return Items.First( value );
		}

		public static string[] All()
		{
			return Items.All();
		}
	}
}