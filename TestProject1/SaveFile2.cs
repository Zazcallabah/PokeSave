using System.Diagnostics;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class SaveFile2
	{
		SaveFile _file;

		[SetUp]
		public void Setup()
		{
			_file = new SaveFile( "p2.sav" );
			Debug.Write( _file.ToString() );
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
