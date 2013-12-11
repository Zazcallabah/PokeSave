using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using PokeSave;

namespace PokeEdit
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		readonly Dictionary<string, Editor> _editwindows;

		public MainWindow()
		{
			_editwindows = new Dictionary<string, Editor>();
			InitializeComponent();
			DataContext = new BindingList<SaveFile>();
			Drop += MainWindow_Drop;
			Info.Text = "This tool lives at https://github.com/Zazcallabah/PokeSave";
		}

		void MainWindow_Drop( object sender, DragEventArgs e )
		{
			if( e.Data.GetDataPresent( DataFormats.FileDrop ) )
				try
				{
					foreach( string file in (string[]) e.Data.GetData( DataFormats.FileDrop ) )
						AddSaveFile( new SaveFile( file ) );
				}
				catch( ArgumentException )
				{
					Info.Text = "Not valid file";
				}
		}

		void AddSaveFile( SaveFile saveFile )
		{
			var list = (BindingList<SaveFile>) DataContext;
			if( list.All( sf => sf.FileName != saveFile.FileName ) )
				list.Add( saveFile );
		}

		void LoadButtonClicked( object sender, RoutedEventArgs e )
		{
			var dlg = new OpenFileDialog { Multiselect = true };
			bool? result = dlg.ShowDialog();
			if( result == true )
				foreach( string name in dlg.FileNames )
				{
					AddSaveFile( new SaveFile( name ) );
					Info.Text = name;
				}
		}

		SaveFile ExtractSaveFileFromElement( Button button )
		{
			var buttoncontainer = (StackPanel) VisualTreeHelper.GetParent( button );
			var savecontainer = (StackPanel) VisualTreeHelper.GetParent( buttoncontainer );
			var sc = (SaveControl) savecontainer.Children[0];
			return (SaveFile) sc.DataContext;
		}

		void SaveAsButtonClicked( object sender, RoutedEventArgs e )
		{
			var dlg = new SaveFileDialog();
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

		void ClaimButtonClicked( object sender, RoutedEventArgs e )
		{
			SaveFile sf = ExtractSaveFileFromElement( (Button) sender );
			IEnumerable<MonsterEntry> l = sf.Latest.Team.Where( t => !t.Empty ).Concat(
				sf.Latest.PcBuffer.Where( t => !t.Empty ) );
			foreach( MonsterEntry me in l )
				me.MakeOwn( sf.Latest );
		}

		void HexButtonClicked( object sender, RoutedEventArgs e )
		{
			SaveFile sf = ExtractSaveFileFromElement( (Button) sender );
			if( _editwindows.ContainsKey( sf.FileName ) )
				_editwindows[sf.FileName].Focus();
			else
			{
				var editor = new Editor();
				editor.Closed += EditClosed;
				editor.DataContext = sf;
				_editwindows.Add( sf.FileName, editor );
				editor.Show();
			}
		}

		void EditClosed( object sender, EventArgs e )
		{
			var editor = (Editor) sender;
			editor.Closed -= EditClosed;
			var sf = (SaveFile) editor.DataContext;
			_editwindows.Remove( sf.FileName );
		}
	}
}