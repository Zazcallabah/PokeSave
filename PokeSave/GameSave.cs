using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PokeSave
{
	public class GameSave
	{
		readonly List<GameSection> _sections;
		readonly List<GameSection> _originalOrderSections;

		readonly Dictionary<GameType, Dictionary<string, int>> _pointers = new Dictionary<GameType, Dictionary<string, int>>
			{
				{ GameType.FRLG, new Dictionary<string,int> {
					{"Name",0},
					{"Gender",8},
					{"PublicId",0xA},
					{"SecretId",0xC},
					{"TimeHours",0xE},
					{"TimeMinutes",0x10},
					{"TimeSeconds",0x11},
					{"TimeFrames",0x12},
					{"GameCode",0xAC},
					{"SecurityKey",0xAF8},

					{"TeamSize",0x34},
					{"TeamList",0x38},
					{"Money",0x290},

					{"PCItems",0x298},
					{"PCItemsLength",30},
					{"Items",0x310},
					{"ItemsLength",42},
					{"KeyItems",0x3B8},
					{"KeyItemsLength",30},
					{"BallPocket",0x430},
					{"BallPocketLength",13},
					{"TMCase",0x464},
					{"TMCaseLength",58},
					{"Berries",0x54c},
					{"BerriesLength",43},

					{"Rival",0xBCC},
				}},
				{ GameType.E, new Dictionary<string,int>{}},
				{ GameType.RS, new Dictionary<string,int>{}},
			};

		public GameSave( Stream instream )
		{
			_sections = new List<GameSection>();
			_originalOrderSections = new List<GameSection>();

			for( int i = 0; i < 14; i++ )
				_originalOrderSections.Add( new GameSection( instream ) );
			_sections = _originalOrderSections.OrderBy( s => s.ID ).ToList();

			ExtractType();

			Xor = new Cipher( SecurityKey );

			ExtractTeam();
			ExtractPcBuffer();
			ExtractItems();
		}

		void ExtractTeam()
		{
			Team = new MonsterEntry[6];
			for( int i = 0; i < Team.Length; i++ )
			{
				Team[i] = new MonsterEntry( _sections[1], _pointers[Type]["TeamList"] + ( i * 100 ), false );
			}
		}

		void ExtractItems()
		{
			PCItems = new ItemEntry[_pointers[Type]["PCItemsLength"]];
			for( int i = 0; i < PCItems.Length; i++ )
				PCItems[i] = new ItemEntry( _sections[1], _pointers[Type]["PCItems"] + ( i * 4 ) );

			Items = new ItemEntry[_pointers[Type]["ItemsLength"]];
			for( int i = 0; i < Items.Length; i++ )
				Items[i] = new ItemEntry( _sections[1], _pointers[Type]["Items"] + ( i * 4 ), Xor );

			KeyItems = new ItemEntry[_pointers[Type]["KeyItemsLength"]];
			for( int i = 0; i < KeyItems.Length; i++ )
				KeyItems[i] = new ItemEntry( _sections[1], _pointers[Type]["KeyItems"] + ( i * 4 ), Xor );

			BallPocket = new ItemEntry[_pointers[Type]["BallPocketLength"]];
			for( int i = 0; i < BallPocket.Length; i++ )
				BallPocket[i] = new ItemEntry( _sections[1], _pointers[Type]["BallPocket"] + ( i * 4 ), Xor );

			TMCase = new ItemEntry[_pointers[Type]["TMCaseLength"]];
			for( int i = 0; i < TMCase.Length; i++ )
				TMCase[i] = new ItemEntry( _sections[1], _pointers[Type]["TMCase"] + ( i * 4 ), Xor );

			Berries = new ItemEntry[_pointers[Type]["BerriesLength"]];
			for( int i = 0; i < Berries.Length; i++ )
				Berries[i] = new ItemEntry( _sections[1], _pointers[Type]["Berries"] + ( i * 4 ), Xor );
		}

		void ExtractPcBuffer()
		{
			var buffer = new PcBuffer( _sections.Skip( 5 ).ToArray() );
			PcBuffer = new MonsterEntry[420];
			for( int i = 0; i < PcBuffer.Length; i++ )
				PcBuffer[i] = new MonsterEntry( buffer, 4 + ( 80 * i ), true );
		}

		void ExtractType()
		{
			var code = _sections[0].GetInt( 0xAC );
			if( code == 0 )
				Type = GameType.RS;
			else if( code == 1 )
				Type = GameType.FRLG;
			else
				Type = GameType.E;
		}

		Cipher Xor { get; set; }

		public uint SaveIndex
		{
			get
			{
				var index = _sections[0].SaveIndex;
				foreach( var s in _sections )
				{
					if( s.SaveIndex != index )
					{
						throw new InvalidOperationException( "Differing save indexes" );
					}
				}
				return index;
			}
		}

		public GameType Type { get; private set; }
		public MonsterEntry[] Team { get; private set; }
		public MonsterEntry[] PcBuffer { get; private set; }
		public ItemEntry[] PCItems { get; private set; }
		public ItemEntry[] Items { get; private set; }
		public ItemEntry[] KeyItems { get; private set; }
		public ItemEntry[] BallPocket { get; private set; }
		public ItemEntry[] TMCase { get; private set; }
		public ItemEntry[] Berries { get; private set; }

		public string Name
		{
			get
			{
				return _sections[0].GetText( _pointers[Type]["Name"], 8 );
			}
		}

		public string Rival
		{
			get
			{
				return _sections[4].GetText( _pointers[Type]["Rival"], 8 );
			}
		}

		public string Gender
		{
			get
			{
				return _sections[0][_pointers[Type]["Gender"]] == 0 ? "Boy" : "Girl";
			}
		}

		public uint PublicId
		{
			get
			{
				return _sections[0].GetShort( _pointers[Type]["PublicId"] );
			}
		}

		public uint SecretId
		{
			get
			{
				return _sections[0].GetShort( _pointers[Type]["SecretId"] );
			}
		}

		public string TimePlayed
		{
			get
			{
				var h = _sections[0].GetShort( _pointers[Type]["TimeHours"] );
				var m = _sections[0][_pointers[Type]["TimeMinutes"]];
				var s = _sections[0][_pointers[Type]["TimeSeconds"]];
				var f = _sections[0][_pointers[Type]["TimeFrames"]];

				return string.Format( "{0}h{1}m{2}s{3}f", h, m, s, f );
			}
		}

		public uint GameCode
		{
			get
			{
				return _sections[0].GetShort( _pointers[Type]["GameCode"] );
			}
		}

		public uint SecurityKey
		{
			get
			{
				return _sections[0].GetInt( _pointers[Type]["SecurityKey"] );
			}
		}

		public uint TeamSize
		{
			get
			{
				return _sections[1].GetInt( _pointers[Type]["TeamSize"] );
			}
		}

		public uint Money
		{
			get
			{
				return Xor.Run( _sections[1].GetInt( _pointers[Type]["Money"] ) );
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( "Type:\t" + Type );
			sb.AppendLine( "\tName:\t" + Name );
			sb.AppendLine( "\tGender:\t" + Gender );
			sb.AppendLine( "\tPublic id:\t" + PublicId );
			sb.AppendLine( "\tSecret:\t" + SecretId );
			sb.AppendLine( "\tKey:\t" + SecurityKey );
			sb.AppendLine( "\tTime:\t" + TimePlayed );
			sb.AppendLine( "\tGamecode:\t" + GameCode );
			sb.AppendLine( "\tMoney:\t" + Money );
			sb.AppendLine( "\tRival:\t" + Rival );

			sb.AppendLine( "Teamsize:\t" + TeamSize );
			foreach( var m in Team )
				sb.Append( m.ToString() );

			sb.AppendLine( "PC Items:" );
			foreach( var m in PCItems )
				sb.Append( m );

			sb.AppendLine( "Bag Items:" );
			foreach( var m in Items )
				sb.Append( m );

			sb.AppendLine( "Key Items:" );
			foreach( var m in KeyItems )
				sb.Append( m );

			sb.AppendLine( "Ball pocket:" );
			foreach( var m in BallPocket )
				sb.Append( m );
			sb.AppendLine( "TM case:" );

			foreach( var m in TMCase )
				sb.Append( m );
			sb.AppendLine( "Berries:" );

			foreach( var m in Berries )
				sb.Append( m );


			sb.AppendLine( "PC buffer:" );
			for( int i = 0; i < PcBuffer.Length; i++ )
			{
				if( i % 30 == 0 )
					sb.AppendLine( "PC Box #" + Math.Floor( i / 30.0 ) );
				sb.Append( PcBuffer[i] );
			}
			return sb.ToString();


		}

		public void Save( Stream stream )
		{
			foreach( var s in _originalOrderSections )
			{
				s.Write( stream );
			}
		}
	}
}