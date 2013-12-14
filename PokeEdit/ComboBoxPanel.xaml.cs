using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using PokeSave;

namespace PokeEdit
{
	public partial class ComboBoxPanel : UserControl
	{
		string _propertyName;

		public ComboBoxPanel()
		{
			InitializeComponent();
		}

		public string Label
		{
			get { return TextBlock.Text; }
			set { TextBlock.Text = value; }
		}

		public string ListSourceClass
		{
			get { return null; }
			set
			{ 
				Assembly assembly = Assembly.GetAssembly( typeof( MoveList ) );
				Type type = assembly.GetType( "PokeSave." + value );
				var m = type.GetMethod("All",BindingFlags.Static | BindingFlags.Public );
				ListSource = (string[])m.Invoke(null,null);
			}
		}

		public string[] ListSource
		{
			get { return null; }
			set { Combo.ItemsSource = value; }
		}

		public string DisplayProperty
		{
			get { return Combo.DisplayMemberPath; }
			set { Combo.DisplayMemberPath = value; }
		}

		public string EnumSource
		{
			get { return null; }

			set
			{
				Assembly assembly = Assembly.GetAssembly( typeof( GameType ) );
				Type type = assembly.GetType( "PokeSave." + value );
				Combo.ItemsSource = Enum.GetValues( type );
			}
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
					Combo.SetBinding( ComboBox.SelectedItemProperty, new Binding( _propertyName ) );
				}
			}
		}

		public bool SearchBox
		{
			get { return Combo.IsEditable && Combo.IsTextSearchEnabled; }
			set { Combo.IsTextSearchEnabled = Combo.IsEditable = value; }
		}
	}
}