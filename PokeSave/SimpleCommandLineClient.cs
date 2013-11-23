using System.IO;

namespace PokeSave
{
	public class SimpleCommandLineClient
	{
		readonly IComms _com;
		SaveFile _current;
		public SimpleCommandLineClient( IComms com )
		{
			_com = com;
			_current = null;
		}

		public void Run( string[] args )
		{

			while( true )
			{
				_com.Write( "\nld, st, r, w, p\n> " );
				string input = _com.ReadLine();
				if( input == "q" )
					return;
				if( input.StartsWith( "ld" ) )
				{
					var name = input.Substring( 2 ).Trim();
					if( !File.Exists( name ) )
						_com.WriteLine( "No such file" );
					else
						_current = new SaveFile( name );
				}
				else if( input.StartsWith( "st" ) )
				{
					if( _current == null )
						_com.WriteLine( "No file loaded" );
					else
					{
						var name = input.Substring( 2 ).Trim();
						if( File.Exists( name ) )
							_com.WriteLine( "File already exists" );
						else
							_current.Save( name );
					}
				}
				else if( input.StartsWith( "p" ) )
					_com.WriteLine( _current.ToString() );
			}
		}
	}
}