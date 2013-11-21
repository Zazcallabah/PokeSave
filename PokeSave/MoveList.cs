using System;
using System.Collections.Generic;
using System.IO;

namespace PokeSave
{
	public static class MoveList
	{
		static Dictionary<uint, string> _moves;

		public static void Init( string filename = "moves.bin" )
		{
			if( _moves != null )
				return;

			_moves = new Dictionary<uint, string>();
			foreach( var entry in File.ReadAllLines( filename ) )
			{
				var d = entry.Split( ',' );
				_moves.Add( UInt32.Parse( d[0] ), d[1] );
			}
		}

		public static string Get( int index )
		{
			return Get( (uint) index );
		}

		public static string Get( uint index )
		{
			Init();
			if( _moves.ContainsKey( index ) )
				return _moves[index];
			return _moves[0];
		}

		public static string Get( string index )
		{

			return Get( UInt32.Parse( index ) );
		}
	}
}