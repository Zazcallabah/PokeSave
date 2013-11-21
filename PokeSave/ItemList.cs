﻿
using System;
using System.Collections.Generic;
using System.IO;

namespace PokeSave
{
	public static class ItemList
	{
		static Dictionary<uint, string> _names;
		public static void Init( string filename = "items.bin" )
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

		public static string Get( uint index )
		{
			Init();
			if( _names.ContainsKey( index ) )
				return _names[index];
			return "BAD ITEM ID";
		}

		public static string Get( string index )
		{
			Init();

			return _names[UInt32.Parse( index )];
		}
	}
}

