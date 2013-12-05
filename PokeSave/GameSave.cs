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

		#region _pointers
		readonly Dictionary<GameType, Dictionary<string, int>> _pointers = new Dictionary<GameType, Dictionary<string, int>>
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
					{ "Rival", 0xBCC }
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
					{ "Rival", -1 }
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
					{ "Rival", -1 }
				}
			}
		};
		#endregion

		readonly List<GameSection> _sections;

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

		Cipher Xor { get; set; }

		public uint SaveIndex
		{
			get
			{
				uint index = _sections[0].SaveIndex;
				if( _sections.Any( s => s.SaveIndex != index ) )
					throw new InvalidOperationException( "Differing save indexes" );
				return index;
			}
		}

		/// <summary>
		/// Checks subsection save indexes and checksums.
		/// This will be false if save file has been changed without calling FixChecksum()
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

		public BindingList<Box> Boxes { get; private set; }
		public string Name
		{
			get { return _sections[0].GetText( _pointers[Type]["Name"], 8 ); }
			set
			{
				if( Name != value )
				{
					_sections[0].SetText( _pointers[Type]["Name"], 8, value );
					InvokePropertyChanged( "Name" );
				}
			}
		}

		public string Rival
		{
			get { return _sections[4].GetText( _pointers[Type]["Rival"], 8 ); }
			set
			{
				if( Rival != value )
				{
					_sections[4].SetText( _pointers[Type]["Rival"], 8, value );
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
		/// Writing these is not currently supported
		/// </summary>
		public uint PublicId
		{
			get { return _sections[0].GetShort( _pointers[Type]["PublicId"] ); }
			private set { _sections[0].SetShort( _pointers[Type]["PublicId"], value ); }
		}

		/// <summary>
		/// Writing these is not currently supported
		/// </summary>
		public uint SecretId
		{
			get { return _sections[0].GetShort( _pointers[Type]["SecretId"] ); }
			private set { _sections[0].SetShort( _pointers[Type]["SecretId"], value ); }
		}

		public string TimePlayed
		{
			get
			{
				uint h = _sections[0].GetShort( _pointers[Type]["TimeHours"] );
				byte m = _sections[0][_pointers[Type]["TimeMinutes"]];
				byte s = _sections[0][_pointers[Type]["TimeSeconds"]];
				byte f = _sections[0][_pointers[Type]["TimeFrames"]];

				return string.Format( "{0}h{1}m{2}s{3}f", h, m, s, f );
			}
		}

		public uint GameCode
		{
			get { return _sections[0].GetShort( _pointers[Type]["GameCode"] ); }
			private set { _sections[0].SetShort( _pointers[Type]["GameCode"], value ); }
		}

		public uint SecurityKey
		{
			get { return _sections[0].GetInt( _pointers[Type]["SecurityKey"] ); }
			private set { _sections[0].SetInt( _pointers[Type]["SecurityKey"], value ); }
		}

		public uint TeamSize
		{
			get { return _sections[1].GetInt( _pointers[Type]["TeamSize"] ); }
			set
			{
				if( value != TeamSize )
				{
					_sections[1].SetInt( _pointers[Type]["TeamSize"], value );
					InvokePropertyChanged( "TeamSize" );
				}
			}
		}

		public uint Money
		{
			get { return Xor.Run( _sections[1].GetInt( _pointers[Type]["Money"] ) ); }
			set
			{
				if( value != Money )
				{
					_sections[1].SetInt( _pointers[Type]["Money"], Xor.Run( value ) );
					InvokePropertyChanged( "Money" );
				}
			}
		}

		/// <summary>
		/// This sets publicid and secretid at the same time
		/// </summary>
		public uint TrainerId
		{
			get { return _sections[0].GetInt( _pointers[Type]["PublicId"] ); }
			private set { _sections[0].SetInt( _pointers[Type]["PublicId"], value ); }
		}

		void ExtractTeam()
		{
			Team = new BindingList<MonsterEntry>();
			Team.ListChanged += ( a, e ) => InvokePropertyChanged( "Team" );
			for( int i = 0; i < 6; i++ )
				Team.Add( new MonsterEntry( _sections[1], _pointers[Type]["TeamList"] + ( i * 100 ), false ) );
		}

		void ExtractItems()
		{
			PCItems = new BindingList<ItemEntry>();
			PCItems.ListChanged += ( a, e ) => InvokePropertyChanged( "PCItems" );
			for( int i = 0; i < _pointers[Type]["PCItemsLength"]; i++ )
				PCItems.Add( new ItemEntry( _sections[1], _pointers[Type]["PCItems"] + ( i * 4 ) ) );

			Items = new BindingList<ItemEntry>();
			Items.ListChanged += ( a, e ) => InvokePropertyChanged( "Items" );
			for( int i = 0; i < _pointers[Type]["ItemsLength"]; i++ )
				Items.Add( new ItemEntry( _sections[1], _pointers[Type]["Items"] + ( i * 4 ), Xor ) );

			KeyItems = new BindingList<ItemEntry>();
			KeyItems.ListChanged += ( a, e ) => InvokePropertyChanged( "KeyItems" );
			for( int i = 0; i < _pointers[Type]["KeyItemsLength"]; i++ )
				KeyItems.Add( new ItemEntry( _sections[1], _pointers[Type]["KeyItems"] + ( i * 4 ), Xor ) );

			BallPocket = new BindingList<ItemEntry>();
			BallPocket.ListChanged += ( a, e ) => InvokePropertyChanged( "BallPocket" );
			for( int i = 0; i < _pointers[Type]["BallPocketLength"]; i++ )
				BallPocket.Add( new ItemEntry( _sections[1], _pointers[Type]["BallPocket"] + ( i * 4 ), Xor ) );

			TMCase = new BindingList<ItemEntry>();
			TMCase.ListChanged += ( a, e ) => InvokePropertyChanged( "TMCase" );
			for( int i = 0; i < _pointers[Type]["TMCaseLength"]; i++ )
				TMCase.Add( new ItemEntry( _sections[1], _pointers[Type]["TMCase"] + ( i * 4 ), Xor ) );

			Berries = new BindingList<ItemEntry>();
			Berries.ListChanged += ( a, e ) => InvokePropertyChanged( "Berries" );
			for( int i = 0; i < _pointers[Type]["BerriesLength"]; i++ )
				Berries.Add( new ItemEntry( _sections[1], _pointers[Type]["Berries"] + ( i * 4 ), Xor ) );
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
			var pre = string.Empty;
			for( int i = 0; i < PcBuffer.Count; i++ )
			{
				if( i % 30 == 0 )
				{
					pre = "PC Box #" + Math.Floor( i / 30.0 );
				}
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

		public event PropertyChangedEventHandler PropertyChanged;

		public void InvokePropertyChanged( string e )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( e ) );
		}
	}
}