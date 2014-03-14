using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PokeSave
{
	public static class MonsterList
	{
		static Dictionary<uint, MonsterInfo> _dex;

		static void Init()
		{
			if( _dex != null )
				return;

			_dex = new Dictionary<uint, MonsterInfo>();

			using( var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( "PokeSave.Resources.dex.bin" ) )
			{
				var data = new byte[11536];
				stream.Read( data, 0, data.Length );
				for( uint offset = 0, index = 1; offset < data.Length - 28; offset += 28, index++ )
					_dex.Add( index, new MonsterInfo( data, index, offset ) );
			}
		}

		public static MonsterInfo Get( string name )
		{
			Init();
			var NAME = name.ToUpperInvariant();
			return _dex.Values.FirstOrDefault( e => e.Name == NAME );
		}

		public static MonsterInfo Get( uint index )
		{
			Init();
			if( _dex.ContainsKey( index ) )
				return _dex[index];
			return _dex.Values.Last();
		}
	}
}