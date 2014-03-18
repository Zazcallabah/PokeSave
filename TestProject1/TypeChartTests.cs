using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NUnit.Framework;
using PokeSave;
using PokeSave.Sixth;

namespace TestProject1
{
	[TestFixture]
	public class TypeChartTests
	{
		WebClient _wc;
		[SetUp]
		public void Setup()
		{
			_wc = new WebClient();
		}

		string IL( int l )
		{
			var sb = new StringBuilder();
			var s = "  ";
			while( l-- > 0 )
			{
				sb.Append( s );
			}
			return sb.ToString();
		}

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

		[Test, Ignore]
		public void MakeWithTypeChart()
		{
			var coll = new List<Line>();
			var tc = new Gen6TypeChart();
			var types = tc.Types;
			for( var i = 0; i < types.Length; i++ )
			{
				coll.Add( tc.For( new[] { types[i] } ) );
				/*		for( var j = i + 1; j < types.Length; j++ )
						{
							coll.Add( tc.For( new[] { types[i], types[j] } ) );
						}*/
			}
			var _headerIndexes = new Dictionary<MonsterType, int>
		{
			{ MonsterType.Normal, 0 },
			{ MonsterType.Fire, 1 },
			{ MonsterType.Water, 2 },
			{ MonsterType.Electric, 3 },
			{ MonsterType.Grass, 4 },
			{ MonsterType.Ice, 5 },
			{ MonsterType.Fighting, 6 },
			{ MonsterType.Poison, 7 },
			{ MonsterType.Ground, 8 },
			{ MonsterType.Flying, 9 },
			{ MonsterType.Psychic, 10 },
			{ MonsterType.Bug, 11 },
			{ MonsterType.Rock, 12 },
			{ MonsterType.Ghost, 13 },
			{ MonsterType.Dragon, 14 },
			{ MonsterType.Dark, 15 },
			{ MonsterType.Steel, 16 },
			{ MonsterType.Fairy, 17 }
		};

			for( int i = 0; i < _headerIndexes.Count; i++ )
			{
				var mt = (MonsterType) i;
				if( _headerIndexes.ContainsKey( mt ) )
					Debug.WriteLine( mt + ": " + _headerIndexes[mt] + "," );
				else
					Debug.WriteLine( "-1" );
			}

			var _serializer = JsonSerializer.Create( new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			} );
			if( File.Exists( @"C:\src\git\PokeSave\pokedb_mobile\x_chart.json" ) )
				File.Delete( @"C:\src\git\PokeSave\pokedb_mobile\x_chart.json" );
			using( var jsonWriter = new JsonTextWriter( new StreamWriter( File.OpenWrite( @"C:\src\git\PokeSave\pokedb_mobile\x_chart.json" ), Encoding.UTF8 ) ) )
			{
				_serializer.Serialize( jsonWriter, coll.Select( MakeMin ).ToArray() );
			}

		}

		LineMin MakeMin( Line l )
		{
			return new LineMin
					{
						t = l.Type,
						w = l.Weights.Select( Map ).ToArray()
					};

		}

		int Map( decimal arg )
		{
			if( arg == 0.5m )
				return 5;
			if( arg == 0.25m )
				return 8;
			return (int) arg;
		}

		Gen6TypeInformation g6 = new Gen6TypeInformation();
		Gen5TypeInformation g5 = new Gen5TypeInformation();
		Gen1TypeInformation g1 = new Gen1TypeInformation();
		[Test, Ignore]
		public void Run()
		{
			var m1 = DateTime.Now.Ticks;
			var l = new List<MonsterMin>();
			for( int i = 1; i <= 719; i++ )
			{
				var v = testfile( getDocString( getData( i ) ) );
				l.Add( v );
			}
			var m2 = DateTime.Now.Ticks;

			var _serializer = JsonSerializer.Create( new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			} );
			if( File.Exists( @"c:\src\x_dex.json" ) )
				File.Delete( @"c:\src\x_dex.json" );
			using( var jsonWriter = new JsonTextWriter( new StreamWriter( File.OpenWrite( @"c:\src\x_dex.json" ), Encoding.UTF8 ) ) )
			{
				_serializer.Serialize( jsonWriter, l.ToArray() );
			}
			if( File.Exists( @"c:\src\x_bank.json" ) )
				File.Delete( @"c:\src\x_bank.json" );
			using( var jsonWriter = new JsonTextWriter( new StreamWriter( File.OpenWrite( @"c:\src\x_bank.json" ), Encoding.UTF8 ) ) )
			{
				_serializer.Serialize( jsonWriter, MoveBank.All() );
			}
			var m3 = DateTime.Now.Ticks;


			Debug.WriteLine( "reading: " + new TimeSpan( reading ).ToString() );
			Debug.WriteLine( "parsing: " + new TimeSpan( parsing ).ToString() );
			Debug.WriteLine( "calculating: " + new TimeSpan( calculating ).ToString() );

