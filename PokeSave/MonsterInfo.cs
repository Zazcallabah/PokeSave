using System.Text;

namespace PokeSave
{
	public struct MonsterInfo
	{
		public MonsterInfo( byte[] source, uint index, uint offset )
		{
			Name = NameList.Get( index );
			HP = source[offset];
			Attack = source[offset + 1];
			Defense = source[offset + 2];
			Speed = source[offset + 3];
			SpAttack = source[offset + 4];
			SpDefense = source[offset + 5];
			Type1 = source[offset + 6];
			Type2 = source[offset + 7];
			Gender = source[offset + 16];
		}
		public string Name;
		public byte HP;
		public byte Attack;
		public byte Defense;
		public byte Speed;
		public byte SpAttack;
		public byte SpDefense;
		public byte Type1;
		public byte Type2;
		public byte Gender;

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append( Name );
			sb.Append( " hp:" + HP );
			sb.Append( " t1:" + Type1 );
			sb.Append( " t2:" + Type2 );
			sb.Append( " a:" + Attack );
			sb.Append( " d:" + Defense );
			return sb.ToString();
		}
	}
}