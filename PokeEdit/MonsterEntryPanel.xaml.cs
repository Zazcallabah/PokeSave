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
			Clipboard.SetText( ( (MonsterEntry) DataContext ).RawDataString );
		}

		void PasteClicked( object sender, RoutedEventArgs e )
		{
			try
			{
				( (MonsterEntry) DataContext ).RawDataString = Clipboard.GetText();
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
