using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PokeSave;

namespace PokeEdit
{
	public partial class DexHolderPanel : UserControl
	{
		public DexHolderPanel()
		{
			InitializeComponent();
		}

		private void RepairClick( object sender, System.Windows.RoutedEventArgs e )
		{
			DependencyObject current = this;
			while( !(current is SaveControl) )
			{
				current = VisualTreeHelper.GetParent( current );
			}

			var sf = (SaveFile)((SaveControl)current).DataContext;
			sf.Latest.RepairPokeDex();
		}
	}
}
