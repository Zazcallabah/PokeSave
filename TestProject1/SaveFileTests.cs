using System.Diagnostics;
using System.IO;
using System.Linq;
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
		public void TeamCorrectMoveName()
		{
			_file = new SaveFile( "p4.sav" );
			Assert.AreEqual( "Thundershock", _file.Latest.Team[0].Move1Name );
		}

		[Test]
		public void TeamHasStatusAndPP()
		{
			_file = new SaveFile( "p4.sav" );
			Debug.WriteLine( _file.Latest.Team[0].Full() );
			Assert.IsTrue( _file.Latest.Team[0].Poisoned );
			Assert.AreEqual( 28, _file.Latest.Team[0].PP1 );
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
			var filenames = new[] { "p.sav", "p2.sav", "p3.sav" };
			var files = filenames.Select( n => new SaveFile( n ) );

			foreach( var s in files.SelectMany( file => new[] { file.A, file.B } ).SelectMany( save => new MonsterEntry[0].Concat( save.PcBuffer ).Concat( save.Team ) ) )
			{
				Assert.AreEqual( s.Checksum, s.CalculatedChecksum );
			}
		}

		[Test]
		[Ignore]
		public void MakeTeamLeaderOther()
		{

			Assert.AreEqual( _file.Latest.Team[0].Checksum, _file.Latest.Team[0].CalculatedChecksum );
			_file.Latest.Team[0].MonsterId = 277;
			Assert.AreEqual( 277, _file.Latest.Team[0].MonsterId );
			Assert.AreNotEqual( _file.Latest.Team[0].Checksum, _file.Latest.Team[0].CalculatedChecksum );
			_file.Save( "upd.sav" );
			Assert.AreEqual( _file.Latest.Team[0].Checksum, _file.Latest.Team[0].CalculatedChecksum );

		}

		[Test]
		public void MakeTeamStatusDifferent()
		{

			Assert.AreEqual( _file.Latest.Team[0].Checksum, _file.Latest.Team[0].CalculatedChecksum );
			_file.Latest.Team[0].Poisoned = true;
			_file.Latest.Team[1].Burned = true;
			_file.Latest.Team[2].Frozen = true;
			_file.Latest.Team[3].Paralyzed = true;
			_file.Latest.Team[4].BadPoisoned = true;
			_file.Latest.Team[5].Sleeping = 5;
			Assert.IsTrue( _file.Latest.Team[0].Poisoned );
			Assert.AreEqual( 5, _file.Latest.Team[5].Sleeping );
			Assert.AreEqual( 8, _file.Latest.Team[0].StatusByte );
			Assert.AreEqual( _file.Latest.Team[0].Checksum, _file.Latest.Team[0].CalculatedChecksum );

		}

		[Test]
		[Ignore]
		public void MakeTeamOwn()
		{
			var t = _file.Latest.TrainerId;
			foreach( var p in _file.Latest.Team )
			{
				if( !p.Empty )
				{
					if( t != p.OriginalTrainerId )
					{
						p.OriginalTrainerId = t;
					}
				}
			}

			_file.Save( "fr.sav" );
		}

		[Test]
		public void AfterSaveFileHasSameSize()
		{
			var f = new SaveFile( "p.sav" );
			f.Save( "tmp.sav" );

			Assert.AreEqual( new FileInfo( "p.sav" ).Length, new FileInfo( "tmp.sav" ).Length );
		}

		[Test]
		[Ignore]
		public void MakeTeamShiny()
		{
			foreach( var p in _file.Latest.Team )
			{
				if( !p.Empty )
				{
					Assert.AreEqual( p.Checksum, p.CalculatedChecksum );
					var engine = new PersonalityEngine() { OriginalTrainer = p.OriginalTrainerId };
					p.SetPersonality( engine );
					Assert.AreEqual( p.Checksum, p.CalculatedChecksum );
				}
			}

			_file.Save( "out.sav" );
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
