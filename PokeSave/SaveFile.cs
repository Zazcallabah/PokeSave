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