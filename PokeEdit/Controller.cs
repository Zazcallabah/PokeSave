using System.ComponentModel;
using PokeSave;

namespace PokeEdit
{
	internal class Controller : INotifyPropertyChanged
	{
		public Controller()
		{
			Gen3Saves = new BindingList<SaveFile>();
			PKM = new BindingList<MonsterEntry>();

			Gen3Saves.ListChanged += Gen3Saves_ListChanged;
			PKM.ListChanged += PKM_ListChanged;
		}

		void PKM_ListChanged( object sender, ListChangedEventArgs e )
		{
			OnPropertyChanged( "PKM" );
		}

		void Gen3Saves_ListChanged( object sender, ListChangedEventArgs e )
		{
			OnPropertyChanged( "Gen3Saves" );
		}
		public BindingList<SaveFile> Gen3Saves { get; private set; }
		public BindingList<MonsterEntry> PKM { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged( string propertyName )
		{
			var handler = PropertyChanged;
			if( handler != null )
				handler( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}
}