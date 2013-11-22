using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class PersonalityTests
	{
		[TestCase( 1 )]
		[TestCase( 2 )]
		[TestCase( 3 )]
		[TestCase( 4 )]
		[TestCase( 5 )]
		[TestCase( 11 )]
		[TestCase( 12 )]
		[TestCase( 13 )]
		public void GivenTypeCanDecideGenderFemale( int type )
		{
			var gd = new GenderDecision( MonsterGender.F, (uint) type );

			var p = new PersonalityEngine { Gender = gd };
			var g = p.Generate();

			var t = MonsterList.Get( (uint) type );

			Assert.IsTrue( ( g & 0xff ) < t.Gender );
		}
		[TestCase( 1 )]
		[TestCase( 51 )]
		[TestCase( 71 )]
		[TestCase( 88 )]
		[TestCase( 102 )]
		[TestCase( 109 )]
		[TestCase( 11 )]
		public void GivenTypeCanDecideGenderMale( int type )
		{
			var gd = new GenderDecision( MonsterGender.M, (uint) type );

			var p = new PersonalityEngine { Gender = gd };
			var g = p.Generate();

			var t = MonsterList.Get( (uint) type );

			Assert.IsFalse( ( g & 0xff ) < t.Gender );
		}
		[Test]
		public void GivenTypeGenderlessDecisionIsIgnored()
		{
			var gd = new GenderDecision( MonsterGender.M, 81 );

			var p = new PersonalityEngine { Gender = gd };
			var g = p.Generate();

		}
		[Test]
		public void GivenTypeMOnlyDecisionIsIgnored()
		{
			var gd = new GenderDecision( MonsterGender.F, 32 );

			var p = new PersonalityEngine { Gender = gd };
			var g = p.Generate();

		}
		[Test]
		public void GivenTrainerIdEngineCanMakeShiny()
		{
			uint tID = 6;
			var p = new PersonalityEngine { OriginalTrainer = tID };
			var g = p.Generate();

			uint r = g ^ tID;
			Assert.IsTrue( ( ( r & 0xFFFF ) ^ ( r >> 16 ) ) < 8 );
		}
		[Test]
		public void GivenTrainerIdEngineCanMakeNotShiny()
		{
			uint tID = 6;
			var p = new PersonalityEngine();
			var g = p.Generate();
			uint r = g ^ tID;
			Assert.IsFalse( ( ( r & 0xFFFF ) ^ ( r >> 16 ) ) < 8 );
		}

		[Test]
		public void GivenNatureEngineCanMakeNature()
		{
			var p = new PersonalityEngine { Nature = MonsterNature.Lax };
			var g = p.Generate();
			Assert.AreEqual( 9, g % 25 );
		}

		[Test]
		public void CanSpecifyShinyNatureAbilityEvolutionAtSameTime()
		{
			uint tID = 87373;
			var t = MonsterList.Get( 1 );
			var p = new PersonalityEngine( 444 )
			{
				Nature = MonsterNature.Lax,
				Ability = AbilityIndex.First,
				Evolution = EvolutionDirection.C,
				OriginalTrainer = tID,
				Gender = new GenderDecision( MonsterGender.F, 1 )
			};
			var g = p.Generate();

			Assert.AreEqual( 9, g % 25 );
			uint r = g ^ tID;
			Assert.IsTrue( ( ( r & 0xFFFF ) ^ ( r >> 16 ) ) < 8 );
			Assert.AreEqual( 0, g % 2 );
			Assert.IsFalse( ( g & 0xffff ) % 10 < 5 );
			Assert.IsTrue( ( g & 0xff ) < t.Gender );
		}
	}
}
