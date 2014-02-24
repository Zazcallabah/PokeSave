using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PokeEdit
{
	/// <summary>
	/// </summary>
	public partial class FilePanel : UserControl
	{

		public FilePanel()
		{
			InitializeComponent();
		}
	}

	public class BoolToFontWeightConverter : IValueConverter
	{
		public object Convert( object value, Type targetType,
			object parameter, CultureInfo culture )
		{
			var b = value as bool?;
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
}
