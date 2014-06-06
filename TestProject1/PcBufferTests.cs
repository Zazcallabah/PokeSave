using System.IO;
using System.Linq;
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

		[Test]
		public void SortObjectCanBeSortedByMonsterIdAndMark()
		{
			var s = new[]{
			 new SortedBoxObject { Mark = 0, MonsterId = 3 },
			 new SortedBoxObject { Mark = 0, MonsterId = 2 },
			 new SortedBoxObject { Mark = 0, MonsterId = 3 },
			 new SortedBoxObject { Mark = 0, MonsterId = 1 },
			 new SortedBoxObject { Mark = 1, MonsterId = 3 },
			 new SortedBoxObject { Mark = 1, MonsterId = 2 },
			 new SortedBoxObject { Mark = 2, MonsterId = 3 },
			 new SortedBoxObject { Mark = 1, MonsterId = 1 },
			 new SortedBoxObject { Mark = 0, MonsterId = 0 }
			};

			var r = s.OrderBy( o=> o ).ToArray();

			Assert.AreSame( s[6], r[0] );
			Assert.AreSame( s[7], r[1] );
			Assert.AreSame( s[5], r[2] );
			Assert.AreSame( s[4], r[3] );
			Assert.AreSame( s[8], r[4] );
			Assert.AreSame( s[3], r[5] );
			Assert.AreSame( s[1], r[6] );
			Assert.AreSame( s[0], r[7] );
			Assert.AreSame( s[2], r[8] );
		}

		[Test]
		public void SortObjectCanBeSortedByMonsterId()
		{
			var s = new[]{
			 new SortedBoxObject { Mark = 0, MonsterId = 3 },
			 new SortedBoxObject { Mark = 0, MonsterId = 2 },
			 new SortedBoxObject { Mark = 0, MonsterId = 3 },
			 new SortedBoxObject { Mark = 0, MonsterId = 1 }
			};

			var r = s.OrderBy( o=> o ).ToArray();

			Assert.AreSame( s[3], r[0] );
			Assert.AreSame( s[1], r[1] );
			Assert.AreSame( s[0], r[2] );
			Assert.AreSame( s[2], r[3] );
			Assert.AreNotSame( s[3], r[1] );
		}
	}
}
