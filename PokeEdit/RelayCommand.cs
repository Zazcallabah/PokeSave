using System;
using System.Windows.Input;

namespace PokeEdit
{
	public class RelayCommand : ICommand
	{
		readonly Action _do;
		public RelayCommand( Action @do )
		{
			_do = @do;
		}

		public void Execute( object parameter )
		{
			_do();
		}

		public bool CanExecute( object parameter )
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;
	}
}