﻿using System.Windows.Controls;
using System.Windows.Data;

namespace PokeEdit
{
	/// <summary>
	/// Interaction logic for ReadOnlyPropertyPanel.xaml
	/// </summary>
	public partial class ReadOnlyPropertyPanel : UserControl
	{
		string _propertyName;

		public ReadOnlyPropertyPanel()
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
					ReadOnly.SetBinding( TextBlock.TextProperty, new Binding( _propertyName ) { Mode = BindingMode.OneWay } );
				}
			}
		}
	}
}
