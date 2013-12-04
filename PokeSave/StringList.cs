using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PokeSave
{
	public class StringList
	{
		readonly Dictionary<uint, string> _data;

		public StringList( string filename )
		{
			if( _data != null )
				return;

			_data = new Dictionary<uint, string>();
			foreach( string entry in File.ReadAllLines( filename ) )
			{
				string[] d = entry.Split( ',' );
				_data.Add( UInt32.Parse( d[0] ), d[1] );
			}
		}

		public string Get( uint index )
		{
			if( _data.ContainsKey( index ) )
				return _data[index];
			return "BAD EGG";
		}

		public uint First( string value )
		{
			return ( from kvp in _data where kvp.Value == value select kvp.Key ).FirstOrDefault();
		}

		public string[] All()
		{
			return _data.Values.ToArray();
		}
	}
}