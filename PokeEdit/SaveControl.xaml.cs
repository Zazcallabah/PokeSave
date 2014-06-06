using System.Windows;
using System.Windows.Controls;
using PokeSave;

namespace PokeEdit
{
	public partial class SaveControl : UserControl
	{
		public SaveControl()
		{
			InitializeComponent();
		}

		void ClaimClick( object sender, RoutedEventArgs e )
		{
			( (SaveFile) DataContext ).Latest.ClaimAll();
		}

		void SortClick( object sender, RoutedEventArgs e )
		{
			( (SaveFile) DataContext ).Latest.SortPC();
		}
	}
}
