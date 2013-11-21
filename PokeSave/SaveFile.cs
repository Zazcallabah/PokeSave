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

		public GameSave Newest { get { return A.SaveIndex > B.SaveIndex ? A : B; } }

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

		public void Save( string file )
		{
			using( var fs = File.OpenWrite( file ) )
			{
				A.Save( fs );
				B.Save( fs );
			}
		}
	}
}