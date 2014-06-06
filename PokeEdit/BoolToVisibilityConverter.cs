using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace PokeEdit
{
	public class SelectedOpenFilesToBooleanConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var list = value as BindingList<OpenFile>;
			if( list == null )
				return false;
			return list.Any( o => o.Selected );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return null;
		}
	}

	public class EmptyListMeansVisibleConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (int) value == 0 ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return null;
		}
	}

	public class EmptyListMeansCollapsedConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return (int) value == 0 ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return null;
		}
	}

	public class BoolToFontWeightConverter : IValueConverter
	{
		public object Convert( object value, Type targetType,
			object parameter, CultureInfo culture )
		{
			var b = value as bool?; // maybe never works?
			if( b.HasValue && b.Value )
				return FontWeights.Bold;
			else
				return FontWeights.Normal;
		}

		public object ConvertBack( object value, Type targetType,
			object parameter, CultureInfo culture )
		{
			return false;
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