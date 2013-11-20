namespace PokeSave
{
	public static class Cipher
	{
		static int? _key;

		public static void Init( int key )
		{
			if( _key == null )
				_key = key;
		}

		public static int Run( int data )
		{
			if( !_key.HasValue )
				throw new System.InvalidOperationException( "Tried to decrypt without key" );

			return _key.Value ^ data;
		}
	}
}