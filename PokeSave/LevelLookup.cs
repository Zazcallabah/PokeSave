using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokeSave
{
	public class LevelLookup
	{
		static readonly IDictionary<ExperienceRate, uint[]> LookupTable;
		static LevelLookup()
		{
			LookupTable = new Dictionary<ExperienceRate, uint[]>();
			LookupTable.Add( ExperienceRate.Slow, PopulateArray( i => (uint) ( ( 5 * i * i * i ) / 4.0 ) ) );
			LookupTable.Add( ExperienceRate.MediumFast, PopulateArray( i => i * i * i ) );
			LookupTable.Add( ExperienceRate.Fast, PopulateArray( i => ( 4 * i * i * i ) / 5 ) );
			LookupTable.Add( ExperienceRate.MediumSlow, PopulateArray( i => ( 6 * i * i * i ) / 5 - 15 * i * i + 100 * i - 140 ) );
			LookupTable.Add( ExperienceRate.Fluctuating, PopulateArray( i =>
				{
					double res = 0;
					if( i <= 15 )
						res = ( ( i + 1 ) / 3 ) + 24;
					else if( i <= 36 )
						res = i + 14;
					else
						res = ( i / 2 ) + 32;
					return (uint) Math.Floor( i * i * i * ( res / 50.0 ) );
				}
				) );
			LookupTable.Add( ExperienceRate.Erratic, PopulateArray( i =>
				{
					if( i <= 50 )
						return ( i * i * i * ( 100 - i ) ) / 50;
					if( i <= 68 )
						return ( i * i * i * ( 150 - i ) ) / 100;
					if( i <= 98 )
						return ( i * i * i * ( ( 1911 - 10 * i ) / 3 ) ) / 500;
					return ( i * i * i * ( 160 - i ) ) / 100;
				}
				) );

		}
		static uint[] PopulateArray( Func<uint, uint> calculate )
		{
			var arr = new uint[101];
			for( uint i = 0; i < arr.Length; i++ )
			{
				arr[i] = calculate( i );
			}
			return arr;
		}
		public static uint CalculatedLevel( ExperienceRate rate, uint exp )
		{

			for( uint i = 100; i >= 1; i-- )
			{
				if( BigEnoughForLevel( LookupTable[rate], exp, i ) )
					return i;
			}

			return 1;
		}

		static bool BigEnoughForLevel( uint[] lookup, uint exp, uint level )
		{
			if( level == 1 )
				return true;
			return exp >= lookup[level];
		}


	}
}
