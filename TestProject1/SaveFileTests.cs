﻿using System.Diagnostics;
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
		public void SaveFileCanSetName()
		{
			Assert.AreEqual( "RED2", _file.Latest.Name );
			_file.Latest.Name = "12345678";
			Assert.AreEqual( "1234567", _file.Latest.Name );
		}


		[Test]
		public void SaveFileCanSwapGender()
		{
			Assert.AreEqual( "Girl", _file.Latest.Gender );
			_file.Latest.Gender = "Boy";
			Assert.AreEqual( "Boy", _file.Latest.Gender );
		}

		[Test]
		public void SaveFileCanChangePCItems()
		{
			Assert.AreEqual( 13, _file.Latest.PCItems[0].ID );
			_file.Latest.PCItems[0].ID = 20;
			Assert.AreEqual( "Max Potion", _file.Latest.PCItems[0].Name );
		}

		[Test]
		public void SaveFileCanClearPCItems()
		{
			Assert.AreEqual( 13, _file.Latest.PCItems[0].ID );
			_file.Latest.PCItems[0].Clear();
			Assert.AreEqual( 0, _file.Latest.PCItems[0].ID );
			Assert.AreEqual( 0, _file.Latest.PCItems[0].Count );
		}

		[Test]
		public void SaveFileCanChangeNumberOfPCItems()
		{
			Assert.AreEqual( 1, _file.Latest.PCItems[0].Count );
			_file.Latest.PCItems[0].Count = 100;
			Assert.AreEqual( 100, _file.Latest.PCItems[0].Count );
		}

		[Test]
		public void SaveFileCanChangeNumberOfItems()
		{
			Assert.AreEqual( 7, _file.Latest.Items[0].Count );
			_file.Latest.Items[0].Count = 100;
			Assert.AreEqual( 100, _file.Latest.Items[0].Count );
		}

		[Test]
		public void SaveFileCanChangeNumberOfItemsToNothing()
		{
			Assert.AreEqual( 7, _file.Latest.Items[0].Count );
			_file.Latest.Items[0].Count = 0;
			Assert.AreEqual( 0, _file.Latest.Items[0].Count );
		}

		[Test]
		public void SaveFileCanSetRival()
		{
			Assert.AreEqual( "GARY", _file.Latest.Rival );
			_file.Latest.Rival = "12345678";
			Assert.AreEqual( "1234567", _file.Latest.Rival );
		}

		[Test]
		public void SaveFileCanSetFriendship()
		{
			Assert.AreEqual( 135, _file.Latest.Team[0].Friendship );
			_file.Latest.Team[0].Friendship = 60;
			Assert.AreEqual( 60, _file.Latest.Team[0].Friendship );
		}

		[Test]
		public void SaveFileCanSetXP()
		{
			Assert.AreEqual( 3060, _file.Latest.Team[0].XP );
			_file.Latest.Team[0].XP = 60;
			Assert.AreEqual( 60, _file.Latest.Team[0].XP );
		}


		[Test]
		public void SaveFileCanSetMoney()
		{
			Assert.AreEqual( 311364, _file.Latest.Money );
			_file.Latest.Money = 1000;
			Assert.AreEqual( 1000, _file.Latest.Money );
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
		public void MakeTeamLeaderOtherAndSave()
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
			Assert.IsFalse( _file.Latest.Team[0].Burned );
			Assert.IsFalse( _file.Latest.Team[0].Frozen );
			Assert.IsFalse( _file.Latest.Team[0].Paralyzed );
			Assert.IsFalse( _file.Latest.Team[0].BadPoisoned );
			Assert.AreEqual( 0, _file.Latest.Team[0].Sleeping );

			Assert.IsTrue( _file.Latest.Team[1].Burned );
			Assert.IsTrue( _file.Latest.Team[2].Frozen );
			Assert.IsTrue( _file.Latest.Team[3].Paralyzed );
			Assert.IsTrue( _file.Latest.Team[4].BadPoisoned );
			Assert.IsFalse( _file.Latest.Team[5].Burned );
			Assert.IsFalse( _file.Latest.Team[5].Poisoned );

			Assert.AreEqual( 5, _file.Latest.Team[5].Sleeping );
			Assert.AreEqual( 8, _file.Latest.Team[0].StatusByte );
			Assert.AreEqual( _file.Latest.Team[0].Checksum, _file.Latest.Team[0].CalculatedChecksum );

		}
		[Test]
		public void CanRemoveStatusAilment()
		{

			Assert.AreEqual( _file.Latest.Team[0].Checksum, _file.Latest.Team[0].CalculatedChecksum );
			_file.Latest.Team[0].Poisoned = true;
			_file.Latest.Team[1].Burned = false;
			_file.Latest.Team[2].Frozen = false;
			_file.Latest.Team[3].Paralyzed = true;
			_file.Latest.Team[4].BadPoisoned = true;
			_file.Latest.Team[5].Sleeping = 5;
			Assert.IsTrue( _file.Latest.Team[0].Poisoned );
			Assert.IsTrue( _file.Latest.Team[3].Paralyzed );
			Assert.IsTrue( _file.Latest.Team[4].BadPoisoned );
			Assert.IsFalse( _file.Latest.Team[2].Frozen );
			Assert.IsFalse( _file.Latest.Team[0].Burned );
			Assert.AreEqual( 5, _file.Latest.Team[5].Sleeping );
			Assert.AreEqual( 8, _file.Latest.Team[0].StatusByte );
			_file.Latest.Team[3].Paralyzed = false;
			Assert.IsFalse( _file.Latest.Team[3].Paralyzed );

			Assert.AreEqual( _file.Latest.Team[0].Checksum, _file.Latest.Team[0].CalculatedChecksum );

		}

		[Test]
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

					Assert.AreEqual( t, p.OriginalTrainerId );
				}
			}
		}

		[Test]
		public void AfterSaveFileHasSameSize()
		{
			var f = new SaveFile( "p.sav" );
			f.Save( "tmp.sav" );

			Assert.AreEqual( new FileInfo( "p.sav" ).Length, new FileInfo( "tmp.sav" ).Length );
		}

		[Test]
		public void CanWritePCBuffm()
		{
			Assert.AreEqual( "PIDGEY", _file.Latest.PcBuffer[0].Type.Name );
			Assert.AreEqual( "PIDGEY", _file.Latest.PcBuffer[0].Name );

			_file.Latest.PcBuffer[0].Name = "PID_GE";

			Assert.AreEqual( "PID", _file.Latest.PcBuffer[0].Name );
		}
		[Test]
		public void MakeTeamShinyAndSaveFile()
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

			_file.Save( "outtmp.sav" );
			var f2 = new SaveFile( "outtmp.sav" );
			foreach( var p in f2.Latest.Team )
			{
				if( !p.Empty )
				{
					Assert.IsTrue( p.Shiny );
				}
			}
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
