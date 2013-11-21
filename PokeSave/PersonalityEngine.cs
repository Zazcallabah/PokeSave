using System;
using System.Diagnostics;

namespace PokeSave
{
	public class PersonalityEngine
	{
		readonly Random _r;

		public PersonalityEngine()
		{
			_r = new Random();
		}
		public PersonalityEngine( int seed )
		{
			_r = new Random( seed );
		}

		public uint Generate()
		{
			var bytes = new byte[4];
			_r.NextBytes( bytes );

			uint p = ( (uint) bytes[0] | (uint) ( bytes[1] << 8 ) | (uint) ( bytes[2] << 16 ) | (uint) ( bytes[3] << 24 ) );

			const int modability = 2;
			const int modnature = 25;
			var mark = DateTime.Now.Ticks;
			if( Ability != null )
				while( p % modability != AbilityToNumber( Ability.Value ) )
					p++;

			if( Nature != null )
				while( p % modnature != Nature )
					p += modability;

			if( Evolution != null || OriginalTrainer != null )
				while( 
					( Evolution != null && !HasEvolution( p, Evolution.Value ) )
					||
					( OriginalTrainer != null && !IsShiny( p ) )
					)
					p += modnature * modability;

			Debug.WriteLine( "took " + new TimeSpan( DateTime.Now.Ticks - mark ) );
			return p;
		}

		bool HasEvolution( uint p, EvolutionDirection e )
		{
			return GetEvolution( p ) == e;
		}
		EvolutionDirection GetEvolution( uint p )
		{
			return ( p & 0xffff ) % 10 < 5 ? EvolutionDirection.S : EvolutionDirection.C;
		}

		int AbilityToNumber( Ability a )
		{
			return a == PokeSave.Ability.First ? 0 : 1;
		}

		bool IsShiny( uint p )
		{
			uint r = p ^ OriginalTrainer.Value;
			return ( ( r & 0xFFFF ) ^ ( r >> 16 ) ) < 8;
		}

		public uint? OriginalTrainer { get; set; }

		public uint? Nature { get; set; }

		public Ability? Ability { get; set; }
		public EvolutionDirection? Evolution { get; set; }
	}
}