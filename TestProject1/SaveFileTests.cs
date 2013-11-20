using System.Diagnostics;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestFixture]
	public class SaveFileTests
	{
		SaveFile _file;

		[SetUp]
		public void Setup()
		{
			_file = new SaveFile( "p.sav" );
			Debug.WriteLine( _file );
		}

		[Test]
		public void AnotherSaveFilesHasPcItems()
		{
			_file = new SaveFile( "p3.sav" );
			Debug.WriteLine( _file );
			Assert.AreEqual( _file.A.SecretId, _file.B.SecretId );
		}


		[Test]
		public void BothSaveFilesHasSameSecret()
		{
			Assert.AreEqual( _file.A.SecretId, _file.B.SecretId );

		}
		[Test]
		public void SaveFileHasTrainerSectionId()
		{
			Assert.AreEqual( "Girl", _file.A.Gender );
			Assert.AreEqual( 56268, _file.A.SecretId );
			Assert.AreEqual( 40132, _file.A.PublicId );
		}
		[Test]
		public void MonsterChecksumCalculatesOk()
		{
			Assert.AreEqual( _file.A.Team[0].Checksum, _file.A.Team[0].CalculatedChecksum );
		}

		[Test]
		public void SaveFileHasCorrectItemCount()
		{
			Assert.AreEqual( 6, _file.A.BallPocket[0].Count );
		}

		[Test]
		public void SaveFileHasCorrectSecretkey()
		{
			Assert.AreEqual( 775683919, _file.A.SecurityKey );
		}

		[Test]
		public void SaveFileHasCorrectMoney()
		{
			Assert.AreEqual( 311364, _file.A.Money );
			Assert.AreEqual( 311208, _file.B.Money );
		}

		[Test]
		public void SaveFileHasName()
		{
			Assert.AreEqual( "RED2", _file.A.Name );
		}
	}
}
