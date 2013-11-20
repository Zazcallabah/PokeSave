using System.IO;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class MonsterTests
	{
		GameSave _saveA;

		[SetUp]
		public void Setup()
		{
			using( var fs = File.OpenRead( "p2.sav" ) )
			{
				_saveA = new GameSave( fs );
			}
		}

		[Test]
		public void SaveHasTrainerData()
		{
			Assert.AreEqual( "GREEN", _saveA.Team[0].OriginalTrainerName );
		}

	}
}
