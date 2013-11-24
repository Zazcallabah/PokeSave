using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class SubstructureTests
	{
		byte[] _b;
		GameSection _gs;
		MonsterEntry _m;

		[SetUp]
		public void Setup()
		{
			_b = new byte[100];
			_gs = new GameSection( _b );
			_m = new MonsterEntry( _gs, 0, false );

		}

		[Test]
		public void SettingHighWordSubsectionTriggersIsDirty()
		{

			Assert.IsFalse( _m.IsDirty );
			_m.SetEncryptedWord( 0, true, 0 );
			Assert.IsTrue( _m.IsDirty );
		}
		[Test]
		public void SettingLowWordSubsectionTriggersIsDirty()
		{

			Assert.IsFalse( _m.IsDirty );
			_m.SetEncryptedWord( 0, false, 0 );
			Assert.IsTrue( _m.IsDirty );
		}

		[Test]
		public void SettingDWordSubsectionTriggersIsDirty()
		{

			Assert.IsFalse( _m.IsDirty );
			_m.SetEncryptedDWord( 0, 0 );
			Assert.IsTrue( _m.IsDirty );
		}
		[Test]
		public void SettingByteSubsectionTriggersIsDirty()
		{

			Assert.IsFalse( _m.IsDirty );
			_m.SetEncryptedByte( 0, 2, 0 );
			Assert.IsTrue( _m.IsDirty );
		}

		[Test]
		public void CanGetByteDataFromSubstructure()
		{

			_b[3] = 0x1;
			_b[2] = 0x2;
			_b[1] = 0x3;
			_b[0] = 0x4;

			Assert.AreEqual( 0x01020304, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0x1, _m.GetEncryptedByte( 0, 3 ) );
			Assert.AreEqual( 0x2, _m.GetEncryptedByte( 0, 2 ) );
			Assert.AreEqual( 0x3, _m.GetEncryptedByte( 0, 1 ) );
			Assert.AreEqual( 0x4, _m.GetEncryptedByte( 0, 0 ) );
		}

		[Test]
		public void CanGetByteDataFromSubstructureWithEncryption()
		{
			_m.Personality = 0x09080706;
			Assert.AreEqual( 0x09080706, _m.SecurityKey );
			Assert.AreEqual( 0, _m.GetEncryptedDWord( 0 ) ); // a xor a = 0

			_b[3] = 0x19;
			_b[2] = 0x28;
			_b[1] = 0x37;
			_b[0] = 0x46;

			Assert.AreEqual( 0x10203040, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0x10, _m.GetEncryptedByte( 0, 3 ) );
			Assert.AreEqual( 0x20, _m.GetEncryptedByte( 0, 2 ) );
			Assert.AreEqual( 0x30, _m.GetEncryptedByte( 0, 1 ) );
			Assert.AreEqual( 0x40, _m.GetEncryptedByte( 0, 0 ) );
		}

		[Test]
		public void CanSetByteDataFromSubstructure()
		{
			_b[3] = 0x19;
			_b[2] = 0x28;
			_b[1] = 0x37;
			_b[0] = 0x46;

			_m.SetEncryptedByte( 0, 3, 0xa3 );
			_m.SetEncryptedByte( 0, 2, 0xb3 );
			_m.SetEncryptedByte( 0, 1, 0xc3 );
			_m.SetEncryptedByte( 0, 0, 0xd3 );

			Assert.AreEqual( 0xa3b3c3d3, _m.GetEncryptedDWord( 0 ) );

		}

		[Test]
		public void CanSetByteDataFromSubstructureWithEncryption()
		{
			_m.Personality = 0x09080706;
			Assert.AreEqual( 0x09080706, _m.SecurityKey );
			Assert.AreEqual( 0, _m.GetEncryptedDWord( 0 ) ); // a xor a = 0

			_b[3] = 0x19;
			_b[2] = 0x28;
			_b[1] = 0x37;
			_b[0] = 0x46;

			_m.SetEncryptedByte( 0, 3, 0xa3 );
			_m.SetEncryptedByte( 0, 2, 0xb3 );
			_m.SetEncryptedByte( 0, 1, 0xc3 );
			_m.SetEncryptedByte( 0, 0, 0xd3 );

			Assert.AreEqual( 0xa3b3c3d3, _m.GetEncryptedDWord( 0 ) );

		}


		[Test]
		public void CanGetDWordDataFromSubstructureWithEncryption()
		{
			_m.Personality = 0x09080706;
			Assert.AreEqual( 0x09080706, _m.SecurityKey );
			Assert.AreEqual( 0, _m.GetEncryptedDWord( 0 ) ); // a xor a = 0
			Assert.AreEqual( 0x0706, (ushort) _m.SecurityKey );

			_b[3] = 0x19;
			_b[2] = 0x28;
			_b[1] = 0x37;
			_b[0] = 0x46;

			Assert.AreEqual( 0x10203040, _m.GetEncryptedDWord( 0 ) );
		}

		[Test]
		public void CanSetDWordDataFromSubstructureWithEncryption()
		{
			_m.Personality = 0x09080706;
			Assert.AreEqual( 0x09080706, _m.SecurityKey );
			Assert.AreEqual( 0, _m.GetEncryptedDWord( 0 ) ); // a xor a = 0
			Assert.AreEqual( 0x0706, (ushort) _m.SecurityKey );

			_b[3] = 0x19;
			_b[2] = 0x28;
			_b[1] = 0x37;
			_b[0] = 0x46;

			Assert.AreEqual( 0x10203040, _m.GetEncryptedDWord( 0 ) );

			_m.SetEncryptedDWord( 0, 0x11213141 );
			Assert.AreEqual( 0x11213141, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0x18, _b[3] );
			Assert.AreEqual( 0x29, _b[2] );
			Assert.AreEqual( 0x36, _b[1] );
			Assert.AreEqual( 0x47, _b[0] );
		}

		[Test]
		public void CanGetWordDataFromSubstructureWithEncryption()
		{
			_m.Personality = 0x09080706;
			Assert.AreEqual( 0x09080706, _m.SecurityKey );
			Assert.AreEqual( 0, _m.GetEncryptedWord( 0, true ) ); // a xor a = 0
			Assert.AreEqual( 0, _m.GetEncryptedWord( 0, false ) ); // a xor a = 0

			_b[3] = 0x19;
			_b[2] = 0x28;
			_b[1] = 0x37;
			_b[0] = 0x46;

			Assert.AreEqual( 0x1020, _m.GetEncryptedWord( 0, false ) );
			Assert.AreEqual( 0x3040, _m.GetEncryptedWord( 0, true ) );
		}


		[Test]
		public void CanSetHighWordDataFromSubstructureWithEncryption()
		{
			_m.Personality = 0x09080706;
			Assert.AreEqual( 0x09080706, _m.SecurityKey );
			Assert.AreEqual( 0, _m.GetEncryptedDWord( 0 ) ); // a xor a = 0

			_b[3] = 0x19;
			_b[2] = 0x28;
			_b[1] = 0x37;
			_b[0] = 0x46;

			Assert.AreEqual( 0x10203040, _m.GetEncryptedDWord( 0 ) );

			_m.SetEncryptedWord( 0, true, 0x1121 );
			Assert.AreEqual( 0x10201121, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0x1121, _m.GetEncryptedWord( 0, true ) );
			Assert.AreEqual( 0x19, _b[3] );
			Assert.AreEqual( 0x28, _b[2] );
			Assert.AreEqual( 0x16, _b[1] );
			Assert.AreEqual( 0x27, _b[0] );
		}

		[Test]
		public void CanSetLowWordDataFromSubstructureWithEncryption()
		{
			_m.Personality = 0x09080706;
			Assert.AreEqual( 0x09080706, _m.SecurityKey );
			Assert.AreEqual( 0, _m.GetEncryptedDWord( 0 ) ); // a xor a = 0

			_b[3] = 0x19;
			_b[2] = 0x28;
			_b[1] = 0x37;
			_b[0] = 0x46;

			Assert.AreEqual( 0x10203040, _m.GetEncryptedDWord( 0 ) );

			_m.SetEncryptedWord( 0, false, 0x3141 );
			Assert.AreEqual( 0x31413040, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0x3141, _m.GetEncryptedWord( 0, false ) );
			Assert.AreEqual( 0x38, _b[3] );
			Assert.AreEqual( 0x49, _b[2] );
			Assert.AreEqual( 0x37, _b[1] );
			Assert.AreEqual( 0x46, _b[0] );
		}

		[Test]
		public void CanSetBothWordDataFromSubstructureWithEncryption()
		{
			_m.Personality = 0x09080706;
			Assert.AreEqual( 0x09080706, _m.SecurityKey );
			Assert.AreEqual( 0, _m.GetEncryptedDWord( 0 ) ); // a xor a = 0

			_b[3] = 0x19;
			_b[2] = 0x28;
			_b[1] = 0x37;
			_b[0] = 0x46;

			Assert.AreEqual( 0x10203040, _m.GetEncryptedDWord( 0 ) );

			_m.SetEncryptedWord( 0, false, 0x1121 );
			_m.SetEncryptedWord( 0, true, 0x3141 );
			Assert.AreEqual( 0x11213141, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0x3141, _m.GetEncryptedWord( 0, true ) );
			Assert.AreEqual( 0x36, _b[1] );
			Assert.AreEqual( 0x47, _b[0] );
			Assert.AreEqual( 0x1121, _m.GetEncryptedWord( 0, false ) );
			Assert.AreEqual( 0x18, _b[3] );
			Assert.AreEqual( 0x29, _b[2] );
		}
		[Test]
		public void CanGetDWordDataFromSubstructure()
		{
			_b[3] = 9;
			_b[2] = 8;
			_b[1] = 7;
			_b[0] = 6;

			Assert.AreEqual( 0, _m.SecurityKey );
			Assert.AreEqual( 0x09080706, _m.GetEncryptedDWord( 0 ) );
		}

		[Test]
		public void CanGetHighWordDataFromSubstructure()
		{
			_b[3] = 9;
			_b[2] = 8;
			_b[1] = 7;
			_b[0] = 6;

			Assert.AreEqual( 0, _m.SecurityKey );
			Assert.AreEqual( 0x0706, _m.GetEncryptedWord( 0, true ) );
		}
		[Test]
		public void CanGetLowWordDataFromSubstructure()
		{
			_b[3] = 9;
			_b[2] = 8;
			_b[1] = 7;
			_b[0] = 6;

			Assert.AreEqual( 0, _m.SecurityKey );
			Assert.AreEqual( 0x0908, _m.GetEncryptedWord( 0, false ) );
		}

		[Test]
		public void CanSetDWordDataIntoSubstructure()
		{
			_m.SetEncryptedDWord( 0, 0xABCD4567 );

			Assert.AreEqual( 0, _m.SecurityKey );
			Assert.AreEqual( 0xABCD4567, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0xAB, _b[3] );
			Assert.AreEqual( 0xCD, _b[2] );
			Assert.AreEqual( 0x45, _b[1] );
			Assert.AreEqual( 0x67, _b[0] );
		}

		[Test]
		public void CanSetHighWordDataIntoSubstructure()
		{
			_m.SetEncryptedWord( 0, true, 0xABCD );

			Assert.AreEqual( 0, _m.SecurityKey );
			Assert.AreEqual( 0x0000ABCD, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0xABCD, _m.GetEncryptedWord( 0, true ) );
			Assert.AreEqual( 0xAB, _b[1] );
			Assert.AreEqual( 0xCD, _b[0] );
			Assert.AreEqual( 0, _b[2] );
			Assert.AreEqual( 0, _b[3] );
		}

		[Test]
		public void CanSetLowWordDataIntoSubstructure()
		{
			_m.SetEncryptedWord( 0, false, 0xABCD );

			Assert.AreEqual( 0, _m.SecurityKey );
			Assert.AreEqual( 0xABCD0000, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0xABCD, _m.GetEncryptedWord( 0, false ) );
			Assert.AreEqual( 0xAB, _b[3] );
			Assert.AreEqual( 0xCD, _b[2] );
			Assert.AreEqual( 0x00, _b[1] );
			Assert.AreEqual( 0x00, _b[0] );
		}

		[Test]
		public void CanSetDWordDataIntoSubstructureWithExistingData()
		{
			_m.SetEncryptedDWord( 0, 0x32333435 );
			_m.SetEncryptedDWord( 0, 0xABCD4567 );

			Assert.AreEqual( 0, _m.SecurityKey );
			Assert.AreEqual( 0xABCD4567, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0xAB, _b[3] );
			Assert.AreEqual( 0xCD, _b[2] );
			Assert.AreEqual( 0x45, _b[1] );
			Assert.AreEqual( 0x67, _b[0] );
		}

		[Test]
		public void CanSetHighWordDataIntoSubstructureWithExistingData()
		{
			_m.SetEncryptedDWord( 0, 0x32333435 );
			_m.SetEncryptedWord( 0, true, 0xABCD );

			Assert.AreEqual( 0, _m.SecurityKey );
			Assert.AreEqual( 0x3233ABCD, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0xABCD, _m.GetEncryptedWord( 0, true ) );
			Assert.AreEqual( 0x32, _b[3] );
			Assert.AreEqual( 0x33, _b[2] );
			Assert.AreEqual( 0xAB, _b[1] );
			Assert.AreEqual( 0xCD, _b[0] );
		}

		[Test]
		public void CanSetLowWordDataIntoSubstructureWithExistingData()
		{
			_m.SetEncryptedDWord( 0, 0x32333435 );
			_m.SetEncryptedWord( 0, false, 0xABCD );

			Assert.AreEqual( 0, _m.SecurityKey );
			Assert.AreEqual( 0xABCD3435, _m.GetEncryptedDWord( 0 ) );
			Assert.AreEqual( 0xABCD, _m.GetEncryptedWord( 0, false ) );
			Assert.AreEqual( 0x35, _b[0] );
			Assert.AreEqual( 0x34, _b[1] );
			Assert.AreEqual( 0xCD, _b[2] );
			Assert.AreEqual( 0xAB, _b[3] );
		}

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
			Assert.AreEqual( expect, SubstructureOffset.Action( (uint) index ) );
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
			Assert.AreEqual( expect, SubstructureOffset.EVs( (uint) index ) );
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
			Assert.AreEqual( expect, SubstructureOffset.Misc( (uint) index ) );
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
			Assert.AreEqual( expect, SubstructureOffset.Growth( (uint) index ) );
		}
	}
}
