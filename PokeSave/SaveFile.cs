using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PokeSave
{
	public class SaveFile
	{
		public SaveFile( string name )
			: this( File.OpenRead( name ) )
		{ }

		readonly byte[] _tail;

		public SaveFile( Stream inputstream )
		{
			try
			{
				A = new GameSave( inputstream );

				B = new GameSave( inputstream );

				_tail = GrabTail( inputstream );
			}
			finally
			{
				inputstream.Close();
			}
		}

		byte[] GrabTail( Stream inputstream )
		{
			var tailbuffer = new List<byte[]>();

			while( true )
			{
				var piece = new byte[16384];
				int count = inputstream.Read( piece, 0, piece.Length );
				if( count == 0 )
					break;
				if( count == piece.Length )
					tailbuffer.Add( piece );
				else
				{
					var resizedpiece = new byte[count];
					Array.Copy( piece, resizedpiece, count );
					tailbuffer.Add( resizedpiece );
				}
			}

			var tail = new byte[tailbuffer.Sum( a => a.Length )];
			var index = 0;
			foreach( var piece in tailbuffer )
			{
				Array.Copy( piece, 0, tail, index, piece.Length );
				index += piece.Length;
			}
			return tail;
		}

		public GameSave A { get; private set; }
		public GameSave B { get; private set; }

		public GameSave Latest { get { return A.SaveIndex > B.SaveIndex ? A : B; } }

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
				fs.Write( _tail, 0, _tail.Length );
			}
		}
	}
}