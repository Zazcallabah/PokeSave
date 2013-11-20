using System.IO;
using System.Text;

namespace PokeSave
{
	public class SaveFile
	{
		public SaveFile( string name )
			: this( File.OpenRead( name ) )
		{ }

		public SaveFile( Stream inputstream )
		{
			try
			{
				A = new GameSave( inputstream );

				B = new GameSave( inputstream );

				var tail = new byte[16385];
				var ended = inputstream.Read( tail, 0, tail.Length );
				/*			if( ended < 16384 )
								throw new ArgumentException( "file too short" );
							if( ended > 16384 )
								throw new ArgumentException( "file too long" );
				*/
			}
			finally
			{
				inputstream.Close();
			}
		}

		public GameSave A { get; private set; }
		public GameSave B { get; private set; }


		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( "Save file A" );
			sb.AppendLine( A.ToString() );
			sb.AppendLine();
			sb.AppendLine();
			sb.AppendLine( "Save file B" );
			sb.AppendLine( B.ToString() );
			return sb.ToString();
		}

	}
}