
namespace PokeSave
{
	public class PcBuffer : GameSection
	{
		readonly GameSection[] _data;

		public PcBuffer( GameSection[] sections )
		{
			_data = sections;
		}

		public override uint GetByte( int offset )
		{
			var currentsection = 0;
			while( offset >= _data[currentsection].Length )
			{
				offset -= _data[currentsection++].Length;
			}
			return _data[currentsection].GetByte( offset );
		}

	}
}