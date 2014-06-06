using System;

namespace PokeSave
{
	public class SortedBoxObject : IComparable<SortedBoxObject>
	{
		public uint Mark { get; set; }
		public uint MonsterId { get; set; }
		public byte[] Data { get; set; }

		public int CompareTo( SortedBoxObject other )
		{
			var tmark = (int) Mark * -256;
			var omark = (int) other.Mark * -256;
			return tmark + (int) MonsterId - ( (int) other.MonsterId + omark );

		}
	}
}