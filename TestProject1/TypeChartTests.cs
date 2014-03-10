using NUnit.Framework;
using PokeSave;
using PokeSave.Sixth;

namespace TestProject1
{
	[TestFixture]
	public class TypeChartTests
	{
		[Test]
		public void CanCombineTypesToCorrectLine()
		{
			var chart = new Gen6TypeChart();
			var lg = chart.For( MonsterType.Ground );
			var lf = chart.For( MonsterType.Flying );
			var combo = chart.For( new[] { MonsterType.Ground, MonsterType.Flying } );

			Assert.AreEqual( 0m, lg.AttackedBy( MonsterType.Electric ) );
			Assert.AreEqual( 0m, combo.AttackedBy( MonsterType.Electric ) );
			Assert.AreEqual( 2m, lf.AttackedBy( MonsterType.Electric ) );


			Assert.AreEqual( 2m, lg.AttackedBy( MonsterType.Ice ) );
			Assert.AreEqual( 4m, combo.AttackedBy( MonsterType.Ice ) );
			Assert.AreEqual( 2m, lf.AttackedBy( MonsterType.Ice ) );

			Assert.AreEqual( 2m, lg.AttackedBy( MonsterType.Grass ) );
			Assert.AreEqual( 1m, combo.AttackedBy( MonsterType.Grass ) );
			Assert.AreEqual( .5m, lf.AttackedBy( MonsterType.Grass ) );
		}

	}

}