using System.IO;
using System.Text;

namespace PokeSave.Sections
{
	public class TrainerSection : GameSection
	{
		const int Size = 3884;

		public TrainerSection( Stream instream )
			: base( instream, Size )
		{ }

		public string Name
		{
			get
			{
				return TextTable.ConvertArray( Data, 0, 8 );
			}
		}

		public string Gender
		{
			get
			{
				return Data[8] == 0 ? "Boy" : "Girl";
			}
		}

		public int PublicId
		{
			get
			{
				return ByteConverter.ToShort( Data, 0xA );
			}
		}

		public int SecretId
		{
			get
			{
				return ByteConverter.ToShort( Data, 0xC );
			}
		}

		public string TimePlayed
		{
			get
			{
				var h = ByteConverter.ToShort( Data, 0xE );

				return string.Format( "{0}h{1}m{2}s{3}f", h, Data[0xA0], Data[0xA1], Data[0xA2] );

			}
		}

		public string GameCode
		{
			get
			{
				var gc = ByteConverter.ToShort( Data, 0xAC );
				if( gc == 0 )
				{
					return "RS: gamecode 0, key 0";
				}
				if( gc == 1 )
				{
					return string.Format( "FRLG: gamecode 1, key {0}", ByteConverter.ToInt( Data, 0xAF8 ) );
				}
				return string.Format( "E: key {0}", gc );
			}
		}

		public int SecretKey
		{
			get
			{
				var gc = ByteConverter.ToInt( Data, 0xAC );
				if( gc == 1 )
				{
					return ByteConverter.ToInt( Data, 0xAF8 );
				}
				return gc;
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( base.ToString().TrimEnd() );
			sb.AppendLine( "\tName:\t" + Name );
			sb.AppendLine( "\tGender:\t" + Gender );
			sb.AppendLine( "\tPublic id:\t" + PublicId );
			sb.AppendLine( "\tSecret:\t" + SecretId );
			sb.AppendLine( "\tTime:\t" + TimePlayed );
			sb.AppendLine( "\t" + GameCode );
			return sb.ToString();
		}
	}
}