using System.Diagnostics;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestFixture]
	public class SaveFile1
	{
		SaveFile _file;

		[SetUp]
		public void Setup()
		{
			_file = new SaveFile( "p.sav" );
			Debug.Write( _file.ToString() );
		}

		[Test]
		public void BothSaveFilesHasSameSecret()
		{
			Assert.AreEqual( _file.A.Trainer.SecretKey, _file.B.Trainer.SecretKey );

		}

		[Test]
		public void SaveFileHasTrainerSectionId()
		{
			Assert.AreEqual( 0, _file.A.Trainer.ID );
			Assert.AreEqual( 64295, _file.A.Trainer.Checksum );
			Assert.AreEqual( 42, _file.A.Trainer.SaveIndex );
		}

		[Test]
		public void SaveFileHasCorrectSecretkey()
		{
			Assert.AreEqual( 775683919, _file.A.Trainer.SecretKey );
		}

		[Test]
		public void SaveFileHasCorrectMoney()
		{
			Assert.AreEqual( 775896843, _file.A.Team.MoneyEncrypted );
			Assert.AreEqual( 311364, Cipher.Run( _file.A.Team.MoneyEncrypted ) );
			Assert.AreEqual( 311364, _file.A.Team.Money );

		}

		[Test]
		public void CalculatedChecksumIsChecksum()
		{
			Assert.AreEqual( _file.A.Trainer.Checksum, _file.A.Trainer.CalculatedChecksum );
		}

		[Test]
		public void SaveFileHasName()
		{
			Assert.AreEqual( "RED2____", _file.A.Trainer.Name );
		}
	}
}
