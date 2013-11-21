using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class SubstructureTests
	{
		/*
		00. GAEM	 06. AGEM	 12. EGAM	 18. MGAE
		01. GAME	 07. AGME	 13. EGMA	 19. MGEA
		02. GEAM	 08. AEGM	 14. EAGM	 20. MAGE
		03. GEMA	 09. AEMG	 15. EAMG	 21. MAEG
		04. GMAE	 10. AMGE	 16. EMGA	 22. MEGA
		05. GMEA	 11. AMEG	 17. EMAG	 23. MEAG
		*/
		[TestCase( 1, 0 )]
		[TestCase( 1, 1 )]
		[TestCase( 2, 2 )]
		[TestCase( 3, 3 )]
		[TestCase( 2, 4 )]
		[TestCase( 3, 5 )]
		[TestCase( 0, 6 )]
		[TestCase( 0, 7 )]
		[TestCase( 0, 8 )]
		[TestCase( 0, 9 )]
		[TestCase( 0, 10 )]
		[TestCase( 0, 11 )]
		[TestCase( 2, 12 )]
		[TestCase( 3, 13 )]
		[TestCase( 1, 14 )]
		[TestCase( 1, 15 )]
		[TestCase( 3, 16 )]
		[TestCase( 2, 17 )]
		[TestCase( 2, 18 )]
		[TestCase( 3, 19 )]
		[TestCase( 1, 20 )]
		[TestCase( 1, 21 )]
		[TestCase( 3, 22 )]
		[TestCase( 2, 23 )]
		public void CanFindCorrectOffsetForAction( int expect, int index )
		{
			Assert.AreEqual( expect, SubstructureOffset.Action( (uint)index ) );
		}


		[TestCase( 2, 0 )]
		[TestCase( 3, 1 )]
		[TestCase( 1, 2 )]
		[TestCase( 1, 3 )]
		[TestCase( 3, 4 )]
		[TestCase( 2, 5 )]
		[TestCase( 2, 6 )]
		[TestCase( 3, 7 )]
		[TestCase( 1, 8 )]
		[TestCase( 1, 9 )]
		[TestCase( 3, 10 )]
		[TestCase( 2, 11 )]
		[TestCase( 0, 12 )]
		[TestCase( 0, 13 )]
		[TestCase( 0, 14 )]
		[TestCase( 0, 15 )]
		[TestCase( 0, 16 )]
		[TestCase( 0, 17 )]
		[TestCase( 3, 18 )]
		[TestCase( 2, 19 )]
		[TestCase( 3, 20 )]
		[TestCase( 2, 21 )]
		[TestCase( 1, 22 )]
		[TestCase( 1, 23 )]
		public void CanFindCorrectOffsetForEVs( int expect, int index )
		{
			Assert.AreEqual( expect, SubstructureOffset.EVs( (uint)index ) );
		}

		[TestCase( 3, 0 )]
		[TestCase( 2, 1 )]
		[TestCase( 3, 2 )]
		[TestCase( 2, 3 )]
		[TestCase( 1, 4 )]
		[TestCase( 1, 5 )]
		[TestCase( 3, 6 )]
		[TestCase( 2, 7 )]
		[TestCase( 3, 8 )]
		[TestCase( 2, 9 )]
		[TestCase( 1, 10 )]
		[TestCase( 1, 11 )]
		[TestCase( 3, 12 )]
		[TestCase( 2, 13 )]
		[TestCase( 3, 14 )]
		[TestCase( 2, 15 )]
		[TestCase( 1, 16 )]
		[TestCase( 1, 17 )]
		[TestCase( 0, 18 )]
		[TestCase( 0, 19 )]
		[TestCase( 0, 20 )]
		[TestCase( 0, 21 )]
		[TestCase( 0, 22 )]
		[TestCase( 0, 23 )]
		public void CanFindCorrectOffsetForMisc( int expect, int index )
		{
			Assert.AreEqual( expect, SubstructureOffset.Misc( (uint)index ) );
		}

		[TestCase( 0, 0 )]
		[TestCase( 0, 1 )]
		[TestCase( 0, 2 )]
		[TestCase( 0, 3 )]
		[TestCase( 0, 4 )]
		[TestCase( 0, 5 )]
		[TestCase( 1, 6 )]
		[TestCase( 1, 7 )]
		[TestCase( 2, 8 )]
		[TestCase( 3, 9 )]
		[TestCase( 2, 10 )]
		[TestCase( 3, 11 )]
		[TestCase( 1, 12 )]
		[TestCase( 1, 13 )]
		[TestCase( 2, 14 )]
		[TestCase( 3, 15 )]
		[TestCase( 2, 16 )]
		[TestCase( 3, 17 )]
		[TestCase( 1, 18 )]
		[TestCase( 1, 19 )]
		[TestCase( 2, 20 )]
		[TestCase( 3, 21 )]
		[TestCase( 2, 22 )]
		[TestCase( 3, 23 )]
		public void CanFindCorrectOffsetForGrowth( int expect, int index )
		{
			Assert.AreEqual( expect, SubstructureOffset.Growth((uint)index ) );
		}
	}
}
