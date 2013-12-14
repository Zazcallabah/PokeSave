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
	}
}