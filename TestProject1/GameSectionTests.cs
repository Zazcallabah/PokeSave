using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class GameSectionTests
	{
		GameSection[] _savesA;
		GameSection[] _savesB;

		[SetUp]
		public void Setup()
		{
			using( var fs = File.OpenRead( "p2.sav" ) )
			{
				_savesA = new GameSection[14];
				for( int i = 0; i < _savesA.Length; i++ )
					_savesA[i] = new GameSection( fs );

				_savesB = new GameSection[14];
				for( int i = 0; i < _savesB.Length; i++ )
					_savesB[i] = new GameSection( fs );
			}
		}

		[Test]
		public void PrintContainsChecksum()
		{
			Assert.IsTrue( _savesA[0].ToString().Contains( _savesA[0].Checksum.ToString() ) );
		}

		[Test]
		public void AllSectionsHaveSameSaveIndex()
		{
			foreach( var s in _savesA )
				Assert.AreEqual( _savesA[0].SaveIndex, s.SaveIndex );
			foreach( var s in _savesB )
				Assert.AreEqual( _savesB[0].SaveIndex, s.SaveIndex );
		}

		[Test]
		public void AllSectionsHaveDifferentIndexes()
		{
			var dict = new Dictionary<uint, string>();
			foreach( var s in _savesA )
				dict.Add( s.ID, s.Name );

			var dict2 = new Dictionary<uint, string>();
			foreach( var s in _savesB )
				dict2.Add( s.ID, s.Name );
		}

		[Test]
		public void AllSectionsHaveCorrectChecksum()
		{
			int misses = 0;
			foreach( var s in _savesA )
				if( s.Checksum != s.CalculatedChecksum )
					misses++;
			foreach( var s in _savesB )
				if( s.Checksum != s.CalculatedChecksum )
					misses++;
			Assert.AreEqual( 0, misses );
		}

		[Test]
		public void CanGetRawTextFromData()
		{
			foreach( var s in _savesA )
			{
				if( s.ID == 0 )
				{
					Assert.AreEqual( "GREEN___", s.GetTextRaw( 0, 8 ) );
				}
			}
		}

		[Test]
		public void CanGetTextFromData()
		{
			foreach( var s in _savesA )
			{
				if( s.ID == 0 )
				{
					Assert.AreEqual( "GREEN", s.GetText( 0, 8 ) );
				}
			}
		}

		[Test]
		public void CanGetShortFromData()
		{
			foreach( var s in _savesA )
			{
				if( s.ID == 0 )
				{
					Assert.AreEqual( 18, s.GetShort( 0xE ) );
				}
			}
		}

		[Test]
		public void CanGetByteFromData()
		{
			foreach( var s in _savesB )
			{
				if( s.ID == 1 )
				{
					Assert.AreEqual( 6, s[0x34] );
				}
			}
		}

		[Test]
		public void CanGetIntFromData()
		{
			foreach( var s in _savesA )
			{
				if( s.ID == 1 )
				{
					Assert.AreEqual( 6, s.GetInt( 0x34 ) );
				}
			}
		}
	}
}
