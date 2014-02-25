using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;
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
#if DEBUG
			Add( @"C:\src\VisualBoyAdvance 1.8.0 beta 3\pfr.sa1" );
			Add( @"C:\src\xs\data.txt" );
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
			if( _controller.OpenFiles.Any( f => f.Path == path ) )
				return;

			var data = File.ReadAllBytes( path );

			if( FileTypeDetector.IsGen3SaveFile( data ) )
			{
				var file = new SaveFile( data, path );
				var fileinfo = new FileInfo( path );
				_controller.OpenFiles.Add(
					new OpenFile( file )
					{
						Path = path,
						Type = FileType.Gen3Save,
						Label = fileinfo.Name
					} );
			}
			else
			{
				var entries = FileTypeDetector.Open( data );
				if( entries != null )
				{
					foreach( var entry in entries )
					{
						var existing = _controller.OpenFiles.Where( f => f.Type == FileType.PKM ).ToList();
						var md5 = CalculateMD5Hash( entry.RawData );
						if( existing.All( e => e.Path != md5 ) )
						{
							_controller.OpenFiles.Add( new OpenFile( entry )
							{
								Path = md5,
								Label = entry.TypeName + entry.Name,
								Type = FileType.PKM
							} );
						}
					}
				}
			}
		}

		public string CalculateMD5Hash( byte[] data )
		{
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] hash = md5.ComputeHash( data );

			StringBuilder sb = new StringBuilder();
			for( int i = 0; i < hash.Length; i++ )
			{
				sb.Append( hash[i].ToString( "X2" ) );
			}
			return sb.ToString();
		}

		void OpenClick( object sender, RoutedEventArgs e )
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

		SaveFile ExtractSaveFileFromElement( Button button )
		{
			var buttoncontainer = (StackPanel) VisualTreeHelper.GetParent( button );
			var savecontainer = (StackPanel) VisualTreeHelper.GetParent( buttoncontainer );
			var sc = (SaveControl) savecontainer.Children[0];
			return (SaveFile) sc.DataContext;
		}
		
		void SaveButtonClicked( object sender, RoutedEventArgs e )
		{
			ExtractSaveFileFromElement( (Button) sender ).Save();
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

		void Browser( object sender, RequestNavigateEventArgs e )
		{
			Process.Start( new ProcessStartInfo( e.Uri.AbsoluteUri ) );
			e.Handled = true;
		}
	}
}