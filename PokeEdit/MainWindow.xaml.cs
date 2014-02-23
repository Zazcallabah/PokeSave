using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
		readonly Controller _controller;

		public MainWindow()
		{
			_editwindows = new Dictionary<string, Editor>();
			DataContext = _controller = new Controller();
			InitializeComponent();
			Drop += MainWindowDrop;
			Info.Text = "This tool lives at https://github.com/Zazcallabah/PokeSave";
#if DEBUG
			Add( @"C:\src\VisualBoyAdvance 1.8.0 beta 3\pfr.sa1" );
#endif
		}

		void MainWindowDrop( object sender, DragEventArgs e )
		{
			if( e.Data.GetDataPresent( DataFormats.FileDrop ) )
				try
				{
					foreach( string file in (string[]) e.Data.GetData( DataFormats.FileDrop ) )
						Add( file );
				}
				catch( ArgumentException )
				{
					Info.Text = "Not valid file";
				}
		}

		void Add( string path )
		{
			var data = File.ReadAllBytes( path );
			if( FileTypeDetector.IsGen3SaveFile( data ) )
				AddGen3SaveFile( new SaveFile( data, path ) );
			else
				AddPkmFile( FileTypeDetector.Open( data ) );
		}

		void AddPkmFile( IEnumerable<MonsterEntry> entries )
		{
			if( entries != null )
			{
				foreach( var entry in entries )
					_controller.PKM.Add( entry );
			}
		}

		void AddGen3SaveFile( SaveFile saveFile )
		{
			if( _controller.Gen3Saves.All( sf => sf.FileName != saveFile.FileName ) )
				_controller.Gen3Saves.Add( saveFile );
		}

		void LoadButtonClicked( object sender, RoutedEventArgs e )
		{
			var dlg = new OpenFileDialog { Multiselect = true };
			bool? result = dlg.ShowDialog();
			if( result == true )
				foreach( string name in dlg.FileNames )
				{
					Add( name );
					Info.Text = name;
				}
		}

		void MergeButtonClicked( object sender, RoutedEventArgs e )
		{
			var list = _controller.Gen3Saves;

			for( int i = 0; i < list.Count; i++ )
			{
				for( int j = 0; j < list.Count; j++ )
				{
					if( i != j )
						list[i].Latest.Merge( list[j].Latest );
				}
				list[i].Latest.RepairPokeDex();
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

		void CloseButtonClicked( object sender, RoutedEventArgs e )
		{
			SaveFile sf = ExtractSaveFileFromElement( (Button) sender );
			_controller.Gen3Saves.Remove( sf );
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