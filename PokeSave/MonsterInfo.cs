using System.Text;

namespace PokeSave
{
	public class MonsterInfo
	{
		public byte Ability1;
		public byte Ability2;
		public byte Attack;
		public byte BaseFriendship;
		public byte BaseXpYield;
		public byte CatchRate;
		public byte Color;
		public byte Defense;
		public uint EffortYield;
		public byte EggGroup1;
		public byte EggGroup2;
		public byte Gender;
		public byte HP;
		public uint Item1;
		public uint Item2;
		public ExperienceRate LevelUpType;
		public string Name { get; private set; }
		public byte SafariRate;
		public byte SpAttack;
		public byte SpDefense;
		public byte Speed;
		public byte StepsToHatch; // *256
		public MonsterType Type1;
		public MonsterType Type2;

		public MonsterInfo( byte[] source, uint index, uint offset )
		{
			HP = source[offset];
			Attack = source[offset + 1];
			Defense = source[offset + 2];
			Speed = source[offset + 3];
			SpAttack = source[offset + 4];
			SpDefense = source[offset + 5];
			Type1 = (MonsterType) source[offset + 6];
			Type2 = (MonsterType) source[offset + 7];
			CatchRate = source[offset + 8];
			BaseXpYield = source[offset + 9];
			EffortYield = (uint) ( source[offset + 10] | ( source[offset + 11] << 8 ) );
			Item1 = (uint) ( source[offset + 12] | ( source[offset + 13] << 8 ) );
			Item2 = (uint) ( source[offset + 14] | ( source[offset + 15] << 8 ) );
			Gender = source[offset + 16];
			StepsToHatch = source[offset + 17];
			BaseFriendship = source[offset + 18];
			LevelUpType = (ExperienceRate) source[offset + 19];
			EggGroup1 = source[offset + 20];
			EggGroup2 = source[offset + 21];
			Ability1 = source[offset + 22];
			Ability2 = source[offset + 23];
			SafariRate = source[offset + 24];
			Color = source[offset + 25];
			Name = NameList.Get( index );

		}

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