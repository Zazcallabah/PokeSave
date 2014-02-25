using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PokeSave;

namespace PokeEdit
{
	internal class Controller : INotifyPropertyChanged
	{
		public Controller()
		{
			Gen3Saves = new BindingList<OpenFile>();
			PKM = new BindingList<OpenFile>();
			OpenFiles = new BindingList<OpenFile>();

			Gen3Saves.ListChanged += ( s, e ) => InvokePropertyChanged( "Gen3Saves" );
			PKM.ListChanged += ( s, e ) => InvokePropertyChanged( "PKM" );
			OpenFiles.ListChanged += ( s, e ) => InvokePropertyChanged( "OpenFiles" );
			OpenFiles.ListChanged += SubscribeToButtons;
		}

		void SubscribeToButtons( object sender, ListChangedEventArgs listargs )
		{
			if( listargs.ListChangedType == ListChangedType.ItemAdded )
			{
				var item = OpenFiles[listargs.NewIndex];
				item.Edit += EditFile;
				item.StopEdit += StopEditFile;
				item.Close += CloseFile;
			}
		}

		void CloseFile( object sender, EventArgs e )
		{
			StopEditFile( sender, e );
			var file = (OpenFile) sender;
			file.Edit -= EditFile;
			file.StopEdit -= StopEditFile;
			file.Close -= CloseFile;

			OpenFiles.Remove( file );
		}

		public void StopEditFile( object sender, EventArgs e )
		{
			var file = (OpenFile) sender;
			var list = GetListForType( file.Type );
			if( list.Any( ft => ft.Path == file.Path ) )
				list.Remove( file );
		}

		public void EditFile( object sender, EventArgs e )
		{
			var file = (OpenFile) sender;
			var list = GetListForType( file.Type );
			if( list.All( ft => ft.Path != file.Path ) )
				list.Add( file );
		}

		BindingList<OpenFile> GetListForType( FileType type )
		{
			if( type == FileType.PKM )
				return PKM;
			if( type == FileType.Gen3Save )
				return Gen3Saves;
			return null;
		}

		// commands

		// bulk commands
		// export pkm to stringfile - query for gen
		// export pkm to zipped 3gpkm (and other gens)
		// merge
		// claim - queries for original trainer
		// save (saves file as is)

		// filters for selecting all files of one type, for easy bulk export

		public BindingList<OpenFile> OpenFiles { get; private set; }
		public BindingList<OpenFile> Gen3Saves { get; private set; }
		public BindingList<OpenFile> PKM { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		void InvokePropertyChanged( string propertyName )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}

	public enum FileType { Gen3Save, PKM }
}