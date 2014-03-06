using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PokeSave
{
	public class Line
	{
		static readonly IDictionary<char, decimal> _weights = new Dictionary<char, decimal>()
		{
			{ 'H', 0.5m },
			{ 'D', 2m },
			{ '0', 0m },
			{ 'Q', 0.25m },
			{ '4', 4m },
			{ '@', 1m }
		};

		static readonly IDictionary<MonsterType, int> _headerIndexes = new Dictionary<MonsterType, int>
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

		public Line( Line a, Line b )
		{
			Weights = new decimal[a.Weights.Length];
			for( int i = 0; i < Weights.Length; i++ )
				Weights[i] = Combiner.Run( a.Weights[i], b.Weights[i] );
			Type = new[] { a.Type[0], b.Type[0] };
		}

		public Line( string line )
		{
			var spl = line.Split( ';' );
			Type =
				spl.Take( spl.Length == 2 ? 1 : 2 ).Select( s => (MonsterType) Enum.Parse( typeof( MonsterType ), s ) ).ToArray();
			Weights = spl.Last().Select( c => _weights[c] ).ToArray();
		}

		public MonsterType[] Type { get; private set; }

		public decimal[] Weights { get; private set; }

		public decimal AttackedBy( MonsterType type )
		{
			return Weights[_headerIndexes[type]];
		}

		public bool Is( MonsterType[] types )
		{
			if( Type.Length != types.Length )
				return false;
			if( Type.Length == 1 )
				return Type[0] == types[0];
			return ( Type[0] == types[0] && Type[1] == types[1] ) || ( Type[0] == types[1] && Type[1] == types[0] );
		}
	}


	public class Rule
	{
		readonly decimal A;
		readonly decimal B;

		public Rule( decimal a, decimal b, decimal result )
		{
			A = a;
			B = b;
			Result = result;
		}

		public decimal Result { get; private set; }

		public bool Applies( decimal a, decimal b )
		{
			return ( a == A && b == B ) || a == B && b == A;
		}
	}

	public static class Combiner
	{
		static readonly IList<Rule> _rules = new List<Rule>()
		{
			new Rule( .5m, .5m, .25m ),
			new Rule( 2, 2, 4 ),
			new Rule( 2, .5m, 1 ),
			new Rule( 1, .5m, .5m ),
			new Rule( 1, 2, 2 ),
			new Rule( 1, 1, 1 ),
			new Rule( 0, 0, 0 ),
			new Rule( 0, 1, 0 ),
			new Rule( 0, 2, 0 ),
			new Rule( 0, .5m, 0 ),
		};

		public static decimal Run( decimal a, decimal b )
		{
			foreach( var r in _rules )
				if( r.Applies( a, b ) )
					return r.Result;
			return 1m;
		}
	}


	public abstract class TypeChart
	{
		readonly Line[] data;

		protected TypeChart( string path )
		{
			var single = new List<Line>();
			using( var textstream = new StreamReader( Assembly
						.GetExecutingAssembly()
						.GetManifestResourceStream( "PokeSave.Resources." + path ) ) )
			{
				string line;
				while( ( line = textstream.ReadLine() ) != null )
					single.Add( new Line( line ) );
			}

			var doubles = new List<Line>();
			for( int i = 0; i < single.Count; i++ )
				for( int j = i + 1; j < single.Count; j++ )
				{
					doubles.Add( new Line( single[i], single[j] ) );
				}
			Types = single.SelectMany( s => s.Type ).ToArray();
			data = single.Concat( doubles ).ToArray();
		}

		public MonsterType[] Types { get; private set; }
		public MonsterType[] AttackTypes { get; private set; }

		public Line For( MonsterType type )
		{
			return For( new[] { type } );
		}

		public Line For( MonsterType[] type )
		{
			return data.FirstOrDefault( line => line.Is( type ) );
		}
	}
}
