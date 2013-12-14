namespace PokeSave
{
	public static class NameList
	{
		static readonly StringList Names;

		static NameList()
		{
			if( Names == null )
				Names = new StringList( "names.bin" );
		}

		public static string Get( uint index )
		{
			return Names.Get( index );
		}

		public static uint First( string value )
		{
			return Names.First(value);
		}

		public static string[] All()
		{
			return Names.All();
		}
	}
}