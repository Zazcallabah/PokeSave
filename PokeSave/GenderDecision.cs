
namespace PokeSave
{
	public struct GenderDecision
	{
		public readonly MonsterGender ExpectedGender;
		public readonly byte TypeGenderByte;

		public GenderDecision( MonsterGender g, uint index )
			:
				this( g, MonsterList.Get( index ) ) { }

		public GenderDecision( MonsterGender g, MonsterInfo t )
		{
			ExpectedGender = g;
			TypeGenderByte = t.Gender;
		}
	}
}