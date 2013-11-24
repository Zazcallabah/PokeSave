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

		void SaveFileWithBackup( string name )
		{
			if( _current == null )
			{
				_com.WriteLine( "No file chosen" );
				return;
			}

			if( File.Exists( name ) )
			{
				string tmp = name;
				int i = 1;
				while( File.Exists( tmp ) )
				{
					tmp = name + "." + ( i++ );
				}
				File.Move( name, tmp );
			}
			_current.Save( name );
		}

		public void Run( string[] args )
		{
			string lastresult = string.Empty;
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
					SaveFileWithBackup( input.Substring( 2 ).Trim() );
				else if( input.StartsWith( "p" ) )
					_com.WriteLine( _current == null ? "No file chosen" : _current.ToString() );
				else if( input.StartsWith( "l" ) )
					_com.WriteLine( _current == null ? "No file chosen" : _parser.List( _current, input.Substring( 1 ).Trim() ) );
				else if( input.StartsWith( "r" ) )
				{
					lastresult = _parser.Read( _current, input.Substring( 1 ).Trim() );
					_com.WriteLine( _current == null ? "No file chosen" : lastresult );
				}
				else if( input.StartsWith( "w" ) )
					_com.WriteLine( _current == null ? "No file chosen" : _parser.Write( _current, input.Substring( 1 ).Replace( "{}", lastresult ).Trim() ) );
			}
		}
	}
}