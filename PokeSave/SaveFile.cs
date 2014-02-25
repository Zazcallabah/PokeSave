using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace PokeSave
{
	public class SaveFile : IFileContent
	{
		readonly byte[] _tail;
		public string FileName { get; private set; }

		public SaveFile( string name )
			: this( File.OpenRead( name ), name )
		{
		}

		public SaveFile( byte[] data, string name )
			: this( new MemoryStream( data ), name )
		{
		}

		public SaveFile( Stream inputstream, string name )
		{
			FileName = name;

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
			Latest.PropertyChanged += ( a, e ) =>
				{
					InvokePropertyChanged( "Latest" );
					if( e.PropertyName == "IsDirty" )
						InvokePropertyChanged( "IsDirty" );
				};
		}

		public GameSave A { get; private set; }
		public GameSave B { get; private set; }

		public GameSave Latest
		{
			get { return A.SaveIndex > B.SaveIndex ? A : B; }
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
			int index = 0;
			foreach( var piece in tailbuffer )
			{
				Array.Copy( piece, 0, tail, index, piece.Length );
				index += piece.Length;
			}
			return tail;
		}

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

		public void Save()
		{
			if( File.Exists( FileName ) )
			{
				string tmp = FileName;
				int i = 1;
				while( File.Exists( tmp ) )
					tmp = FileName + "." + ( i++ );
				File.Move( FileName, tmp );
			}
			Save( FileName );
		}

		public void Save( string path )
		{
			using( FileStream fs = File.OpenWrite( path ) )
			{
				A.Save( fs );
				B.Save( fs );
				fs.Write( _tail, 0, _tail.Length );
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		public void InvokePropertyChanged( string e )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( e ) );
		}

		public bool IsDirty { get { return Latest.IsDirty; } }
	}
}