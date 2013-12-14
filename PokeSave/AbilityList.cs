namespace PokeSave
{
	public static class AbilityList
	{
		static readonly StringList Abilities;

		static AbilityList()
		{
			if( Abilities == null )
				Abilities = new StringList( "abilities.bin" );
		}

		public static string Get( uint index )
		{
			return Abilities.Get( index );
		}
	}
}