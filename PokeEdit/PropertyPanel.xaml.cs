using System.Windows.Controls;
using System.Windows.Data;

namespace PokeEdit
{
	/// <summary>
	/// Interaction logic for PropertyPanel.xaml
	/// </summary>
	public partial class PropertyPanel : UserControl
	{
		string _propertyName;

		public PropertyPanel()
		{
			InitializeComponent();
		}

		public string Label
		{
			get { return TextBlock.Text; }
			set { TextBlock.Text = value; }
		}

		public string PropertyName
		{
			get { return _propertyName; }
			set
			{
				if( value != _propertyName )
				{
					if( string.IsNullOrEmpty( Label ) )
						Label = value;
					_propertyName = value;
					TextBox.SetBinding( TextBox.TextProperty, new Binding( _propertyName ) );
				}
			}
		}
	}
}
