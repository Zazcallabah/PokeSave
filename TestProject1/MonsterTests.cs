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
			Assert.AreEqual( "BULBASAUR", MonsterList.Get( 1 ) );
		}
		[Test]
		public void SaveHasTrainerData()
		{
			Assert.AreEqual( "GREEN", _saveA.Team[0].OriginalTrainerName );
		}

		[Test]
		public void CanGetStatus()
		{
			Assert.AreEqual( 0, _saveA.Team[0].Status );
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
		public void WeCanCalculateFieldsFromPersonality()
		{
			var m = TestSection();
			Assert.AreEqual( 0x9d, m.GenderByte );
			Assert.AreEqual( Ability.Second, m.Ability );
			Assert.AreEqual( 10, m.Nature );
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
		public void WeCanFetchGrowthData()
		{
			var m = TestSection();
			Assert.AreEqual( 293, m.MonsterId );
			Assert.AreEqual( 0, m.Item );
			Assert.AreEqual( 125, m.XP );
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
