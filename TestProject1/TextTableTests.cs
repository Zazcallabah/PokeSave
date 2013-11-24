using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class TextTableTests
	{
		GameSection _gs;
		byte[] _b;

		[SetUp]
		public void Setup()
		{
			_b = new byte[4096];
			_gs = new GameSection( _b );
		}

		[Test]
		public void WritingStringWithUnknownCharPutsTerminator()
		{
			var str = "aa&aa";
			TextTable.WriteString( _gs, str, 0, 5 );
			Assert.AreEqual( 0xFF, _b[2] );
		}

		[Test]
		public void WritingRawStringPutsAllCharsDespiteTerminator()
		{
			var str = "00_00";
			TextTable.WriteStringRaw( _gs, str, 0, 5 );
			Assert.AreEqual( 0xA1, _b[0] );
			Assert.AreEqual( 0xA1, _b[1] );
			Assert.AreEqual( 0xFF, _b[2] );
			Assert.AreEqual( 0xA1, _b[3] );
			Assert.AreEqual( 0xA1, _b[4] );
		}

		[Test]
		public void WritingStringForcesTerminator()
		{
			var str = "00000";
			TextTable.WriteString( _gs, str, 0, 5 );
			Assert.AreEqual( 0xA1, _b[0] );
			Assert.AreEqual( 0xA1, _b[1] );
			Assert.AreEqual( 0xA1, _b[2] );
			Assert.AreEqual( 0xA1, _b[3] );
			Assert.AreEqual( 0xFF, _b[4] );
		}
		[Test]
		public void WritingStringRawRespectsIndexAndLength()
		{
			var str = "000000000";
			TextTable.WriteStringRaw( _gs, str, 2, 2 );
			Assert.AreEqual( 0x0, _b[0] );
			Assert.AreEqual( 0x0, _b[1] );
			Assert.AreEqual( 0xA1, _b[2] );
			Assert.AreEqual( 0xA1, _b[3] );
			Assert.AreEqual( 0x00, _b[4] );
		}

	}
}
