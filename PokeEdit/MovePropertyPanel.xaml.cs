using System.Windows.Controls;
using System.Windows.Data;
using PokeSave;

namespace PokeEdit
{
	public partial class MovePropertyPanel : UserControl
	{
		int _index;

		public MovePropertyPanel()
		{
			InitializeComponent();
		}

		public int Index
		{
			get
			{
				return _index;
			}
			set
			{
				if( _index != value )
				{
					_index = value;
					var movename = "Move" + value + "Name";
					Header.SetBinding( Expander.HeaderProperty, new Binding( movename ) { Mode = BindingMode.OneWay } );

					Move.PropertyName = movename;
					PP.PropertyName = "PP" + value;
					PPBonus.PropertyName = "PPBonus" + value;
				}
			}
		}
	}
}
