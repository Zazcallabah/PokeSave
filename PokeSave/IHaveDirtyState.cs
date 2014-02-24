using System.ComponentModel;

namespace PokeSave
{
	public interface IHaveDirtyState : INotifyPropertyChanged
	{
		bool IsDirty { get; }
	}
}