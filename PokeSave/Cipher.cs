namespace PokeSave
{
	public class Cipher
	{
		readonly uint _key;

		public Cipher( uint key1, uint key2 )
		{
			_key = key1 ^ key2;
		}

		public Cipher( uint key )
		{
			_key = key;
		}

		public uint Run( uint data )
		{
			return _key ^ data;
		}

		public uint RunLower( uint data )
		{
			return ( _key & 0xffff ) ^ ( data & 0xffff );
		}
	}
}