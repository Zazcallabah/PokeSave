using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class CipherTests
	{
		[Test]
		public void TestMethod1()
		{
			Cipher.Init( 775683919 );
			Assert.AreEqual( 311364, Cipher.Run( 775896843 ) );
		}
	}
}
