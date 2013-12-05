using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using PokeSave;

namespace PokeEdit
{
	public partial class TeamListPanel : UserControl
	{
		public TeamListPanel()
		{
			InitializeComponent();
		}

		void ActivateNextClicked( object sender, RoutedEventArgs e )
		{
			foreach( var entry in (BindingList<MonsterEntry>) DataContext )
			{
				if( entry.Empty )
				{
					entry.MonsterId = 1;
					return;
				}
			}
		}
	}
}
