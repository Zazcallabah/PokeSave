﻿using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using PokeSave;

namespace PokeEdit
{
	public partial class TeamListPanel : UserControl
	{
		public TeamListPanel()
		{
			InitializeComponent();
		}

		void CopyClicked( object sender, RoutedEventArgs e )
		{
			var sb = new StringBuilder();
			foreach( var entry in ( (BindingList<MonsterEntry>) DataContext ).Where( m => !m.Empty ) )
				sb.AppendLine( entry.RawDataString );

			Clipboard.SetText( sb.ToString() );
		}

		void SetInFirstEmptySlot( string data )
		{
			var next = ( (BindingList<MonsterEntry>) DataContext ).FirstOrDefault( m => m.Empty );
			if( next != null )
				next.RawDataString = data;
		}

		void PasteClicked( object sender, RoutedEventArgs e )
		{
			try
			{
				var data = Clipboard.GetText().Split( new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries );
				foreach( var line in data )
					SetInFirstEmptySlot( line );
			}
			catch( Exception ) { }
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

		void ImportClicked( object sender, RoutedEventArgs e )
		{
			var dlg = new OpenFileDialog { Multiselect = true };
			bool? result = dlg.ShowDialog();
			if( result == true )
				foreach( string name in dlg.FileNames )
				{
					var data = File.ReadAllBytes( name );
					if( data.Length == 80 || data.Length == 100 )
					{
						foreach( var entry in (BindingList<MonsterEntry>) DataContext )
						{
							if( entry.Empty )
							{
								entry.RawData = data;
								break;
							}
						}
					}
				}

		}

		void ClaimClicked( object sender, RoutedEventArgs e )
		{
			var r = this.FirstAncestorOfType<SaveControl>();
			foreach( var entry in (BindingList<MonsterEntry>) DataContext )
			{
				entry.MakeOwn( ( (SaveFile) r.DataContext ).Latest );
			}
		}
	}
}
