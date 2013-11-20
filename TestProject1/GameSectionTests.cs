using System.IO;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class GameSaveTests
	{
		GameSave _save;

		[SetUp]
		public void Setup()
		{
			using( var fs = File.OpenRead( "p2.sav" ) )
				_save = new GameSave( fs );
		}

		[Test]
		public void BothSaveFilesHasSameSecret()
		{
			Assert.AreEqual( _file.A.Trainer.SecretKey, _file.B.Trainer.SecretKey );
		}

		[Test]
		public void SaveFileHasSaveIndex()
		{
			Assert.AreEqual( 44, _file.A.Trainer.SaveIndex );
		}

		[Test]
		public void CalculatedChecksumIsChecksum()
		{
			Assert.AreEqual( _file.A.Trainer.Checksum, _file.A.Trainer.CalculatedChecksum );
		}

	}
}
