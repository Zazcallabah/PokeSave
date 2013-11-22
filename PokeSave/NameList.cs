namespace PokeSave
{
	public static class NameList
	{
		static StringList _names;

		public static string Get( uint index )
		{
			if( _names == null )
				_names = new StringList( "names.bin" );
			return _names.Get( index );
		}
	}
}