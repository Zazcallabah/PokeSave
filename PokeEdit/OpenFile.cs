using System;
using System.ComponentModel;
using System.Windows.Input;
using PokeSave;

namespace PokeEdit
{
	/// <summary>
	/// Controller entity for opened file
	/// </summary>
	public class OpenFile : INotifyPropertyChanged
	{
		public OpenFile( IHaveDirtyState data )
		{
			Data = data;
			Data.PropertyChanged += ( s, e ) => InvokePropertyChanged( "Data" );
			EditCommand = new RelayCommand( InvokeEdit );
			StopEditCommand = new RelayCommand( InvokeStopEdit );
			ClaimCommand = new RelayCommand( Claim );
		}

		void Claim()
		{
			if( Type == FileType.Gen3Save )
				( (SaveFile) Data ).Latest.ClaimAll();
		}

		/// <summary>
		/// PKM types uses path for a hash to avoid duplicates.
		/// </summary>
		public string Path { get; set; }
		public string Label { get; set; }
		public IHaveDirtyState Data { get; private set; }
		public FileType Type { get; set; }

		public ICommand EditCommand { get; private set; }
		public ICommand StopEditCommand { get; private set; }
		public ICommand ClaimCommand { get; private set; }
		public ICommand SaveCommand { get; private set; }

		public event EventHandler<EventArgs> Edit;
		public event EventHandler<EventArgs> StopEdit;

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

		public event PropertyChangedEventHandler PropertyChanged;

		void InvokePropertyChanged( string propertyName )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}
}