			Debug.WriteLine( "sum: " + new TimeSpan( m2 - m1 ).ToString() );
			Debug.WriteLine( "serializing: " + new TimeSpan( m3 - m2 ).ToString() );
		}

		long reading;
		long parsing;
		long calculating;

		MonsterMin testfile( HtmlDocument h )
		{
			var mark = DateTime.Now.Ticks;
			var tmp = h.DocumentNode.SelectSingleNode( "html/head/title" ).InnerText;
			var istr = tmp.Substring( tmp.Length - 32, 3 );
			var nstr = tmp.Substring( 0, tmp.Length - 36 ).Replace( "&#9792;", "♀" ).Replace( "&#9794;", "♂" );
			var m = new MonsterMin()
			{
				n = nstr,
			};
			List<MoveEntry> entries = new List<MoveEntry>();
			List<MoveEntryMin> levels = new List<MoveEntryMin>();
			var tables = h.DocumentNode.SelectNodes( "//table[@class=\"dextable\"]" );
			Assert.AreEqual( "Type", tables[0].SelectNodes( "(tr/td)" )[5].InnerText );
			m.t = GetTypes( tables, nstr );
			foreach( var t in tables )
			{
				var node = t.SelectSingleNode( "(tr/td)" );
				if( node == null )
					continue;
				if( node.InnerText == "X / Y Level Up" )
				{
					var e = LevelEntries( t, "L" );
					levels.AddRange( e.Select( ee =>
					{
						var mm = MoveBank.Get( ee.MoveIndex );
						return new MoveEntryMin
						{
							l = ee.Level,
							t = mm.Type
						};

					} ).ToArray() );
					entries.AddRange( e );
				}
				else if( node.InnerText == "TM & HM Attacks" )
					entries.AddRange( LevelEntries( t, "" ) );
				//		else if( node.InnerText.StartsWith( "Egg Moves" ) )
				//		entries.AddRange( LevelEntries( t, "E", 0, -1 ) );
				//	else if( node.InnerText == "Move Tutor Attacks" )
				//		entries.AddRange( LevelEntries( t, "MT", 0, -1 ) );
				//	else if( node.InnerText == "Pre-Evolution Only Moves" )
				//		entries.AddRange( LevelEntries( t, "P", 0, -1 ) );
				//else if( node.InnerText.StartsWith( "Transfer Only Moves" ) )
				//		entries.AddRange( LevelEntries( t, "T", 0, 7 ) );

			}
			m.s = levels.Select( l => new[] { l.l, (int) l.t } ).ToArray();
			//	m.m = entries.Select( e => MoveBank.Get( e.MoveIndex ).Type ).Distinct().ToArray();
			calculating += DateTime.Now.Ticks - mark;
			return m;
		}

		MonsterType[] GetTypes( HtmlNodeCollection tables, string name )
		{
			if( name == "Wormadam" )
				return new[] { MonsterType.Bug, MonsterType.Grass };
			if( name == "Rotom" )
				return new[] { MonsterType.Electric, MonsterType.Ghost };
			if( name == "Shaymin" )
				return new[] { MonsterType.Grass };
			if( name == "Darmanitan" )
				return new[] { MonsterType.Fire };
			if( name == "Meloetta" )
				return new[] { MonsterType.Normal, MonsterType.Psychic };
			return tables[0].SelectNodes( "(tr/td)" )[11].SelectNodes( "a/img" ).Select( mapType ).ToArray();
		}

		IEnumerable<MoveEntry> LevelEntries( HtmlNode htmlNode, string labelprefix = "", int colindex = 1, int labelindex = 0 )
		{
			var rows = htmlNode.SelectNodes( "tr" );
			for( int i = 2; i < rows.Count; i += 2 )
			{
				var tds = rows[i].SelectNodes( "td" );
				MoveBank.Add( new Move
					{
						Name = tds[colindex].InnerText,
						Type = mapType( tds[colindex + 1].SelectSingleNode( "img" ) ),
						Category = mapCategory( tds[colindex + 2].SelectSingleNode( "img" ) ),
						Attack = tds[colindex + 3].InnerText,
						Accuracy = tds[colindex + 4].InnerText,
						PP = tds[colindex + 5].InnerText,
						Effect = tds[colindex + 6].InnerText,
						Info = rows[i + 1].SelectSingleNode( "td" ).InnerText
					} );
				yield return new MoveEntry( labelprefix, labelindex != -1 ? tds[labelindex].InnerText : "" )
				{
					MoveIndex = MoveBank.Get( tds[colindex].InnerText ).Index
				};

			}
		}

		IDictionary<string, MonsterType> typemap = new Dictionary<string, MonsterType>{
			{"/bug.gif",MonsterType.Bug},
			{"/dark.gif",MonsterType.Dark},
			{"/dragon.gif",MonsterType.Dragon},
			{"/electric.gif",MonsterType.Electric},
			{"/fairy.gif",MonsterType.Fairy},
			{"/fighting.gif",MonsterType.Fighting},
			{"/fire.gif",MonsterType.Fire},
			{"/flying.gif",MonsterType.Flying},
			{"/ghost.gif",MonsterType.Ghost},
			{"/grass.gif",MonsterType.Grass},
			{"/ground.gif",MonsterType.Ground},
			{"/ice.gif",MonsterType.Ice},
			{"/normal.gif",MonsterType.Normal},
			{"/poison.gif",MonsterType.Poison},
			{"/psychic.gif",MonsterType.Psychic},
			{"/rock.gif",MonsterType.Rock},
			{"/steel.gif",MonsterType.Steel},
			{"/unknown.gif",MonsterType.Unknown},
			{"/water.gif",MonsterType.Water},
		};

		MonsterType mapType( HtmlNode img )
		{
			if( img == null )
				return MonsterType.Unknown;
			var attr = img.GetAttributeValue( "src", "" );
			var typestr = attr.Substring( attr.LastIndexOf( "/" ) );
			if( typemap.ContainsKey( typestr ) )
				return typemap[typestr];
			return MonsterType.Unknown;
		}

		string mapCategory( HtmlNode img )
		{
			var src = img.GetAttributeValue( "src", "" );
			var i1 = src.LastIndexOf( "/" );
			var i2 = src.LastIndexOf( "." );
			return src.Substring( i1 + 1, i2 - i1 );
		}

		HtmlDocument getDocStream( int index )
		{
			long mark = DateTime.Now.Ticks;
			var h = new HtmlDocument();
			using( var fileStream = File.OpenRead( getpath( index ) ) )
				h.Load( fileStream, Encoding.GetEncoding( "ISO-8859-1" ) );
			parsing += DateTime.Now.Ticks - mark;
			return h;
		}
		HtmlDocument getDocString( string data )
		{
			long mark = DateTime.Now.Ticks;
			var h = new HtmlDocument();
			h.LoadHtml( data );
			parsing += DateTime.Now.Ticks - mark;
			return h;
		}

		string getData( int index )
		{
			long mark = DateTime.Now.Ticks;
			var text = File.ReadAllText( getpath( index ), Encoding.GetEncoding( "ISO-8859-1" ) );
			reading += DateTime.Now.Ticks - mark;
			return text;
		}

		string getpath( int index )
		{
			return @"C:\src\pd\" + getNum( index ) + ".html";
		}

		string getNum( int i )
		{
			return i.ToString( "D3" );
		}
	}

	internal class LineMin
	{
		public MonsterType[] t { get; set; }
		public int[] w { get; set; }
	}

	public class MonsterMin
	{
		public string n { get; set; } // Name
		public MonsterType[] t { get; set; } // types
		//	public MonsterType[] m { get; set; } // movetypes
		//	public MoveEntryMin[] s { get; set; } // moveset
		public int[][] s { get; set; }
	}


	public class Monster
	{
		public string Name { get; set; } // Name
		public int Index { get; set; } // index?
		public MonsterType[] Type { get; set; } // types
		public MonsterType[] MoveType { get; set; } // movetypes
		public MoveEntryMin[] MoveSet { get; set; } // moveset
	}

	public class MoveEntryMin
	{
		public int l { get; set; }
		public MonsterType t { get; set; }
	}

	public class MoveEntry
	{
		public MoveEntry( string labelprefix, string innerText )
		{
			int l;
			if( Int32.TryParse( innerText.Replace( "&#8212;", "0" ), out l ) )
				Level = l;
			else
				Level = -1;
			Label = labelprefix + ( innerText.Replace( "&#8212;", "-" ) );
		}

		public int Level { get; private set; }
		public string Label { get; private set; }
		public int MoveIndex { get; set; }
	}

	public class Move
	{
		public int Index { get; set; }
		public string Name { get; set; }
		public MonsterType Type { get; set; }
		public string Category { get; set; }
		public string Attack { get; set; }
		public string Accuracy { get; set; }
		public string PP { get; set; }
		public string Effect { get; set; }
		public string Info { get; set; }
	}

	public static class MoveBank
	{
		readonly static IDictionary<string, Move> Bank;
		static MoveBank()
		{
			Bank = new Dictionary<string, Move>();
		}

		public static Move[] All()
		{
			return Bank.Values.ToArray();
		}

		public static void Add( Move m )
		{
			if( !Exists( m ) )
			{
				m.Index = Bank.Values.Count;
				Bank.Add( m.Name, m );
			}
		}

		static bool Exists( Move m )
		{
			if( !Bank.ContainsKey( m.Name ) )
				return false;
			var m2 = Bank[m.Name];
			Assert.AreEqual( m2.Name, m.Name );
			Assert.AreEqual( m2.Accuracy, m.Accuracy );
			Assert.AreEqual( m2.Attack, m.Attack );
			Assert.AreEqual( m2.Category, m.Category );
			Assert.AreEqual( m2.Effect, m.Effect );
			Assert.AreEqual( m2.Info, m.Info );
			Assert.AreEqual( m2.PP, m.PP );
			Assert.AreEqual( m2.Type, m.Type );
			return true;
		}

		public static Move Get( string name )
		{
			return Bank[name];
		}

		public static Move Get( int index )
		{
			return Bank.Values.FirstOrDefault( m => m.Index == index );
		}
	}
}
