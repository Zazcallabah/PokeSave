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
					Header.SetBinding( Expander.HeaderProperty, new Binding( "Move" + value + "Name" ) { Mode = BindingMode.OneWay } );

					Move.PropertyName = "Move" + value + "Name";
					Move.ListSource = MoveList.All();


					PP.PropertyName = "PP" + value;
					PPBonus.PropertyName = "PPBonus" + value;
				}
			}
		}
	}
}
