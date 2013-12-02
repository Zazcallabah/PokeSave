
namespace PokeConsoleClient
{
	internal class Program
	{
		static void Main( string[] args )
		{
			new SimpleCommandLineClient( new InjectionQueueCommunicator( new ConsoleReader() ), "commands.json" ).Run( args );
		}
	}
}
