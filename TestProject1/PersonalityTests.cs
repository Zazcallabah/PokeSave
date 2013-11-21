using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class PersonalityTests
	{
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
			uint nature = 9;
			var p = new PersonalityEngine { Nature = nature };
			var g = p.Generate();
			Assert.AreEqual( 9, g % 25 );
		}

		[Test]
		public void CanSpecifyShinyNatureAbilityEvolutionAtSameTime()
		{
			uint nature = 9;
			uint tID = 87373;
			var p = new PersonalityEngine(444)
			{
				Nature = nature,
				Ability = Ability.First,
				Evolution = EvolutionDirection.C,
				OriginalTrainer = tID
			};
			var g = p.Generate();

			Assert.AreEqual( 9, g % 25 );
			uint r = g ^ tID;
			Assert.IsTrue( ( ( r & 0xFFFF ) ^ ( r >> 16 ) ) < 8 );
			Assert.AreEqual( 0, g % 2 );
			Assert.IsFalse( ( g & 0xffff ) % 10 < 5 );
		}
	}
}
