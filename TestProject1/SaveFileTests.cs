using System.Collections.Generic;
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
		public void CanReadAndWriteMonsterRawDataAndStillHaveCorrectMoves()
		{
			Assert.AreEqual( "Thundershock", _file.Latest.Team[0].Move1Name );
			Assert.AreEqual( "Tackle", _file.Latest.PcBuffer[0].Move1Name );

			_file.Latest.Team[0].RawData = _file.Latest.PcBuffer[0].RawData;

			Assert.AreEqual( "PIDGEY", _file.Latest.Team[0].Name );
			Assert.AreEqual( "Tackle", _file.Latest.Team[0].Move1Name );
		}

		[Test]
		public void TeamCorrectMoveName()
		{
			_file = new SaveFile( "p4.sav" );
			Assert.AreEqual( "Thundershock", _file.Latest.Team[0].Move1Name );
		}

		[Test]
		public void CanSetGender()
		{
			Assert.AreEqual( MonsterGender.F, _file.Latest.Team[0].Gender );
			_file.Latest.Team[0].Gender = MonsterGender.M;
			Assert.AreEqual( MonsterGender.M, _file.Latest.Team[0].Gender );
		}

		[Test]
		public void CanGetOrigin()
		{
			Assert.AreNotEqual( _file.Latest.Team[0].OriginInfo, _file.Latest.Team[1].OriginInfo );
		}

		[Test]
		public void CanSetPPBonus()
		{
			_file.Latest.Team[0].PPBonus1 = 3;
			_file.Latest.Team[0].PPBonus2 = 2;
			_file.Latest.Team[0].PPBonus3 = 1;
			_file.Latest.Team[0].PPBonus4 = 0;
			Assert.IsTrue( _file.Latest.Team[0].IsDirty );
			_file.Save( "tmp.sav" );
			Assert.IsFalse( _file.Latest.Team[0].IsDirty );
			Assert.AreEqual( 0, _file.Latest.Team[0].PPBonus4 );
			Assert.AreEqual( 1, _file.Latest.Team[0].PPBonus3 );
			Assert.AreEqual( 2, _file.Latest.Team[0].PPBonus2 );
			Assert.AreEqual( 3, _file.Latest.Team[0].PPBonus1 );
		}

		[Test]
		public void CanSetBall()
		{
			Debug.WriteLine( _file.Latest.ToString() );
			Assert.AreEqual( 4, _file.Latest.Team[0].BallCaught );
			Assert.IsFalse( _file.Latest.Team[0].IsDirty );
			_file.Latest.Team[0].BallCaught = 1;
			Assert.IsTrue( _file.Latest.Team[0].IsDirty );
			Assert.AreEqual( 1, _file.Latest.Team[0].BallCaught );
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
		public void AddMarksToTeamWorks()
		{
			foreach( var t in _file.Latest.Team )
			{
				Assert.AreEqual( 0, t.Mark );
				Assert.IsFalse( t.IsDirty );
			}

			for( int i = 0; i < _file.Latest.Team.Count; i++ )
				_file.Latest.Team[i].Mark = (uint) i;
			foreach( var t in _file.Latest.Team )
			{
				Assert.IsFalse( t.IsDirty );
			}
			_file.Save( "tmp.sav" );
			for( int i = 0; i < _file.Latest.Team.Count; i++ )
			{
				Assert.AreEqual( i, _file.Latest.Team[i].Mark );
				Assert.IsFalse( _file.Latest.Team[i].IsDirty );
			}
		}

		[Test]
		public void ChangingItemCountTriggersChange()
		{
			var properties = new List<string>();
			_file.PropertyChanged += ( a, e ) => properties.Add( e.PropertyName );
			_file.Latest.Items[0].Count = 44;

			Assert.AreNotEqual( 0, properties.Count );
		}

		[Test]
		public void TestsForNonSubsectionVirusFields()
		{
			// not part of subsection
			// cleared in buffer
			Assert.AreEqual( 255, _file.Latest.Team[0].Virus );
			Assert.AreEqual( 0, _file.Latest.PcBuffer[0].Virus );

			_file.Latest.Team[0].Virus = 9;
			_file.Latest.PcBuffer[0].Virus = 9;
			Assert.AreEqual( 9, _file.Latest.Team[0].Virus );
			Assert.AreEqual( 0, _file.Latest.PcBuffer[0].Virus );
		}

		[Test]
		public void SettingImmuneAffectsStrainAndViceVerse()
		{
			Assert.AreEqual( false, _file.Latest.Team[0].Immune );
			_file.Latest.Team[0].Immune = true;
			Assert.AreEqual( true, _file.Latest.Team[0].Immune );
			Assert.AreEqual( 7, _file.Latest.Team[0].VirusStrain );
		}

		[Test]
		public void TestsForSubsectionVirusFields()
		{
			// this depends on strain value
			Assert.AreEqual( false, _file.Latest.Team[0].Immune );

			Assert.AreEqual( 0, _file.Latest.Team[0].VirusStrain );
			Assert.AreEqual( 0, _file.Latest.Team[0].VirusFade );

			//this is the combination of the above two values
			Assert.AreEqual( 0x00000000, _file.Latest.Team[0].VirusStatus );

			_file.Latest.Team[0].VirusFade = 1;
			_file.Latest.Team[0].VirusStrain = 9;
			Assert.AreEqual( 9, _file.Latest.Team[0].VirusStrain );
			Assert.AreEqual( 1, _file.Latest.Team[0].VirusFade );
			Assert.AreEqual( 0x91, _file.Latest.Team[0].VirusStatus );
		}


		[Test]
		public void TestAssignVirusStrain()
		{
			_file.Latest.Team[0].AssignVirusStrain( 10 );
			Assert.AreEqual( 10, _file.Latest.Team[0].VirusStrain );
			Assert.AreEqual( 3, _file.Latest.Team[0].VirusFade );
		}

		[Test]
		public void CanSetLevelForTeamMember()
		{
			Assert.AreEqual( 14, _file.Latest.Team[0].Level );
			_file.Latest.Team[0].Level = 9;
			Assert.AreEqual( 9, _file.Latest.Team[0].Level );
		}

		[TestCase( "Virus", 255, 6 )]
		[TestCase( "CurrentHP", 35, 66 )]
		[TestCase( "TotalHP", 35, 99 )]
		[TestCase( "CurrentAttack", 20, 97 )]
		[TestCase( "CurrentDefense", 18, 13 )]
		[TestCase( "CurrentSpeed", 33, 43 )]
		[TestCase( "CurrentSpAttack", 23, 26 )]
		[TestCase( "CurrentSpDefense", 18, 40 )]
		[TestCase( "AttackEV", 3, 60 )]
		[TestCase( "HPEV", 5, 6 )]
		[TestCase( "DefenseEV", 13, 3 )]
		[TestCase( "SpeedEV", 30, 7 )]
		[TestCase( "SpAttackEV", 1, 105 )]
		[TestCase( "SpDefenseEV", 0, 64 )]
		[TestCase( "Coolness", 0, 16 )]
		[TestCase( "Beauty", 0, 24 )]
		[TestCase( "Cuteness", 0, 12 )]
		[TestCase( "Smartness", 0, 0 )]
		[TestCase( "Toughness", 0, 55 )]
		[TestCase( "Feel", 0, 99 )]
		public void CanSetMostFields( string name, int e, int n )
		{
			var existingvalue = (uint) e;
			var newvalue = (uint) n;
			var info = _file.Latest.Team[0].GetType();
			var prop = info.GetProperty( name );

			Assert.AreEqual( existingvalue, prop.GetValue( _file.Latest.Team[0], null ) );
			prop.SetValue( _file.Latest.Team[0], newvalue, null );
			Assert.AreEqual( newvalue, prop.GetValue( _file.Latest.Team[0], null ) );
		}

		[Test]
		public void CanSetEnums()
		{
			Assert.AreEqual( AbilityIndex.Second, _file.Latest.Team[0].Ability );
			Assert.AreEqual( MonsterNature.Quirky, _file.Latest.Team[0].Nature );
			Assert.AreEqual( EvolutionDirection.C, _file.Latest.Team[0].Evolution );
			Assert.AreEqual( "Static", _file.Latest.Team[0].AbilityName );
			_file.Latest.Team[0].Ability = AbilityIndex.First;
			_file.Latest.Team[0].Nature = MonsterNature.Lax;
			_file.Latest.Team[0].Evolution = EvolutionDirection.S;
			Assert.AreEqual( AbilityIndex.First, _file.Latest.Team[0].Ability );
			Assert.AreEqual( MonsterNature.Lax, _file.Latest.Team[0].Nature );
			Assert.AreEqual( EvolutionDirection.S, _file.Latest.Team[0].Evolution );
			// Only has one ability
			Assert.AreEqual( "Static", _file.Latest.Team[0].AbilityName );
		}

		[Test]
		public void CanSetCorrectAbility()
		{
			Assert.AreEqual( AbilityIndex.Second, _file.Latest.Team[3].Ability );
			Assert.AreEqual( "Sturdy", _file.Latest.Team[3].AbilityName );
			_file.Latest.Team[3].Ability = AbilityIndex.First;
			Assert.AreEqual( AbilityIndex.First, _file.Latest.Team[3].Ability );
			Assert.AreEqual( "Sturdy", _file.Latest.Team[3].AbilityName );
			_file.Latest.Team[3].ActualAbility = AbilityIndex.First;
			Assert.AreEqual( AbilityIndex.First, _file.Latest.Team[3].ActualAbility );
			Assert.AreEqual( "Rock Head", _file.Latest.Team[3].AbilityName );
		}


		[Test]
		public void CanSetMoves()
		{
			Assert.AreEqual( "Thundershock", _file.Latest.Team[0].Move1Name );
			Assert.AreEqual( "Growl", _file.Latest.Team[1].Move2Name );
			Assert.AreEqual( "Encore", _file.Latest.Team[2].Move3Name );
			Assert.AreEqual( "Rock Throw", _file.Latest.Team[3].Move4Name );
			_file.Latest.Team[0].Move1 = 88;
			_file.Latest.Team[1].Move2 = 1;
			_file.Latest.Team[2].Move3 = 100;
			_file.Latest.Team[3].Move4 = 0;
			Assert.IsTrue( _file.Latest.Team[0].IsDirty );
			Assert.IsTrue( _file.Latest.Team[1].IsDirty );
			Assert.IsTrue( _file.Latest.Team[2].IsDirty );
			Assert.IsTrue( _file.Latest.Team[3].IsDirty );
			Assert.IsFalse( _file.Latest.Team[4].IsDirty );
			Assert.AreEqual( "Rock Throw", _file.Latest.Team[0].Move1Name );
			Assert.AreEqual( "Pound", _file.Latest.Team[1].Move2Name );
			Assert.AreEqual( "Teleport", _file.Latest.Team[2].Move3Name );
			Assert.AreEqual( "Empty Move", _file.Latest.Team[3].Move4Name );
		}


		[Test]
		public void CanSetCurrentPP()
		{
			Assert.AreEqual( 30, _file.Latest.Team[0].PP1 );
			Assert.AreEqual( 40, _file.Latest.Team[1].PP2 );
			Assert.AreEqual( 5, _file.Latest.Team[2].PP3 );
			Assert.AreEqual( 15, _file.Latest.Team[3].PP4 );
			_file.Latest.Team[0].PP1 = 88;
			_file.Latest.Team[1].PP2 = 1;
			_file.Latest.Team[2].PP3 = 100;
			_file.Latest.Team[3].PP4 = 0;
			Assert.IsTrue( _file.Latest.Team[0].IsDirty );
			Assert.IsTrue( _file.Latest.Team[1].IsDirty );
			Assert.IsTrue( _file.Latest.Team[2].IsDirty );
			Assert.IsTrue( _file.Latest.Team[3].IsDirty );
			Assert.IsFalse( _file.Latest.Team[4].IsDirty );
			Assert.AreEqual( 88, _file.Latest.Team[0].PP1 );
			Assert.AreEqual( 1, _file.Latest.Team[1].PP2 );
			Assert.AreEqual( 100, _file.Latest.Team[2].PP3 );
			Assert.AreEqual( 0, _file.Latest.Team[3].PP4 );
		}
		[Test]
		public void LevelIs0ForStoredEntries()
		{
			Assert.AreEqual( 0, _file.Latest.PcBuffer[0].Level );
			_file.Latest.PcBuffer[0].Level = 9;
			Assert.AreEqual( 0, _file.Latest.PcBuffer[0].Level );
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
		public void EntryReturnsGenderCorrectlyForGenderlessType()
		{
			_file.A.Team[0].Personality = 0;
			_file.A.Team[0].Type = 82;
			_file.A.Team[1].Personality = 0xFF;
			_file.A.Team[1].Type = 82;

			Assert.AreEqual( MonsterGender.None, _file.A.Team[0].Gender );
			Assert.AreEqual( MonsterGender.None, _file.A.Team[1].Gender );
		}

		[Test]
		public void EntryReturnsGenderCorrectlyForMaleOnlyType()
		{
			_file.A.Team[0].Personality = 0;
			_file.A.Team[0].Type = 32;
			_file.A.Team[1].Personality = 0xFF;
			_file.A.Team[1].Type = 32;

			Assert.AreEqual( MonsterGender.M, _file.A.Team[0].Gender );
			Assert.AreEqual( MonsterGender.M, _file.A.Team[1].Gender );
		}
		[Test]
		public void EntryReturnsGenderCorrectlyForFemaleOnlyType()
		{
			_file.A.Team[0].Personality = 0;
			_file.A.Team[0].Type = 30;
			_file.A.Team[1].Personality = 0xFF;
			_file.A.Team[1].Type = 30;
			_file.A.Team[2].Personality = 0x55;
			_file.A.Team[2].Type = 30;

			Assert.AreEqual( MonsterGender.F, _file.A.Team[0].Gender );
			Assert.AreEqual( MonsterGender.F, _file.A.Team[2].Gender );
			Assert.AreEqual( MonsterGender.F, _file.A.Team[1].Gender );
		}

		[Test]
		public void EntryReturnsGenderCorrectlyForType()
		{
			_file.A.Team[0].Personality = 0;
			_file.A.Team[0].Type = 1;
			_file.A.Team[1].Personality = 0xFF;
			_file.A.Team[1].Type = 2;
			_file.A.Team[2].Personality = 30;
			_file.A.Team[2].Type = 2;
			_file.A.Team[3].Personality = 31;
			_file.A.Team[3].Type = 2;

			Assert.AreEqual( MonsterGender.F, _file.A.Team[0].Gender );
			Assert.AreEqual( MonsterGender.M, _file.A.Team[1].Gender );
			Assert.AreEqual( MonsterGender.F, _file.A.Team[2].Gender );
			Assert.AreEqual( MonsterGender.M, _file.A.Team[3].Gender );
		}


		[Test]
		public void CanWritePCBuffm()
		{
			Assert.AreEqual( "PIDGEY", _file.Latest.PcBuffer[0].TypeInformation.Name );
			Assert.AreEqual( "PIDGEY", _file.Latest.PcBuffer[0].Name );

			_file.Latest.PcBuffer[0].Name = "PID_GE";

			Assert.AreEqual( "PID", _file.Latest.PcBuffer[0].Name );
		}
		[Test]
		public void TeamHasCorrectMoves()
		{
			Assert.AreEqual( "Thundershock", _file.Latest.Team[0].Move1Name );
			Assert.AreEqual( 30, _file.Latest.Team[0].PP1 );
			Assert.AreEqual( 20, _file.Latest.Team[0].PP4 );
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
					p.Item = 1;
					Assert.AreNotEqual( p.Checksum, p.CalculatedChecksum );
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
