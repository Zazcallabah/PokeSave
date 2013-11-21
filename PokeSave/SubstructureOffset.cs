
using System;

namespace PokeSave
{
	public static class SubstructureOffset
	{
		static readonly int[][] NormalizedIndexes = new int[][]{
			new []{0,0,0,0,0,0},
			new []{1,1,2,3,2,3},
			new []{2,3,1,1,3,2},
			new []{3,2,3,2,1,1}
		};
		static readonly int[] IndexG = new[] { 0, 1, 1, 1 };
		static readonly int[] IndexA = new[] { 1, 0, 2, 2 };
		static readonly int[] IndexE = new[] { 2, 2, 0, 3 };
		static readonly int[] IndexM = new[] { 3, 3, 3, 0 };

		public static int Growth( uint personality )
		{
			return Select( IndexG, personality );
		}
		public static int Misc( uint personality )
		{
			return Select( IndexM, personality );
		}
		public static int Action( uint personality )
		{
			return Select( IndexA, personality );
		}
		public static int EVs( uint personality )
		{
			return Select( IndexE, personality );
		}
		static int Select( int[] indexer, uint personality )
		{
			var selector = personality % 24;
			var list = Math.Floor( selector / 6.0 );
			var index = indexer[(int) list];
			return NormalizedIndexes[index][selector % 6];
		}
	}
}
