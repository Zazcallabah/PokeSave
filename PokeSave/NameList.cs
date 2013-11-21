using System;
using System.Collections.Generic;
using System.IO;

namespace PokeSave
{
	public static class NameList
	{
		static Dictionary<uint, string> _names;

		public static void Init( string filename = "names.bin" )
		{
			if( _names != null )
				return;

			_names = new Dictionary<uint, string>();
			foreach( var entry in File.ReadAllLines( filename ) )
			{
				var d = entry.Split( ',' );
				_names.Add( UInt32.Parse( d[0] ), d[1] );
			}
		}

		public static string Get( int index )
		{
			return Get( (uint) index );
		}

		public static string Get( uint index )
		{
			Init();
			if( _names.ContainsKey( index ) )
				return _names[index];
			return "BAD EGG";
		}

		public static string Get( string index )
		{

			return Get( UInt32.Parse( index ) );
		}
	}
}