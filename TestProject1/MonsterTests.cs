using System.Diagnostics;
using System.Globalization;
using System.IO;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class MonsterTests
	{
		GameSave _saveA;
		GameSection _gs;

		[SetUp]
		public void Setup()
		{
			using( var fs = File.OpenRead( "p2.sav" ) )
			{
				_saveA = new GameSave( fs );
			}
		}

		[Test]
		public void MonsterListParse()
		{
			Assert.AreEqual( "BULBASAUR", NameList.Get( 1 ) );
		}
		[Test]
		public void SaveHasTrainerData()
		{
			Assert.AreEqual( "GREEN", _saveA.Team[0].OriginalTrainerName );
		}

		[Test]
		public void CanGetStatus()
		{
			Assert.AreEqual( 0, _saveA.Team[0].StatusByte );
			Debug.WriteLine( _saveA.Team[0].Full() );
		}
		[Test]
		public void BaseStatsAreCorrect()
		{
			var t = MonsterList.Get( 1 );
			Assert.AreEqual( 45, t.HP );
			Assert.AreEqual( 49, t.Attack );
			Assert.AreEqual( 49, t.Defense );
			Assert.AreEqual( 65, t.SpAttack );
			Assert.AreEqual( 65, t.SpDefense );
			Assert.AreEqual( 45, t.Speed );

			Assert.AreEqual( 70, t.BaseFriendship );
			Assert.AreEqual( 45, t.CatchRate );
			Assert.AreEqual( 20, t.StepsToHatch );
			Assert.AreEqual( 64, t.BaseXpYield );
			Assert.AreEqual( 65, t.Ability1 );
		}

		[Test]
		public void FirstAbilityArntNull()
		{
			for( uint i = 1; i <= 251; i++ )
			{
				var type = MonsterList.Get( i );
				Assert.AreNotEqual( 0, type.Ability1, i.ToString() );
			}
			for( uint i = 277; i <= 411; i++ )
			{
				var type = MonsterList.Get( i );
				Assert.AreNotEqual( 0, type.Ability1, i.ToString() );
			}
		}

		[Test]
		public void CanPrintMonsterInfo()
		{
			var ml = MonsterList.Get( 1 );
			Assert.IsTrue( ml.ToString().Contains( "BULBASAUR" ) );
		}

		[Test]
		public void EmptyMonsterIsEmpty()
		{
			Assert.IsTrue( _saveA.PcBuffer[419].Empty );
		}
		MonsterEntry TestSection()
		{
			var data = @"9de847ffe1dd6e3bbdbbcdbdc9c9c8ff80430202c5d9e2ffffffff00a4f100007c3529c47c3529c47c3529c4593429c4013529c47c7329c47c0eace45875f8c97c3529c4163529c47c3529c4623529c4";
			var array = new byte[80];

			for( int i = 0; i < array.Length; i++ )
			{
				array[i] = byte.Parse( data.Substring( 2 * i, 2 ), NumberStyles.HexNumber );
			}
			_gs = new GameSection( array );
			return new MonsterEntry( _gs, 0, true );
		}

		[Test]
		public void WeCanFigureOutIsMonsterShiny()
		{
			var m = TestSection();
			Assert.IsFalse( m.Shiny );
		}

		[Test]
		public void WeCanMakeItShiny()
		{
			var m = TestSection();
			Debug.WriteLine( m.ToString() );
			m.SetPersonality( new PersonalityEngine() { OriginalTrainer = m.OriginalTrainerId } );
			Debug.WriteLine( m.ToString() );
			Assert.IsTrue( m.Shiny );
		}

		[Test]
		public void WeCanChangeOTIDYetPreserveShiny()
		{
			var m = TestSection();
			m.Shiny = true;
			Assert.IsTrue( m.Shiny );
			m.OriginalTrainerId = 55;
			Assert.IsTrue( m.Shiny );
		}

		[Test]
		public void AfterSettingPersonalityChecksumsStillMatch()
		{
			var m = TestSection();
			m.Shiny = true;
			Assert.AreEqual( m.Checksum, m.CalculatedChecksum );
		}

		[Test]
		public void WeCanCalculateFieldsFromPersonality()
		{
			var m = TestSection();
			Assert.AreEqual( 0x9d, m.GenderByte );
			Assert.AreEqual( AbilityIndex.Second, m.Ability );
			Assert.AreEqual( MonsterNature.Timid, m.Nature );
			Assert.AreEqual( EvolutionDirection.C, m.Evolution );

		}
		[Test]
		public void WeCanFigureOutWhichSubstructureIsWhere()
		{
			var m = TestSection();
			Assert.AreEqual( 44, m.GrowthOffset );
			Assert.AreEqual( 68, m.ActionOffset );
			Assert.AreEqual( 32, m.EVsOffset );
			Assert.AreEqual( 56, m.MiscOffset );
			Assert.AreEqual( 293, m.MonsterId );
		}
		[Test]
		public void TypeReturnsMonsterId()
		{
			var m = TestSection();
			Assert.AreEqual( m.MonsterId, m.Type );
		}
		[Test]
		public void WeCanFetchGrowthData()
		{
			var m = TestSection();
			Assert.AreEqual( 293, m.MonsterId );
			Assert.AreEqual( 0, m.Item );
			Assert.AreEqual( 125, m.XP );
		}

		[Test]
		public void WeCanSetGrowthData()
		{
			var m = TestSection();
			m.MonsterId = 3;
			m.Item = 44;
			m.XP = 444;
			Assert.AreEqual( 3, m.MonsterId );
			Assert.AreEqual( 44, m.Item );
			Assert.AreEqual( 444, m.XP );
		}

		[Test]
		public void WeCanSetLanguageData()
		{
			var m = TestSection();
			Assert.AreEqual( 0x0202, m.Language );
			m.Language = 0x205;
			Assert.AreEqual( 0x205, m.Language );
		}

		[Test]
		public void WeCanSetOTName()
		{
			var m = TestSection();
			Assert.AreEqual( "Ken", m.OriginalTrainerName );
			m.OriginalTrainerName = "RRR_hide";
			Assert.AreEqual( "RRR", m.OriginalTrainerName );
		}

		[Test]
		public void CanGetUnown()
		{
			var m = TestSection();
			Assert.AreEqual( 0x11, m.UnownShape );
		}



		[Test]
		public void TestWritingOTid()
		{
			var m = TestSection();

			Assert.AreEqual( uint.Parse( "3b6edde1", NumberStyles.HexNumber ), m.OriginalTrainerId );
			Assert.AreEqual( 44, m.GrowthOffset );
			Assert.AreEqual( 293, m.MonsterId );
			Assert.IsFalse( _gs.IsDirty );

			m.OriginalTrainerId = 0xFFFFFFFF;
			Assert.IsTrue( _gs.IsDirty );
			Assert.AreEqual( uint.Parse( "00B81647", NumberStyles.HexNumber ), _gs.GetInt( m.GrowthOffset ) );
			Assert.AreEqual( uint.Parse( "FFFFFFFF", NumberStyles.HexNumber ), m.OriginalTrainerId );
			Assert.AreEqual( 293, m.MonsterId );
		}

		[Test]
		public void TestWritingPersonality()
		{
			var m = TestSection();

			Assert.AreEqual( uint.Parse( "ff47e89d", NumberStyles.HexNumber ), m.Personality );
			Assert.AreEqual( 44, m.GrowthOffset );
			Assert.AreEqual( 293, m.MonsterId );
			Assert.IsFalse( _gs.IsDirty );

			m.Personality = 0xFFFFFFFF;
			Assert.IsTrue( _gs.IsDirty );
			Assert.AreEqual( 68, m.GrowthOffset );
			Assert.AreEqual( uint.Parse( "C491233B", NumberStyles.HexNumber ), _gs.GetInt( m.GrowthOffset ) );
			Assert.AreEqual( uint.Parse( "FFFFFFFF", NumberStyles.HexNumber ), m.Personality );
			Assert.AreEqual( 293, m.MonsterId );
		}

		[Test]
		public void EntryChecksumCalculation()
		{
			var m = TestSection();
			Assert.AreEqual( uint.Parse( "C429357C", NumberStyles.HexNumber ), m.SecurityKey );
			Assert.AreEqual( uint.Parse( "F1A4", NumberStyles.HexNumber ), m.CalculatedChecksum );
			Assert.AreEqual( uint.Parse( "F1A4", NumberStyles.HexNumber ), m.Checksum );
		}

	}
}
