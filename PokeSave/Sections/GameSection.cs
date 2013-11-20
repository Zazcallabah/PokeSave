using System;
using System.IO;
using System.Text;

namespace PokeSave.Sections
{
	public abstract class GameSection
	{
		protected readonly byte[] Data;

		protected GameSection( Stream instream, int length )
		{
			UsedLength = length;
			Data = new byte[4080];
			var count = instream.Read( Data, 0, Data.Length );
			if( count != Data.Length )
				throw new ArgumentException( "file too short" );

			var tail = new byte[16];
			count = instream.Read( tail, 0, 16 );
			if( count != 16 )
				throw new ArgumentException( "file too short" );

			ID = ByteConverter.ToShort( tail, 4 );
			Checksum = ByteConverter.ToShort( tail, 6 );
			SaveIndex = ByteConverter.ToInt( tail, 0xC );

			var chk = 0;
			for( int i = 0; i < UsedLength; i += 4 )
			{
				chk += ByteConverter.ToInt( Data, i );
			}

			var upper = ( chk >> 16 ) & 0xFFFF;
			var lower = chk & 0xFFFF;
			CalculatedChecksum = upper + lower;
		}

		public int UsedLength { get; private set; }
		public int Checksum { get; private set; }
		public int ID { get; private set; }
		public int SaveIndex { get; private set; }

		public int CalculatedChecksum { get; private set; }

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( GetType().Name );
			sb.AppendLine( "ID: " + ID );
			sb.AppendLine( "Checksum : " + Checksum );
			sb.AppendLine( "Checksum2: " + CalculatedChecksum );
			sb.AppendLine( "Index: " + SaveIndex );
			sb.AppendLine( "Length: " + UsedLength );
			return sb.ToString();
		}
	}
}