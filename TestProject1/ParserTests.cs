using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class ParserTests
	{
		CommandLineParser _c;
		SaveFile _sf;

		[SetUp]
		public void Setup()
		{
			_c = new CommandLineParser();
			_sf = new SaveFile( "p.sav" );
			_sf.A.Name = "RED2";
			_sf.B.Name = "RED3";
		}

		[Test]
		public void ReturnsSaveFileForEmptyCommand()
		{
			var p = _c.GetPropertyForString( _sf, "" );
			Assert.IsNullOrEmpty( p.Error );
			Assert.AreSame( _sf, p.Parent );
		}

		[Test]
		public void ReturnsGameSaveForStringLatest()
		{
			var p = _c.GetPropertyForString( _sf, "latest" );
			Assert.IsNullOrEmpty( p.Error );
			var val = p.Property.GetValue( p.Parent, null );
			Assert.AreSame( _sf.Latest, val );
		}

		[Test]
		public void ReturnsNameForStringName()
		{
			var p = _c.GetPropertyForString( _sf, "name" );
			Assert.IsNullOrEmpty( p.Error );
			var val = p.Property.GetValue( p.Parent, null );
			Assert.AreEqual( _sf.Latest.Name, val );
		}
		[Test]
		public void TryingToIndexNonArrayIsIgnored()
		{
			var p = _c.GetPropertyForString( _sf, "name[0]" );
			Assert.IsNullOrEmpty( p.Error );
			var val = p.Property.GetValue( p.Parent, null );
			Assert.AreEqual( _sf.Latest.Name, val );
		}

		[Test]
		public void ReturnsTeamNameForStringTeamName()
		{
			var p = _c.GetPropertyForString( _sf, "team[0].name" );
			Assert.IsNullOrEmpty( p.Error );
			var val = p.Property.GetValue( p.Parent, null );
			Assert.AreEqual( _sf.Latest.Team[0].Name, val );
		}

		[Test]
		public void ReturnsTeamForStringTeam()
		{
			var p = _c.GetPropertyForString( _sf, "team" );
			Assert.IsNullOrEmpty( p.Error );
			var val = p.Property.GetValue( p.Parent, null );
			Assert.AreSame( _sf.Latest.Team, val );
		}

		[Test]
		public void CommandListHasNoEmptyValues()
		{
			var r = _c.ExtractCommands( "a." );
			Assert.AreEqual( 1, r.Length );
			Assert.AreEqual( "a", r[0] );
		}

		[Test]
		public void CommandListPrependsLatestIfNotExplicit()
		{
			var r = _c.ExtractCommands( "name" );
			Assert.AreEqual( 2, r.Length );
			Assert.AreEqual( "latest", r[0] );
			Assert.AreEqual( "name", r[1] );
		}
		[Test]
		public void CommandListDoesntAddLatestIfAlreadyThere()
		{
			var r = _c.ExtractCommands( "latest.name" );
			Assert.AreEqual( 2, r.Length );
			Assert.AreEqual( "latest", r[0] );
			Assert.AreEqual( "name", r[1] );
		}
		[Test]
		public void CanParseVerySimpleInput()
		{
			Assert.AreEqual( "RED2", _c.Read( _sf, "name" ) );
		}

		[Test]
		public void CanDecideWhichSaveFile()
		{
			Assert.AreEqual( "RED2", _c.Read( _sf, "A.name" ) );
			Assert.AreEqual( "RED3", _c.Read( _sf, "B.name" ) );
		}

		[Test]
		public void CanIndexIntoItem()
		{
			Assert.AreEqual( "Potion", _c.Read( _sf, "PCitems[0].Name" ) );
		}
		[Test]
		public void CantFetchNonexistentProperty()
		{
			Assert.AreEqual( "Not valid property", _c.Read( _sf, "PCitems[0].NOTHING" ) );
		}

		[Test]
		public void CanIndexIntoBuffer()
		{
			Assert.AreEqual( "PIDGEY", _c.Read( _sf, "pcbuffer[0].Name" ) );
		}

		[Test]
		public void CanReadUint()
		{
			Assert.AreEqual( "1", _c.Read( _sf, "pcitems[0].count" ) );
		}

		[Test]
		public void CanReadReadonlyField()
		{
			Assert.AreEqual( "False", _c.Read( _sf, "pcbuffer[0].empty" ) );
			Assert.AreEqual( "True", _c.Read( _sf, "pcbuffer[100].empty" ) );
		}

		[Test]
		public void CanSetStringValue()
		{
			_c.Write( _sf, "name RED4" );
			Assert.AreEqual( "RED4", _sf.Latest.Name );
		}

		[Test]
		public void CanSetUintValue()
		{
			_c.Write( _sf, "pcbuffer[0].type 81" );
			Assert.AreEqual( 81, _sf.Latest.PcBuffer[0].MonsterId );
		}


		[Test]
		public void CanSetBoolValue()
		{
			_c.Write( _sf, "Team[0].Poisoned true" );
			Assert.AreEqual( true, _sf.Latest.Team[0].Poisoned );
		}


		[Test]
		public void CantSetReadonlyValue()
		{
			Assert.IsTrue( _c.Write( _sf, "pcbuffer[0].unknown 0" ).Contains( "readonly" ) );
		}

		[Test]
		public void CanListPossibleValues()
		{
			Assert.AreEqual( "ID (UInt32)\r\nName (String)\r\nCount (UInt32)\r\n",
				_c.List( _sf, "pcitems[0]" ) );
		}

		[Test]
		public void CanListPossibleForRootObject()
		{
			Assert.IsFalse( string.IsNullOrEmpty( _c.List( _sf, "" ) ) );
		}
	}
}
