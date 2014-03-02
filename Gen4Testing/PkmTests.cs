using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PokeSave.Fourth;

namespace Gen4Testing
{
	[TestFixture]
	public class PkmTests
	{
		[Test]
		public void lastpart()
		{
			var data = File.ReadAllBytes( "pearlsav.bin" );
			var pkm = new PokeSave.Fourth.NdsPkm( data, 0x98, false, false );
			var a =1317105556;
			Assert.AreEqual( a, pkm.Personality );
			Assert.AreEqual( 102, pkm.AttackEV );
		}
		[Test]
		public void CanCalculateValidChecksum()
		{
			var data = File.ReadAllBytes( "399Bidoof.pkm" );
			var pkm = new PokeSave.Fourth.NdsPkm( data, 0, true, true );

			Assert.AreEqual( pkm.Checksum, pkm.GenerateChecksum().Value );

		}
	}
}
