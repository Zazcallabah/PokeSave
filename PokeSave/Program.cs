using PokeSave.Client;

namespace PokeSave
{
	internal class Program
	{
		static void Main( string[] args )
		{
			new SimpleCommandLineClient( new InjectionQueueCommunicator( new ConsoleReader() ), "commands.json" ).Run( args );
		}
	}
}