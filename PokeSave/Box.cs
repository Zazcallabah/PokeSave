using System.ComponentModel;

namespace PokeSave
{
	public class Box
	{
		readonly int _index;
		public Box( int index )
		{
			_index = index;
			Content = new BindingList<MonsterEntry>();
		}

		public string Title { get { return "#" + ( _index + 1 ); } }
		public BindingList<MonsterEntry> Content { get; private set; }
	}
}