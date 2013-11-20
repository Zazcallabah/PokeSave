using System;
using System.Collections.Generic;
using System.Text;

namespace PokeSave.Sections
{
	public class PcBuffer
	{
		readonly byte[] _data;

		public PcBuffer( GameSection[] sections )
		{
			_data = new byte[33748];
			int readbytes = 0;
			for( int i = 5; i < 14; i++ )
			{
				var ns = (NullSection) sections[i];
				Array.Copy( ns.RawData, 0, _data, readbytes, ns.UsedLength );
				readbytes += ns.UsedLength;
			}
			Content = new List<MonsterEntry>();
			for( int i = 0; i < 420; i++ )
			{
				Content.Add( new MonsterEntry( _data, i * 80, 80 ) );
			}
		}

		protected List<MonsterEntry> Content { get; private set; }

		public int Current
		{
			get { return ByteConverter.ToInt( _data, 0 ); }
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append( "Current box: " );
			sb.Append( Current );
			sb.AppendLine();
			for( int i = 0; i < Content.Count; i++ )
			{
				if( i % 30 == 0 )
					sb.AppendLine( "Box #" + Math.Floor( i / 30.0 ) );
				var data = Content[i].ToString();
				if( !string.IsNullOrEmpty( data ) )
					sb.AppendLine( data );
			}

			return sb.ToString();

		}
	}
}