namespace PokeSave
{
	internal class Program
	{
		static void Main( string[] args )
		{
			new SimpleCommandLineClient( new ConsoleReader() ).Run( args );
		}
	}
}