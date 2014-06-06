using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using PokeSave;

namespace PokeEdit
{
	/// <summary>
	///     Controller entity for opened file
	/// </summary>
	public class OpenFile : INotifyPropertyChanged
	{
		bool _selected;

		public OpenFile( IFileContent data )
		{
			Data = data;
			Data.PropertyChanged += ( s, e ) => InvokePropertyChanged( "Data" );
			EditCommand = new RelayCommand( InvokeEdit );
			StopEditCommand = new RelayCommand( InvokeStopEdit );
			ClaimCommand = new RelayCommand( Claim );
			SaveAsCommand = new RelayCommand( SaveAs );
			CloseCommand = new RelayCommand( InvokeClose );
		}

		/// <summary>
		///     PKM types uses path for a hash to avoid duplicates.
		/// </summary>
		public string Path { get; set; }

		public string Label { get; set; }
		public IFileContent Data { get; private set; }
		public FileType Type { get; set; }

		public bool Selected
		{
			get { return _selected; }
			set
			{
				if( value != _selected )
				{
					_selected = value;
					InvokePropertyChanged( "Selected" );
				}
			}
		}

		public ICommand EditCommand { get; private set; }
		public ICommand StopEditCommand { get; private set; }
		public ICommand ClaimCommand { get; private set; }
		public ICommand SaveAsCommand { get; private set; }
		public ICommand CloseCommand { get; private set; }
		public event PropertyChangedEventHandler PropertyChanged;

		void Claim()
		{
			if( Type == FileType.Gen3Save )
				( (SaveFile) Data ).Latest.ClaimAll();
		}

		void SaveAs()
		{
			var dlg = new SaveFileDialog();
			bool? result = dlg.ShowDialog();

			if( result == true )
				Data.Save( dlg.FileName );
		}

		public void SaveWithBackup()
		{
			if( File.Exists( Path ) )
			{
				string tmp = Path;
				int i = 1;
				while( File.Exists( tmp ) )
					tmp = Path + "." + ( i++ );
				File.Move( Path, tmp );
			}
			Data.Save( Path );

		}

		public void Merge( OpenFile external )
		{
			var externalsave = ( (SaveFile) external.Data ).Latest;
			( (SaveFile) Data ).Latest.Merge( externalsave );
		}

		public void Repair()
		{
			( (SaveFile) Data ).Latest.RepairPokeDex();
		}

		public event EventHandler<EventArgs> Edit;
		public event EventHandler<EventArgs> StopEdit;
		public event EventHandler<EventArgs> Close;

		void InvokeClose()
		{
			if( Close != null )
				Close( this, EventArgs.Empty );
		}

		void InvokeStopEdit()
		{
			if( StopEdit != null )
				StopEdit( this, EventArgs.Empty );
		}

		void InvokeEdit()
		{
			if( Edit != null )
				Edit( this, EventArgs.Empty );
		}

		void InvokePropertyChanged( string propertyName )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}
}