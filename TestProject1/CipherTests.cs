using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class CipherTests
	{
		[Test]
		public void TestXorCipher()
		{
			var c = new Cipher( 775683919 );
			Assert.AreEqual( 311364, c.Run( 775896843 ) );
		}
	}
}
