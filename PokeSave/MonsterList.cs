
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PokeSave
{
	public static class MonsterList
	{
		static Dictionary<uint, MonsterInfo> _dex;
		public static void Init( string filename = "dex.bin" )
		{
			if( _dex != null )
				return;

			_dex = new Dictionary<uint, MonsterInfo>();

			var data = File.ReadAllBytes( "dex.bin" );
			for( uint offset = 0, index = 1; offset < data.Length - 28; offset += 28, index++ )
			{
				_dex.Add( index, new MonsterInfo( data, index, offset ) );
			}
		}

		public static MonsterInfo Get( uint index )
		{
			Init();
			if( _dex.ContainsKey( index ) )
				return _dex[index];
			return _dex.Values.Last();
		}

		public static MonsterInfo Get( string index )
		{
			return Get( UInt32.Parse( index ) );
		}
	}
}

