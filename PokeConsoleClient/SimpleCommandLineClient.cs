using System.IO;
using PokeSave;

namespace PokeConsoleClient
{
	public class SimpleCommandLineClient
	{
		readonly InjectionQueueCommunicator _com;
		readonly CommandLineParser _parser;
		readonly CommandHandler _handler;

		SaveFile _current;


		public SimpleCommandLineClient( InjectionQueueCommunicator com, string commandsfile )
			: this( com, new CommandHandler( commandsfile ) )
		{
		}

		public SimpleCommandLineClient( InjectionQueueCommunicator com, CommandHandler commandsfile )
		{
			_com = com;
			_current = null;
			_parser = new CommandLineParser();
			_handler = commandsfile;
		}

		public void Run( string[] args )
		{
			_com.WriteLine( "ld, st, l, r, w, p" );
			string lastresult = string.Empty;
			while( true )
			{
				_com.Write( "> " );
				string input = _com.ReadLine();
				if( input == "q" )
					return;
				if( input.StartsWith( "ld" ) )
					LoadFile( input );
				else if( input.StartsWith( "st" ) )
					SaveFile( input.Substring( 2 ).Trim() );
				else if( input.StartsWith( "p" ) )
					_com.WriteLine( _current == null ? "No file chosen" : _current.Latest.ToString() );
				else if( input.StartsWith( "l" ) )
					_com.WriteLine( _current == null ? "No file chosen" : _parser.List( _current, input.Substring( 1 ).Trim() ) );
				else if( input.StartsWith( "r" ) )
				{
					lastresult = _parser.Read( _current, input.Substring( 1 ).Trim() );
					_com.WriteLine( _current == null ? "No file chosen" : lastresult );
				}
				else if( input.StartsWith( "w" ) )
					_com.WriteLine(
						_current == null
							? "No file chosen"
							: _parser.Write( _current, input.Substring( 1 ).Replace( "{}", lastresult ).Trim() ) );
				else if( input.StartsWith( "cl" ) )
				{
					foreach( var c in _handler.All() )
					{
						_com.WriteLine( c.ToString() );
					}
				}
				else if( input.StartsWith( "c" ) )
				{
					var lines = _handler.Get( input.Substring( 1 ).Trim() );
					if( lines == null )
						_com.WriteLine( "not valid command name" );
					else
						_com.Inject( lines.Actions );
				}

			}
		}

		void SaveFile( string name )
		{
			if( _current == null )
			{
				_com.WriteLine( "No file chosen" );
				return;
			}
			_current.SaveAs( name );
		}

		void LoadFile( string input )
		{
			string name = input.Substring( 2 ).Trim();
			if( !File.Exists( name ) )
				_com.WriteLine( "No such file" );
			else if( new FileInfo( name ).Length != 128 * 1024 )
				_com.WriteLine( "File has wrong size" );
			else
			{
				_current = new SaveFile( name );
				if( !_current.A.Valid )
					_com.WriteLine( "Warning, game save A not valid" );
				if( !_current.B.Valid )
					_com.WriteLine( "Warning, game save B not valid" );
			}
		}
	}
}