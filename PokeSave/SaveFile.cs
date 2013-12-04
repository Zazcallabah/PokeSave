using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace PokeSave
{
	public class SaveFile : INotifyPropertyChanged
	{
		readonly byte[] _tail;
		readonly string _originalFileName;

		public SaveFile( string name )
		{
			_originalFileName = name;
			var inputstream = File.OpenRead( name );

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
			Latest.PropertyChanged += ( a, e ) => InvokePropertyChanged( "Latest" );
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

		void SaveFileWithBackup( string name )
		{
			if( File.Exists( name ) )
			{
				string tmp = name;
				int i = 1;
				while( File.Exists( tmp ) )
					tmp = name + "." + ( i++ );
				File.Move( name, tmp );
			}

			using( FileStream fs = File.OpenWrite( name ) )
			{
				A.Save( fs );
				B.Save( fs );
				fs.Write( _tail, 0, _tail.Length );
			}
		}

		public void Save()
		{
			SaveFileWithBackup( _originalFileName );
		}

		public void SaveAs( string name )
		{
			SaveFileWithBackup( name );
		}


		public event PropertyChangedEventHandler PropertyChanged;

		public void InvokePropertyChanged( string e )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( e ) );
		}
	}
}