using System.IO;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class GameSaveTests
	{
		GameSave _saveA;
		GameSave _saveB;

		[SetUp]
		public void Setup()
		{
			using( var fs = File.OpenRead( "p2.sav" ) )
			{
				_saveA = new GameSave( fs );
				_saveB = new GameSave( fs );
			}
		}
		//[Test]
		//public void GetSeenWorks()
		//{
		//    Assert.AreEqual( true, _saveA.GetSeen(0) );
		//    Assert.AreEqual( false, _saveA.GetSeen(1) );
		//    Assert.AreEqual( false, _saveA.GetSeen(2) );
		//    Assert.AreEqual( true, _saveA.GetSeen(3) );
		//    Assert.AreEqual( false, _saveA.GetSeen(4) );
		//    Assert.AreEqual( false, _saveA.GetSeen(5) );
		//    Assert.AreEqual( true, _saveA.GetSeen(6) );
		//    Assert.AreEqual( true, _saveA.GetSeen(7) );
		//    Assert.AreEqual( false, _saveA.GetSeen(8) );

		//    Assert.AreEqual( true, _saveA.GetSeen(9) );
		//    Assert.AreEqual( true, _saveA.GetSeen(10) );
		//    Assert.AreEqual( true, _saveA.GetSeen(11) );
		//    Assert.AreEqual( true, _saveA.GetSeen(12) );
		//    Assert.AreEqual( true, _saveA.GetSeen(13) );
		//    Assert.AreEqual( true, _saveA.GetSeen(14) );
		//    Assert.AreEqual( true, _saveA.GetSeen(15) );

		//    Assert.AreEqual( false, _saveA.GetSeen(16) );
		//    Assert.AreEqual( false, _saveA.GetSeen(17) );

		//    Assert.AreEqual( true, _saveA.GetSeen(18) );
		//    Assert.AreEqual( false, _saveA.GetSeen(19) );

		//    Assert.AreEqual( true, _saveA.GetSeen(20) );
		//    Assert.AreEqual( false, _saveA.GetSeen(21) );

		//    Assert.AreEqual( true, _saveA.GetSeen(22) );
		//    Assert.AreEqual( false, _saveA.GetSeen(23) );
		//    Assert.AreEqual( true, _saveA.GetSeen(24) );
		//}


		[Test]
		public void SaveHasTrainerData()
		{
			Assert.AreEqual( "GREEN", _saveA.Name );
			Assert.AreEqual( "Boy", _saveA.Gender );
			Assert.AreEqual( 3663, _saveA.PublicId );
			Assert.AreEqual( 45187, _saveA.SecretId );
			Assert.AreEqual( 189721347, _saveA.SecurityKey );
			Assert.AreEqual( "18h0m16s43f", _saveA.TimePlayed );

			Assert.AreEqual( "GREEN", _saveB.Name );
			Assert.AreEqual( "Boy", _saveB.Gender );
			Assert.AreEqual( 3663, _saveB.PublicId );
			Assert.AreEqual( 45187, _saveB.SecretId );
			Assert.AreEqual( 428509985, _saveB.SecurityKey );
			Assert.AreEqual( "17h14m36s9f", _saveB.TimePlayed );
		}

		[Test]
		public void SaveHasTeamData()
		{
			Assert.AreEqual( 6, _saveA.TeamSize );
			Assert.AreEqual( 146652, _saveA.Money );

			//TODO PC ITEMS BAG ITEMS TEAM

			Assert.AreEqual( 6, _saveB.TeamSize );
			Assert.AreEqual( 146652, _saveB.Money );
		}

		[Test]
		public void SaveHasRivalData()
		{
			Assert.AreEqual( "ASH", _saveA.Rival );
			Assert.AreEqual( "ASH", _saveB.Rival );
		}


		[Test]
		public void CanGetGameTypeFromData()
		{
			Assert.AreEqual( GameType.FRLG, _saveA.Type );
			Assert.AreEqual( GameType.FRLG, _saveB.Type );
		}

	}
}
