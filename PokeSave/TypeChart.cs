using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokeSave
{
	public static class TypeChart
	{
		static readonly Line[] _lines;

		static TypeChart()
		{
			var data = @"Normal 1 1 1 1 1 ½ 1 0 ½ 1 1 1 1 1 1 1 1 1
Fighting 2 1 ½ ½ 1 2 ½ 0 2 1 1 1 1 ½ 2 1 2 ½
Flying 1 2 1 1 1 ½ 2 1 ½ 1 1 2 ½ 1 1 1 1 1
Poison 1 1 1 ½ ½ ½ 1 ½ 0 1 1 2 1 1 1 1 1 2
Ground 1 1 0 2 1 2 ½ 1 2 2 1 ½ 2 1 1 1 1 1
Rock 1 ½ 2 1 ½ 1 2 1 ½ 2 1 1 1 1 2 1 1 1
Bug 1 ½ ½ ½ 1 1 1 ½ ½ ½ 1 2 1 2 1 1 2 ½
Ghost 0 1 1 1 1 1 1 2 1 1 1 1 1 2 1 1 ½ 1
Steel 1 1 1 1 1 2 1 1 ½ ½ ½ 1 ½ 1 2 1 1 2
Fire 1 1 1 1 1 ½ 2 1 2 ½ ½ 2 1 1 2 ½ 1 1
Water 1 1 1 1 2 2 1 1 1 2 ½ ½ 1 1 1 ½ 1 1
Grass 1 1 ½ ½ 2 2 ½ 1 ½ ½ 2 ½ 1 1 1 ½ 1 1
Electric 1 1 2 1 0 1 1 1 1 1 2 ½ ½ 1 1 ½ 1 1
Psychic 1 2 1 2 1 1 1 1 ½ 1 1 1 1 ½ 1 1 0 1
Ice 1 1 2 1 2 1 1 1 ½ ½ ½ 2 1 1 ½ 2 1 1
Dragon 1 1 1 1 1 1 1 1 ½ 1 1 1 1 1 1 2 1 0
Dark 1 ½ 1 1 1 1 1 2 1 1 1 1 1 2 1 1 ½ ½
Fairy 1 2 1 ½ 1 1 1 1 ½ ½ 1 1 1 1 1 2 2 1".Split( new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries );
			int i = 0;
			_lines = data.Select( d => new Line( d, i++ ) ).ToArray();
		}

		public static MonsterType[] AttackStrongAgainst( MonsterType attackingType )
		{
			return AttackFilter( attackingType, "2" );
		}

		public static MonsterType[] AttackWeakAgainstTypes( MonsterType attackingType )
		{
			return AttackFilter( attackingType, "½" );
		}

		public static MonsterType[] AttackNoneAgainstTypes( MonsterType attackingType )
		{
			return AttackFilter( attackingType, "0" );
		}

		public static MonsterType[] DefendInvincibleAgainst( MonsterType type )
		{
			return DefendFilter( type, "0" );
		}

		public static MonsterType[] DefendWeakAgainst( MonsterType type )
		{
			return DefendFilter( type, "2" );
		}

		public static MonsterType[] DefendStrongAgainst( MonsterType type )
		{
			return DefendFilter( type, "½" );
		}

		public static MonsterType[] DefendFilter( MonsterType type, string lookup )
		{
			var indexline = _lines.First( r => r.Type == type );
			var defendstats = _lines.Select( l => l.Weights[indexline.Index] ).ToArray();
			var ret = new List<MonsterType>();
			for( int i = 0; i < defendstats.Length; i++ )
			{
				if( defendstats[i] == lookup )
					ret.Add( _lines.First( r => r.Index == i ).Type );
			}
			return ret.ToArray();
		}

		public static MonsterType[] AttackFilter( MonsterType type, string lookup )
		{
			var attackStats = _lines.First( r => r.Type == type );
			var ret = new List<MonsterType>();
			for( int i = 0; i < attackStats.Weights.Length; i++ )
			{
				if( attackStats.Weights[i] == lookup )
					ret.Add( _lines.First( r => r.Index == i ).Type );
			}
			return ret.ToArray();
		}
	}

	public class Line
	{
		public int Index { get; set; }
		public string Name { get; private set; }
		public MonsterType Type { get; private set; }
		public string[] Weights { get; private set; }
		public Line( string s, int index )
		{
			Index = index;
			var spl = s.Split( ' ' );
			Name = spl[0];
			Type = (MonsterType) Enum.Parse( typeof( MonsterType ), spl[0] );
			Weights = spl.Skip( 1 ).ToArray();
		}
	}
}
