using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using PokeSave;

namespace PokeEdit
{
	public partial class ItemListPanel : UserControl
	{
		public ItemListPanel()
		{
			InitializeComponent();
		}

		public string Header
		{
			get { return (string) BorderLabel.Header; }
			set { BorderLabel.Header = value; }
		}

		void ActivateNextClicked( object sender, RoutedEventArgs e )
		{
			foreach( var item in (BindingList<ItemEntry>) DataContext )
			{
				if( item.ID == 0 )
				{
					item.ID = 13;
					return;
				}
			}
		}
	}

	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (bool) value ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (Visibility) value == Visibility.Collapsed;
		}
	}

}
