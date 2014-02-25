using System.IO;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class DexTests
	{

		[Test]
		public void CanHaveData()
		{
			var entry = MonsterList.Get( 1 );

			Assert.AreEqual( 45, entry.HP );
			Assert.AreEqual( 49, entry.Attack );
			Assert.AreEqual( 49, entry.Defense );
			Assert.AreEqual( 65, entry.SpAttack );
			Assert.AreEqual( 65, entry.SpDefense );
			Assert.AreEqual( 45, entry.Speed );

		}

		[TestCase( "TREECKO", 277 )]
		[TestCase( "CELEBI", 251 )]
		[TestCase( "GRIMER", 88 )]
		[TestCase( "CHIMECHO", 411 )]
		public void CanFetchDataFromList( string name, int id )
		{
			var data = MonsterList.Get( (uint) id );
			Assert.AreEqual( name, data.Name );
		}
	}
}
