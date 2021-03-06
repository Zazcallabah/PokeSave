using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace PokeSave
{
	public class GameSave : INotifyPropertyChanged
	{
		readonly List<GameSection> _originalOrderSections;
		readonly List<GameSection> _sections;

		GameName _gameTypeGuess;
		bool _isDirty;

		#region _pointers
		public readonly Dictionary<GameType, Dictionary<string, int>> _pointers = new Dictionary
			<GameType, Dictionary<string, int>>
		{
			{
				GameType.FRLG, new Dictionary<string, int>
				{
					{ "Name", 0 },
					{ "Gender", 8 },
					{ "PublicId", 0xA },
					{ "SecretId", 0xC },
					{ "TimeHours", 0xE },
					{ "TimeMinutes", 0x10 },
					{ "TimeSeconds", 0x11 },
					{ "TimeFrames", 0x12 },
					{ "GameCode", 0xAC },
					{ "SecurityKey", 0xAF8 },
					{ "TeamSize", 0x34 },
					{ "TeamList", 0x38 },
					{ "Money", 0x290 },
					{ "Coins", 0x294 },
					{ "PCItems", 0x298 },
					{ "PCItemsLength", 30 },
					{ "Items", 0x310 },
					{ "ItemsLength", 42 },
					{ "KeyItems", 0x3B8 },
					{ "KeyItemsLength", 30 },
					{ "BallPocket", 0x430 },
					{ "BallPocketLength", 13 },
					{ "TMCase", 0x464 },
					{ "TMCaseLength", 58 },
					{ "Berries", 0x54c },
					{ "BerriesLength", 43 },
					{ "Rival", 0xBCC },
					{ "DexSeenOffset1", 0x5F8 },
					{ "DexSeenOffset2", 0xB98 },
					{ "NationalDexOffset1", 0x1B },
					{ "NationalDexOffset1Value1", 0xB9 },
					{ "NationalDexOffset1Value2", -1 },
					{ "NationalDexOffset2", 0x68 },
					{ "NationalDexIndex2", 0 },
					{ "NationalDexOffset3", 0x11C },
					{ "NationalDexOffset3Value1", 0x58 },
					{ "NationalDexOffset3Value2", 0x62 },
					{ "BoxNames", 0x8344 },
					{ "BoxWallpapers", 0x83C2 },
				}
			},
			{
				GameType.E, new Dictionary<string, int>
				{
					{ "Name", 0 },
					{ "Gender", 8 },
					{ "PublicId", 0xA },
					{ "SecretId", 0xC },
					{ "TimeHours", 0xE },
					{ "TimeMinutes", 0x10 },
					{ "TimeSeconds", 0x11 },
					{ "TimeFrames", 0x12 },
					{ "GameCode", 0xAC },
					{ "SecurityKey", 0xAC },
					{ "TeamSize", 0x234 },
					{ "TeamList", 0x238 },
					{ "Money", 0x490 },
					{ "Coins", 0x494 },
					{ "PCItems", 0x498 },
					{ "PCItemsLength", 50 },
					{ "Items", 0x560 },
					{ "ItemsLength", 30 },
					{ "KeyItems", 0x5D8 },
					{ "KeyItemsLength", 30 },
					{ "BallPocket", 0x650 },
					{ "BallPocketLength", 16 },
					{ "TMCase", 0x690 },
					{ "TMCaseLength", 64 },
					{ "Berries", 0x790 },
					{ "BerriesLength", 46 },
					{ "Rival", -1 },
					{ "DexSeenOffset1", 0x988 },
					{ "DexSeenOffset2", 0xCA4 },
					{ "NationalDexOffset1", 0x19 },
					{ "NationalDexOffset1Value1", 1 },
					{ "NationalDexOffset1Value2", 0xDA },
					{ "NationalDexOffset2", 0x402 },
					{ "NationalDexIndex2", 6 },
					{ "NationalDexOffset3", 0x4A8 },
					{ "NationalDexOffset3Value1", 2 },
					{ "NationalDexOffset3Value2", 3 },
					{ "BoxNames", 0x8344 },
					{ "BoxWallpapers", 0x83C2 },
				}
			},
			{
				GameType.RS, new Dictionary<string, int>
				{
					{ "Name", 0 },
					{ "Gender", 8 },
					{ "PublicId", 0xA },
					{ "SecretId", 0xC },
					{ "TimeHours", 0xE },
					{ "TimeMinutes", 0x10 },
					{ "TimeSeconds", 0x11 },
					{ "TimeFrames", 0x12 },
					{ "GameCode", 0xAC },
					{ "SecurityKey", 0xAC },
					{ "TeamSize", 0x234 },
					{ "TeamList", 0x238 },
					{ "Money", 0x490 },
					{ "Coins", 0x494 },
					{ "PCItems", 0x498 },
					{ "PCItemsLength", 50 },
					{ "Items", 0x560 },
					{ "ItemsLength", 20 },
					{ "KeyItems", 0x5B0 },
					{ "KeyItemsLength", 20 },
					{ "BallPocket", 0x600 },
					{ "BallPocketLength", 16 },
					{ "TMCase", 0x640 },
					{ "TMCaseLength", 64 },
					{ "Berries", 0x740 },
					{ "BerriesLength", 46 },
					{ "Rival", -1 },
					{ "DexSeenOffset1", 0x938 },
					{ "DexSeenOffset2", 0xC0C },
					{ "NationalDexOffset1", 0x19 },
					{ "NationalDexOffset1Value1", 1 },
					{ "NationalDexOffset1Value2", 0xDA },
					{ "NationalDexOffset2", 0x3A6 },
					{ "NationalDexIndex2", 6 },
					{ "NationalDexOffset3", 0x44C },
					{ "NationalDexOffset3Value1", 0x2 },
					{ "NationalDexOffset3Value2", 0x3 },
					{ "BoxNames", 0x8344 },
					{ "BoxWallpapers", 0x83C2 },
				}
			}
		};
		#endregion

		public GameSave( Stream instream )
		{
			_sections = new List<GameSection>();
			_originalOrderSections = new List<GameSection>();

			for( int i = 0; i < 14; i++ )
			{
				var section = new GameSection( instream );
				section.PropertyChanged += BubbleIsDirty;
				_originalOrderSections.Add( section );
			}
			_sections = _originalOrderSections.OrderBy( s => s.ID ).ToList();

			ExtractType();

			Xor = new Cipher( SecurityKey );

			Dex = new BindingList<DexEntry>();
			for( var i = 0; i < 416; i++ )
				Dex.Add( new DexEntry( i, this ) );

			ExtractTeam();
			ExtractPcBuffer();
			ExtractItems();
			GuessGame();
		}

		public Dictionary<string, int> Pointers
		{
			get { return _pointers[Type]; }
		}

		public bool IsDirty
		{
			get { return _isDirty; }
			set
			{
				if( IsDirty != value )
				{
					_isDirty = value;
					InvokePropertyChanged( "IsDirty" );
				}
			}
		}

		Cipher Xor { get; set; }

		public GameName GameTypeGuess
		{
			get { return _gameTypeGuess; }
			set
			{
				if( _gameTypeGuess != value )
				{
					_gameTypeGuess = value;
					InvokePropertyChanged( "GameTypeGuess" );
				}
			}
		}

		public uint SaveIndex
		{
			get
			{
				uint index = _sections[0].SaveIndex;
				if( _sections.Any( s => s.SaveIndex != index ) )
					RepairSaveIndexes();
				return index;
			}
		}

		public void RepairSaveIndexes()
		{
			uint largest = 0;
			foreach( var s in _sections )
			{
				if( s.SaveIndex > largest )
					largest = s.SaveIndex;
			}
			foreach( var s in _sections.Where( s => s.SaveIndex != largest ) )
			{
				s.SaveIndex = largest;
			}
		}

		/// <summary>
		///     Checks subsection save indexes and checksums. This will be false if save file has been changed without calling
		///     FixChecksum()
		/// </summary>
		public bool Valid
		{
			get
			{
				return _sections.All( s => s.Checksum == s.CalculatedChecksum )
					   && _sections.All( s => s.SaveIndex == _sections[0].SaveIndex );
			}
		}

		public GameType Type { get; private set; }
		public BindingList<MonsterEntry> Team { get; private set; }
		public BindingList<MonsterEntry> PcBuffer { get; private set; }
		public BindingList<ItemEntry> PCItems { get; private set; }
		public BindingList<ItemEntry> Items { get; private set; }
		public BindingList<ItemEntry> KeyItems { get; private set; }
		public BindingList<ItemEntry> BallPocket { get; private set; }
		public BindingList<ItemEntry> TMCase { get; private set; }
		public BindingList<ItemEntry> Berries { get; private set; }
		public BindingList<DexEntry> Dex { get; private set; }

		public BindingList<Box> Boxes { get; private set; }

		public string Name
		{
			get { return _sections[0].GetText( Pointers["Name"], 8 ); }
			set
			{
				if( Name != value )
				{
					_sections[0].SetText( Pointers["Name"], 8, value );
					InvokePropertyChanged( "Name" );
				}
			}
		}

		public string Rival
		{
			get
			{
				if( Type == GameType.FRLG )
					return _sections[4].GetText( Pointers["Rival"], 8 );
				return "";
			}
			set
			{
				if( Rival != value )
				{
					_sections[4].SetText( Pointers["Rival"], 8, value );
					InvokePropertyChanged( "Rival" );
				}
			}
		}

		public string Gender
		{
			get { return GenderByte == 0 ? "Boy" : "Girl"; }
			set { GenderByte = (byte) ( "Boy" == value ? 0 : 1 ); }
		}

		public byte GenderByte
		{
			get { return _sections[0][_pointers[Type]["Gender"]]; }
			set
			{
				if( GenderByte != value )
				{
					_sections[0][_pointers[Type]["Gender"]] = value;
					InvokePropertyChanged( "Gender" );
					InvokePropertyChanged( "GenderByte" );
				}
			}
		}

		/// <summary>
		///     Writing these is not currently supported
		/// </summary>
		public uint PublicId
		{
			get { return _sections[0].GetShort( Pointers["PublicId"] ); }
			private set { _sections[0].SetShort( Pointers["PublicId"], value ); }
		}

		/// <summary>
		///     Writing these is not currently supported
		/// </summary>
		public uint SecretId
		{
			get { return _sections[0].GetShort( Pointers["SecretId"] ); }
			private set { _sections[0].SetShort( Pointers["SecretId"], value ); }
		}

		public string TimePlayed
		{
			get
			{
				uint h = _sections[0].GetShort( Pointers["TimeHours"] );
				byte m = _sections[0][_pointers[Type]["TimeMinutes"]];
				byte s = _sections[0][_pointers[Type]["TimeSeconds"]];
				byte f = _sections[0][_pointers[Type]["TimeFrames"]];

				return string.Format( "{0}h{1}m{2}s{3}f", h, m, s, f );
			}
		}

		public uint GameCode
		{
			get { return _sections[0].GetShort( Pointers["GameCode"] ); }
			private set { _sections[0].SetShort( Pointers["GameCode"], value ); }
		}

		public uint SecurityKey
		{
			get { return _sections[0].GetInt( Pointers["SecurityKey"] ); }
			private set { _sections[0].SetInt( Pointers["SecurityKey"], value ); }
		}

		public uint TeamSize
		{
			get { return _sections[1].GetInt( Pointers["TeamSize"] ); }
			set
			{
				if( value != TeamSize )
				{
					_sections[1].SetInt( Pointers["TeamSize"], value );
					InvokePropertyChanged( "TeamSize" );
				}
			}
		}

		public uint Money
		{
			get { return Xor.Run( _sections[1].GetInt( Pointers["Money"] ) ); }
			set
			{
				if( value != Money )
				{
					_sections[1].SetInt( Pointers["Money"], Xor.Run( value ) );
					InvokePropertyChanged( "Money" );
				}
			}
		}

		public uint Coins
		{
			get { return _sections[1].GetInt( Pointers["Coins"] ); }
			set
			{
				if( value != Coins )
				{
					_sections[1].SetInt( Pointers["Coins"], value );
					InvokePropertyChanged( "Coins" );
				}
			}
		}

		/// <summary>
		///     This sets publicid and secretid at the same time
		/// </summary>
		public uint TrainerId
		{
			get { return _sections[0].GetInt( Pointers["PublicId"] ); }
			private set { _sections[0].SetInt( Pointers["PublicId"], value ); }
		}

		public bool National
		{
			get { return _sections[2][Pointers["NationalDexOffset2"]].IsSet( Pointers["NationalDexIndex2"] ); }
			set
			{
				if( National != value )
				{
					if( value )
						_sections[2][Pointers["NationalDexOffset2"]].SetBit( Pointers["NationalDexIndex2"] );
					else
						_sections[2][Pointers["NationalDexOffset2"]].ClearBit( Pointers["NationalDexIndex2"] );

					_sections[0][Pointers["NationalDexOffset1"]] = (byte) Pointers["NationalDexOffset1Value1"];
					if( Pointers["NationalDexOffset1Value2"] != -1 )
						_sections[0][Pointers["NationalDexOffset1"] + 1] = (byte) Pointers["NationalDexOffset1Value2"];

					_sections[2][Pointers["NationalDexOffset3"]] = (byte) Pointers["NationalDexOffset3Value1"];
					_sections[2][Pointers["NationalDexOffset3"] + 1] = (byte) Pointers["NationalDexOffset3Value2"];

					InvokePropertyChanged( "National" );
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void BubbleIsDirty( object sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName == "IsDirty" )
				IsDirty = ( (GameSection) sender ).IsDirty;
		}

		void GuessGame()
		{
			var d = new Dictionary<uint, int>();
			IEnumerable<MonsterEntry> l = Team.Where( t => !t.Empty ).Concat( PcBuffer.Where( t => !t.Empty ) );
			foreach( MonsterEntry me in l )
			{
				if( !d.ContainsKey( me.GameOfOrigin ) )
					d.Add( me.GameOfOrigin, 0 );
				d[me.GameOfOrigin]++;
			}

			int max = 0;
			uint id = 4;
			foreach( var kvp in d )
				if( max < kvp.Value )
				{
					max = kvp.Value;
					id = kvp.Key;
				}
			GameTypeGuess = (GameName) id;
		}

		public void ClaimAll()
		{
			IEnumerable<MonsterEntry> l = Team.Where( t => !t.Empty ).Concat( PcBuffer.Where( t => !t.Empty ) );
			foreach( MonsterEntry me in l )
				me.MakeOwn( this );
		}

		public void SortPC()
		{
			var sorted = PcBuffer
				.Select( t => new SortedBoxObject { Mark = t.Mark, MonsterId = t.MonsterId, Data = t.RawData } )
				.OrderBy( o => o )
				.ToArray();

			for( int i = 0; i < PcBuffer.Count; i++ )
			{
				PcBuffer[i].PropertyChangedEnabled = false;
				PcBuffer[i].RawData = sorted[i].Data;
				PcBuffer[i].PropertyChangedEnabled = true;
			}
		}

		void ExtractTeam()
		{
			Team = new BindingList<MonsterEntry>();
			Team.ListChanged += ( a, e ) => InvokePropertyChanged( "Team" );
			for( int i = 0; i < 6; i++ )
				Team.Add( new MonsterEntry( _sections[1], Pointers["TeamList"] + ( i * 100 ), false ) );
		}

		void ExtractItems()
		{
			PCItems = new BindingList<ItemEntry>();
			PCItems.ListChanged += ( a, e ) => InvokePropertyChanged( "PCItems" );
			for( int i = 0; i < Pointers["PCItemsLength"]; i++ )
				PCItems.Add( new ItemEntry( _sections[1], Pointers["PCItems"] + ( i * 4 ) ) );

			Items = new BindingList<ItemEntry>();
			Items.ListChanged += ( a, e ) => InvokePropertyChanged( "Items" );
			for( int i = 0; i < Pointers["ItemsLength"]; i++ )
				Items.Add( new ItemEntry( _sections[1], Pointers["Items"] + ( i * 4 ), Xor ) );

			KeyItems = new BindingList<ItemEntry>();
			KeyItems.ListChanged += ( a, e ) => InvokePropertyChanged( "KeyItems" );
			for( int i = 0; i < Pointers["KeyItemsLength"]; i++ )
				KeyItems.Add( new ItemEntry( _sections[1], Pointers["KeyItems"] + ( i * 4 ), Xor ) );

			BallPocket = new BindingList<ItemEntry>();
			BallPocket.ListChanged += ( a, e ) => InvokePropertyChanged( "BallPocket" );
			for( int i = 0; i < Pointers["BallPocketLength"]; i++ )
				BallPocket.Add( new ItemEntry( _sections[1], Pointers["BallPocket"] + ( i * 4 ), Xor ) );

			TMCase = new BindingList<ItemEntry>();
			TMCase.ListChanged += ( a, e ) => InvokePropertyChanged( "TMCase" );
			for( int i = 0; i < Pointers["TMCaseLength"]; i++ )
				TMCase.Add( new ItemEntry( _sections[1], Pointers["TMCase"] + ( i * 4 ), Xor ) );

			Berries = new BindingList<ItemEntry>();
			Berries.ListChanged += ( a, e ) => InvokePropertyChanged( "Berries" );
			for( int i = 0; i < Pointers["BerriesLength"]; i++ )
				Berries.Add( new ItemEntry( _sections[1], Pointers["Berries"] + ( i * 4 ), Xor ) );
		}

		void ExtractPcBuffer()
		{
			var buffer = new PcBuffer( _sections.Skip( 5 ).ToArray() );
			Boxes = new BindingList<Box>();
			PcBuffer = new BindingList<MonsterEntry>();
			PcBuffer.ListChanged += ( a, e ) => InvokePropertyChanged( "PcBuffer" );
			for( int i = 0; i < 420; i++ )
			{
				var entry = new MonsterEntry( buffer, 4 + ( 80 * i ), true );
				var boxnumber = (int) Math.Floor( i / 30.0 );
				if( i % 30 == 0 )
					Boxes.Add( new Box( boxnumber ) );
				PcBuffer.Add( entry );
				Boxes[boxnumber].Content.Add( entry );
			}
		}

		void ExtractType()
		{
			uint code = _sections[0].GetInt( 0xAC );
			if( code == 0 )
				Type = GameType.RS;
			else if( code == 1 )
				Type = GameType.FRLG;
			else
				Type = GameType.E;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( "     Type: " + Type );
			sb.AppendLine( "SaveIndex: " + SaveIndex );
			sb.AppendLine( "     Name: " + Name );
			sb.AppendLine( "   Gender: " + Gender );
			sb.AppendLine( "Public ID: " + PublicId );
			sb.AppendLine( "Secret ID: " + SecretId );
			sb.AppendLine( "      Key: " + SecurityKey );
			sb.AppendLine( "     Time: " + TimePlayed );
			sb.AppendLine( " Gamecode: " + GameCode );
			sb.AppendLine( "    Money: " + Money );
			sb.AppendLine( "    Rival: " + Rival );

			sb.AppendLine( "Teamsize:\t" + TeamSize );
			for( int i = 0; i < Team.Count; i++ )
				sb.AppendIfNotEmpty( Team[i].Brief(), i );

			sb.AppendLine( "PC Items:" );
			for( int i = 0; i < PCItems.Count; i++ )
				sb.AppendIfNotEmpty( PCItems[i].ToString(), i );

			sb.AppendLine( "Bag Items:" );
			for( int i = 0; i < Items.Count; i++ )
				sb.AppendIfNotEmpty( Items[i].ToString(), i );

			sb.AppendLine( "Key Items:" );
			for( int i = 0; i < KeyItems.Count; i++ )
				sb.AppendIfNotEmpty( KeyItems[i].ToString(), i );

			sb.AppendLine( "Ball pocket:" );
			for( int i = 0; i < BallPocket.Count; i++ )
				sb.AppendIfNotEmpty( BallPocket[i].ToString(), i );

			sb.AppendLine( "TM case:" );
			for( int i = 0; i < TMCase.Count; i++ )
				sb.AppendIfNotEmpty( TMCase[i].ToString(), i );

			sb.AppendLine( "Berries:" );
			for( int i = 0; i < Berries.Count; i++ )
				sb.AppendIfNotEmpty( Berries[i].ToString(), i );

			sb.AppendLine( "PC buffer:" );
			string pre = string.Empty;
			for( int i = 0; i < PcBuffer.Count; i++ )
			{
				if( i % 30 == 0 )
					pre = "PC Box #" + Math.Floor( i / 30.0 );
				if( sb.AppendIfNotEmpty( PcBuffer[i].Brief(), i, pre ) )
					pre = string.Empty;
			}
			return sb.ToString();
		}

		public void Save( Stream stream )
		{
			foreach( MonsterEntry p in Team )
				p.Save();
			foreach( MonsterEntry p in PcBuffer )
				p.Save();

			foreach( GameSection s in _originalOrderSections )
				s.Write( stream );
		}

		public void InvokePropertyChanged( string e )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( e ) );
		}

		public GameSection Section( int index )
		{
			return _sections[index];
		}

		public void RepairPokeDex()
		{
			foreach( MonsterEntry p in Team.Where( t => !t.Empty ) )
				RepairPokeDex( p );
			foreach( MonsterEntry p in PcBuffer.Where( t => !t.Empty ) )
				RepairPokeDex( p );
		}

		public void Merge( GameSave external )
		{
			var ml = Team.Concat( PcBuffer ).ToList();
			var externallist = external.Team.Concat( external.PcBuffer ).ToList();
			var toadd = externallist.Where( e => !( ml.Any( m => m.MonsterId == e.MonsterId ) ) ).ToList();

			var l = LastEmptyIndex( PcBuffer.Count - 1 );
			foreach( var entry in toadd )
			{
				if( l == -1 )
					break;
				PcBuffer[l].RawData = entry.RawData;
				l = LastEmptyIndex( l );
			}
		}

		int LastEmptyIndex( int start )
		{
			for( int i = start; i >= 0; i-- )
				if( PcBuffer[i].Empty )
					return i;
			return -1;
		}

		public void RepairPokeDex( MonsterEntry m )
		{
			var dexEntry = Dex.FirstOrDefault( a => a.Name == m.TypeInformation.Name );
			if( dexEntry != null )
			{
				dexEntry.Seen = true;
				dexEntry.Owned = true;
			}
		}
	}
}