using System.Windows;
using PokeSave;

namespace PokeEdit
{
	/// <summary>
	/// Interaction logic for Editor.xaml
	/// </summary>
	public partial class Editor : Window
	{
		public Editor()
		{
			InitializeComponent();
		}

		void Load( object sender, RoutedEventArgs e )
		{
			Text.Text = ( (SaveFile) DataContext ).Latest.Section( Selection.SelectedIndex ).TextRepresentation;
		}
		void Save( object sender, RoutedEventArgs e )
		{
			( (SaveFile) DataContext ).Latest.Section( Selection.SelectedIndex ).TextRepresentation = Text.Text;
		}
	}
}
