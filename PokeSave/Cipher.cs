using System;

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

		public uint Key
		{
			get { return _key; }
		}

		public uint Run( uint data )
		{
			return _key ^ data;
		}

		public uint RunLower( uint data )
		{
			return ( _key & 0xffff ) ^ ( data & 0xffff );
		}

		public uint RunHigher( uint data )
		{
			return ( _key >> 16 ) ^ ( data & 0xffff );
		}

		public uint RunByte( uint data, int section )
		{
			return ( ( _key >> ( 8 * section ) ) & 0xff ) ^ ( data & 0xff );
		}

		public Func<uint, uint> Selector( bool high )
		{
			return high ? (Func<uint, uint>) RunLower : RunHigher;
		}
	}
}