
namespace PokeSave
{
	public class PcBuffer : GameSection
	{
		readonly GameSection[] _data;

		public PcBuffer( GameSection[] sections )
		{
			_data = sections;
		}

		public override byte this[int index]
		{
			get
			{
				var currentsection = 0;
				while( index >= _data[currentsection].Length )
				{
					index -= _data[currentsection++].Length;
				}
				return _data[currentsection][index];
			}

			set
			{
				var currentsection = 0;
				while( index >= _data[currentsection].Length )
				{
					index -= _data[currentsection++].Length;
				}
				_data[currentsection][index] = value;
			}
		}
	}
}