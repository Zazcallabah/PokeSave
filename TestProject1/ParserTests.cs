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
			_sf = new SaveFile( "p.sav" ) { A = { Name = "RED2" }, B = { Name = "RED3" } };
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
			Assert.AreEqual( "ID (System.UInt32)\r\nName (System.String)\r\nCount (System.UInt32)\r\n", _c.List( _sf, "pcitems[0]" ) );
		}

		[Test]
		public void CanListPossibleForRootObject()
		{
			Assert.IsFalse( string.IsNullOrEmpty( _c.List( _sf, "" ) ) );
		}
	}
}
