﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PokeSave;

namespace PokeEdit
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new BindingList<SaveFile>();
			Drop += MainWindow_Drop;
		}

		void MainWindow_Drop( object sender, DragEventArgs e )
		{
			if( e.Data.GetDataPresent( DataFormats.FileDrop ) )
			{
				try
				{
					foreach( var file in (string[]) e.Data.GetData( DataFormats.FileDrop ) )
						AddSaveFile( new SaveFile( file ) );
				}
				catch( ArgumentException )
				{
					Info.Text = "Not valid file";
				}
			}
		}

		void AddSaveFile( SaveFile saveFile )
		{
			( (BindingList<SaveFile>) DataContext ).Add( saveFile );
		}

		void LoadButtonClicked( object sender, RoutedEventArgs e )
		{
			var dlg = new Microsoft.Win32.OpenFileDialog() { Multiselect = true };
			bool? result = dlg.ShowDialog();
			if( result == true )
			{
				foreach( var name in dlg.FileNames )
				{
					AddSaveFile( new SaveFile( name ) );
					Info.Text = name;
				}
			}
		}

		SaveFile ExtractSaveFileFromElement( Button button )
		{
			var buttoncontainer = VisualTreeHelper.GetParent( button ) as StackPanel;
			var savecontainer = VisualTreeHelper.GetParent( buttoncontainer ) as StackPanel;
			var sc = savecontainer.Children[0] as SaveControl;
			return sc.DataContext as SaveFile;
		}

		void SaveAsButtonClicked( object sender, RoutedEventArgs e )
		{
			var dlg = new Microsoft.Win32.SaveFileDialog();
			bool? result = dlg.ShowDialog();

			if( result == true )
			{
				string filename = dlg.FileName;
				ExtractSaveFileFromElement( (Button) sender ).SaveAs( filename );
			}
		}

		void SaveButtonClicked( object sender, RoutedEventArgs e )
		{
			ExtractSaveFileFromElement( (Button) sender ).Save();
		}
	}
}