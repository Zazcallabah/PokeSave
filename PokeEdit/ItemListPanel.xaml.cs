﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
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
				if( item.Empty )
				{
					item.ID = 13;
					return;
				}
			}
		}
	}
}
