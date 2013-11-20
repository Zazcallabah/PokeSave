using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PokeSave.Sections
{
	public class TeamSection : GameSection
	{
		public TeamSection( Stream instream )
			: base( instream, 3968 )
		{

			TeamList = new List<MonsterEntry>();
			for( int i = 0; i < 6; i++ )
			{
				TeamList.Add( new MonsterEntry( Data, 0x38 + ( 100 * i ), 100 ) );
			}

			PcItems = new List<ItemEntry>();
			BagItems = new List<ItemEntry>();
			for( int i = 0; i < 30; i++ ) // 50 RS,E
			{
				PcItems.Add( new ItemEntry( Data, 0x298 + ( i * 4 ), true ) ); //0x498 E,RS
			}

			//236-50 E
			//216-50 RS
			for( int i = 0; i < 186; i++ )
			{
				BagItems.Add( new ItemEntry( Data, 0x310 + ( i * 4 ), false ) ); //0x560 E,RS
			}

		}

		public int TeamSize
		{
			get
			{
				return ByteConverter.ToInt( Data, 0x34 ); // 0x234 for RS & E
			}
		}
		public List<ItemEntry> PcItems { get; private set; }
		public List<ItemEntry> BagItems { get; private set; }
		public List<MonsterEntry> TeamList { get; private set; }

		public int MoneyEncrypted
		{
			get
			{
				return ByteConverter.ToInt( Data, 0x290 ); // 0x490 for RS & E
			}
		}

		public int Money
		{
			get
			{
				return Cipher.Run( MoneyEncrypted );
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( base.ToString().TrimEnd() );
			sb.Append( "\tMoney:\t" + Money );
			sb.AppendLine( "\tTeam Size:\t" + TeamSize );
			sb.AppendLine( "Team:" );
			foreach( var t in TeamList )
				sb.AppendLine( "\t" + t );
			sb.AppendLine( "PC items:" );
			foreach( var i in PcItems )
			{
				var data = i.ToString();
				if( !string.IsNullOrEmpty( data ) )
					sb.AppendLine( "\t" + data );
			}
			sb.AppendLine( "Bag items:" );
			foreach( var i in BagItems )
			{
				var data = i.ToString();
				if( !string.IsNullOrEmpty( data ) )
					sb.AppendLine( "\t" + data );
			}
			return sb.ToString();
		}
	}
}