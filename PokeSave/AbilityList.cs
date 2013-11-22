namespace PokeSave
{
	public static class AbilityList
	{
		static StringList _abilities;

		public static string Get( uint index )
		{
			if( _abilities == null )
				_abilities = new StringList( "abilities.bin" );

			return _abilities.Get( index );
		}
	}
}