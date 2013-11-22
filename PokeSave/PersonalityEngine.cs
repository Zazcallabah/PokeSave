using System;
using System.Diagnostics;

namespace PokeSave
{
	public class PersonalityEngine
	{
		readonly Random _r;

		public PersonalityEngine()
			: this( new Random() )
		{
		}

		public PersonalityEngine( int seed )
			: this( new Random( seed ) )
		{
		}

		private PersonalityEngine( Random r )
		{
			_r = r;
		}

		public PersonalityEngine( MonsterEntry existing )
			: this()
		{
			OriginalTrainer = existing.Shiny ? (uint?) existing.OriginalTrainerId : null;
			Nature = existing.Nature;
			Ability = existing.Ability;
			Evolution = existing.Evolution;
			Gender = new GenderDecision( existing.Gender, existing.Type );
		}

		public uint? OriginalTrainer { get; set; }
		public MonsterNature? Nature { get; set; }
		public AbilityIndex? Ability { get; set; }
		public EvolutionDirection? Evolution { get; set; }
		public GenderDecision? Gender { get; set; }

		public uint Generate()
		{
			var bytes = new byte[4];
			_r.NextBytes( bytes );

			uint p = ( bytes[0] | (uint) ( bytes[1] << 8 ) | (uint) ( bytes[2] << 16 ) | (uint) ( bytes[3] << 24 ) );

			const int modability = 2;
			const int modnature = 25;
			long mark = DateTime.Now.Ticks;
			if( Ability != null )
				while( p % modability != AbilityToNumber( Ability.Value ) )
					p++;

			if( Nature != null )
			{
				var n = (int) Nature;
				while( p % modnature != n )
					p += modability;
			}

			if( Evolution != null || OriginalTrainer != null || Gender != null )
				while(
					( Evolution != null && !HasEvolution( p, Evolution.Value ) )
						|| ( OriginalTrainer != null && !IsShiny( p ) )
						|| ( Gender != null && !IsCorrectGender( p ) )
					)
					p += modnature * modability;

			Debug.WriteLine( "took " + new TimeSpan( DateTime.Now.Ticks - mark ) );
			return p;
		}

		bool IsCorrectGender( uint p )
		{
			if( Gender.Value.TypeGenderByte == 0xff || Gender.Value.TypeGenderByte == 0xfe || Gender.Value.TypeGenderByte == 0 )
				return true;
			if( Gender.Value.ExpectedGender == MonsterGender.M )
			{
				return ( p & 0xff ) >= Gender.Value.TypeGenderByte;
			}
			else
				return ( p & 0xff ) < Gender.Value.TypeGenderByte;
		}

		bool HasEvolution( uint p, EvolutionDirection e )
		{
			return GetEvolution( p ) == e;
		}

		EvolutionDirection GetEvolution( uint p )
		{
			return ( p & 0xffff ) % 10 < 5 ? EvolutionDirection.S : EvolutionDirection.C;
		}

		int AbilityToNumber( AbilityIndex a )
		{
			return a == AbilityIndex.First ? 0 : 1;
		}

		bool IsShiny( uint p )
		{
			uint r = p ^ OriginalTrainer.Value;
			return ( ( r & 0xFFFF ) ^ ( r >> 16 ) ) < 8;
		}
	}
}