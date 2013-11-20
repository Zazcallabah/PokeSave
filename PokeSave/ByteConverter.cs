using System;
using System.IO;

namespace PokeSave
{
	public static class ByteConverter
	{
		public static int ToInt( byte[] arr, int index )
		{
			return ( arr[index + 3] << 24 )
				| ( arr[index + 2] << 16 )
					| ( arr[index + 1] << 8 )
						| arr[index];
		}
		public static int ToShort( byte[] arr, int index )
		{
			return ( arr[index + 1] << 8 ) | arr[index];
		}

		public static int ReadInt( Stream stream )
		{
			var buffer = new byte[4];
			int check = stream.Read( buffer, 0, 4 );
			if( check != 4 )
				throw new ArgumentException( "Stream ended prematurely" );
			Array.Reverse( buffer );
			return BitConverter.ToInt32( buffer, 0 );
		}

		public static int ReadShort( Stream stream )
		{
			var buffer = new byte[2];
			int check = stream.Read( buffer, 0, 2 );
			if( check != 2 )
				throw new ArgumentException( "Stream ended prematurely" );
			Array.Reverse( buffer );
			return BitConverter.ToInt16( buffer, 0 );
		}
	}
}