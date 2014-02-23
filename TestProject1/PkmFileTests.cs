using System.Diagnostics;
using System.Globalization;
using System.IO;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class PkmFileTests
	{
		[Test]
		public void CanDetectIfPkmFile()
		{
			var h = File.ReadAllBytes( "edited.pkm" );

			Assert.IsTrue( FileTypeDetector.Is3gpkm( h ) );
		}


		[Test]
		public void CanOpenPkmFile()
		{
			var h = File.ReadAllBytes( "testing.pkm" );
			var me = new MonsterEntry( h, true );
			Assert.AreEqual( "MANKEY", me.TypeName );
			Assert.AreEqual( "Scratch", me.Move1Name );
			Assert.AreEqual( 0, me.HPEV );
			Assert.AreEqual( 0, me.Virus );
		}

		[Test]
		public void CanSaveAndCreateExactlySamePkmFile()
		{
			var h = File.ReadAllBytes( "testing.pkm" );
			var origin = new byte[h.Length];
			h.CopyTo( origin, 0 );
			var me = new MonsterEntry( h, true );
			var r = me.To3gPkm();
			Assert.AreEqual( origin.Length, r.Length );
			for( int i = 0; i < r.Length; i++ )
				Assert.AreEqual( origin[i], r[i] );
		}

		[Test]
		public void CanCorrectChecksumWhenImportingPkm()
		{
			var h = File.ReadAllBytes( "testing.pkm" );
			h[28] = 0;
			var me = new MonsterEntry( h, true );
			me.HPEV = 5;
			var ch = me.CalculatedChecksum;
			var r = me.To3gPkm();
			Assert.AreEqual( ch & 0xff, r[28] );
			Assert.AreEqual( ( ch & 0xff00 ) >> 8, r[29] );
		}
	}
}
