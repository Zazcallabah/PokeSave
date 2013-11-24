using System.IO;

namespace PokeSave
{
	public class SimpleCommandLineClient
	{
		readonly IComms _com;
		readonly CommandLineParser _parser;
		SaveFile _current;

		public SimpleCommandLineClient( IComms com )
		{
			_com = com;
			_current = null;
			_parser = new CommandLineParser();
		}

		public void Run( string[] args )
		{
			while( true )
			{
				_com.Write( "\nld, st, l, r, w, p\n> " );
				string input = _com.ReadLine();
				if( input == "q" )
					return;
				if( input.StartsWith( "ld" ) )
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
				else if( input.StartsWith( "st" ) )
					if( _current == null )
						_com.WriteLine( "No file loaded" );
					else
					{
						string name = input.Substring( 2 ).Trim();
						if( File.Exists( name ) )
							_com.WriteLine( "File already exists" );
						else
							_current.Save( name );
					}
				else if( input.StartsWith( "p" ) )
					_com.WriteLine( _current == null ? "No file chosen" : _current.ToString() );
				else if( input.StartsWith( "l" ) )
					_com.WriteLine( _current == null ? "No file chosen" : _parser.List( _current, input.Substring( 1 ).Trim() ) );
				else if( input.StartsWith( "r" ) )
					_com.WriteLine( _current == null ? "No file chosen" : _parser.Read( _current, input.Substring( 1 ).Trim() ) );
				else if( input.StartsWith( "w" ) )
					_com.WriteLine( _current == null ? "No file chosen" : _parser.Write( _current, input.Substring( 1 ).Trim() ) );
			}
		}
	}
}