using System;
using System.Windows;
using System.Windows.Controls;
using PokeSave;

namespace PokeEdit
{
	public partial class MonsterEntryPanel : UserControl
	{
		public MonsterEntryPanel()
		{
			InitializeComponent();
		}

		void CopyClicked( object sender, RoutedEventArgs e )
		{
			var data = ( (MonsterEntry) DataContext ).RawData;
			Clipboard.SetText( Convert.ToBase64String( data ) );
		}

		void PasteClicked( object sender, RoutedEventArgs e )
		{
			try
			{
				var data = Convert.FromBase64String( Clipboard.GetText() );
				( (MonsterEntry) DataContext ).RawData = data;
			}
			catch( Exception )
			{ }
		}

		void RemoveClicked( object sender, RoutedEventArgs e )
		{
			( (MonsterEntry) DataContext ).Clear();
		}
	}
}
