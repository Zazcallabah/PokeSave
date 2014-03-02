using System.Windows.Controls;
using System.Windows.Data;

namespace PokeEdit
{
	/// <summary>
	/// Interaction logic for StatsPanel.xaml
	/// </summary>
	public partial class StatsPanel : UserControl
	{
		string _propertyName;

		public StatsPanel()
		{
			InitializeComponent();
		}

		public string Stat
		{
			get { return _propertyName; }
			set
			{
				if( value != _propertyName )
				{
					_propertyName = value;
					Text.Text = value;
					IV.SetBinding( TextBox.TextProperty, new Binding( _propertyName + "IV" ) );
					EV.SetBinding( TextBox.TextProperty, new Binding( _propertyName + "EV" ) );
					Calc.SetBinding( TextBlock.TextProperty, new Binding( "Calculated" + _propertyName ) );
					Actual.SetBinding( TextBlock.TextProperty, new Binding( "Current" + _propertyName ) );
				}
			}
		}
	}
}
