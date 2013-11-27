
namespace PokeSave.Client
{
	public interface IComms
	{
		string ReadLine();
		void Write( string str );
		void WriteLine( string line );
	}
}