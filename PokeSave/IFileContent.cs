using System.ComponentModel;

namespace PokeSave
{
	public interface IFileContent : INotifyPropertyChanged
	{
		bool IsDirty { get; }

		void Save( string path );
	}
}