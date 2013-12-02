
namespace PokeConsoleClient
{
	public class Command
	{
		public string Name { get; set; }
		public string[] Actions { get; set; }
		public string Trigger { get; set; }
		public string Description { get; set; }

		public override string ToString()
		{
			return Name + ": " + Description;
		}
	}
}
