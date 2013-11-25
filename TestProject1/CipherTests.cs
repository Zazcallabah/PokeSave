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

		[Test]
		public void TestMaskExtension()
		{
			var x = 0xABCD1234;
			Assert.AreEqual( 0xABC51234, x.Mask( 0x000F0000, 5 << 16 ) );
		}

		[Test]
		public void TestShiftRight()
		{
			Assert.AreEqual( 4, ( 0xA205 & 0x7800 ) >> 11 );
		}

		[Test]
		public void TestShiftLeft()
		{
			var x = 0xA205u;
			Assert.AreEqual( 0x8A05, x.Mask( 0x7800, 1 << 11 ) );
		}
	}
}
