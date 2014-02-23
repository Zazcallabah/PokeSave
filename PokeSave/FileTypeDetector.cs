using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokeSave
{
	public class FileTypeDetector
	{
		static int GetShort( byte[] b, int index )
		{
			return b[index] | ( b[index + 1] << 8 );
		}

		public static bool IsGen3SaveFile( byte[] data )
		{
			return data.Length == 128 * 1024;
		}

		public static bool Is3gpkm( byte[] data )
		{
			if( data.Length == 80 || data.Length == 100 )
			{
				var sum = 0;
				for( int i = 32; i < 80; i += 2 )
				{
					sum += GetShort( data, i );
				}

				return GetShort( data, 28 ) == ( sum & 0xffff );
			}
			return false;
		}

		public static IEnumerable<MonsterEntry> Open( byte[] data )
		{
			if( IsGen3SaveFile( data ) )
				return new MonsterEntry[0];

			if( Is3gpkm( data ) )
				return new[] { new MonsterEntry( data, true ) };

			var b64 = TryExtractByte64Data( data );
			if( b64 != null )
			{
				return b64.Select( b => new MonsterEntry( b, false ) ).ToArray();
			}

			if( data.Length >= 80 )
			{
				return new[] { new MonsterEntry( data, false ) };
			}

			return new MonsterEntry[0];
		}

		static IEnumerable<byte[]> TryExtractByte64Data( byte[] data )
		{
			try
			{
				var str = Encoding.UTF8.GetString( data );
				var rows = str.Split( new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries );
				if( rows.All( s => s.Length == 108 || s.Length == 136 ) )
				{
					return rows.Select( Convert.FromBase64String );
				}
				return null;
			}
			catch( Exception )
			{
				return null;
			}
		}

	}
}