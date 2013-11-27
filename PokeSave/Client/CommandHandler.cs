using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PokeSave.Client
{
	public class CommandHandler
	{
		readonly IDictionary<string, Command> _commands;

		public CommandHandler( IEnumerable<Command> commands )
		{
			_commands = commands.ToDictionary( c => c.Trigger );
		}

		public CommandHandler( string filename )
			: this( JsonConvert.DeserializeObject<IList<Command>>( File.ReadAllText( filename ), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore } ) )
		{ }

		public IEnumerable<Command> All()
		{
			return _commands.Values;
		}

		public Command Get( string trigger )
		{
			return _commands.ContainsKey( trigger ) ? _commands[trigger] : null;
		}
	}
}