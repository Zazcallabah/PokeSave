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

		[TestCase( 0, 3, true, 8 )]
		[TestCase( 0xf, 3, false, 7 )]
		[TestCase( 0xff, 10, true, 0xff )]
		public void CanAssignBit( byte start, int bit, bool value, byte result )
		{
			Assert.AreEqual( result, start.AssignBit( bit, value ) );
		}

		[Test]
		public void CanSetMostSignificantUintBit()
		{
			Assert.AreEqual( (uint) 0x92345678, ( (uint) 0x12345678 ).SetBit( 31 ) );
		}

		[TestCase( 0xFF, 0, true )]
		[TestCase( 0xFF, 2, true )]
		[TestCase( 0xFF, 3, true )]
		[TestCase( 0xFF, 6, true )]
		[TestCase( 0x7, 0, true )]
		[TestCase( 0x7, 1, true )]
		[TestCase( 0x7, 2, true )]
		[TestCase( 0x7, 3, false )]
		[TestCase( 0x7, 4, false )]
		[TestCase( 0x7, 5, false )]
		public void CanCheckIfByteBitIsSet( byte start, int bit, bool value )
		{
			Assert.AreEqual( value, start.IsSet( bit ) );
		}
	}
}
