using System.IO;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class PcBufferTests
	{
		PcBuffer _buffer;

		[SetUp]
		public void Setup()
		{
			using( var fs = File.OpenRead( "p2.sav" ) )
			{
				var s1 = new GameSection( fs );
				var s2 = new GameSection( fs );
				var s3 = new GameSection( fs );
				_buffer = new PcBuffer( new[] { s1, s2, s3 } );
			}
		}

		[Test]
		public void BufferCanIndexIntoSplitBetweenSections()
		{
			Assert.AreEqual( "GREEN", _buffer.GetText( 5968, 8 ) );
		}
	}
}
