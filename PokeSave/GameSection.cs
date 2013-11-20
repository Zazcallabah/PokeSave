using System;
using System.IO;
using System.Text;

namespace PokeSave
{
	public class GameSection
	{
		#region Lookuptables
		static readonly string[] Names = new[]{
			"Trainer",
			"Team",
			"Unknown 1",
			"Unknown 2",
			"Rival",
			"PC Buffer A",
			"PC Buffer B",
			"PC Buffer C",
			"PC Buffer D",
			"PC Buffer E",
			"PC Buffer F",
			"PC Buffer G",
			"PC Buffer H",
			"PC Buffer I"
		};

		static readonly int[] Sizes = new[]{
			3884,
			3968,
			3968,
			3968,
			3848,
			3968,
			3968,
			3968,
			3968,
			3968,
			3968,
			3968,
			3968,
			2000
		};
		#endregion

		readonly byte[] _datasource;

		public string GetText( int offset, int count )
		{
			return TextTable.ConvertArray( this, offset, count );
		}

		public string GetTextRaw( int offset, int count )
		{
			return TextTable.ConvertArrayRaw( this, offset, count );
		}

		public virtual uint GetByte( int offset )
		{
			return _datasource[offset];
		}

		public uint GetInt( int offset )
		{
			return ( GetByte( offset + 3 ) << 24 )
	| ( GetByte( offset + 2 ) << 16 )
		| ( GetByte( offset + 1 ) << 8 )
			| GetByte( offset );
		}

		public uint GetShort( int offset )
		{
			return ( GetByte( offset + 1 ) << 8 ) | GetByte( offset );
		}

		public virtual void Set( int offset, byte data )
		{
			// Todo calculate checksum
			throw new NotImplementedException();
		}

		protected GameSection()
		{
		}

		public GameSection( Stream instream )
		{
			_datasource = new byte[4 * 1024];
			var count = instream.Read( _datasource, 0, _datasource.Length );
			if( count != _datasource.Length )
				throw new ArgumentException( "file too short" );
		}

		public int Length { get { return Sizes[ID]; } }
		public string Name { get { return Names[ID]; } }
		public uint Checksum { get { return GetShort( 0xff6 ); } }
		public uint ID { get { return GetShort( 0xff4 ); } }
		public uint SaveIndex { get { return GetShort( 0xffc ); } }

		public uint CalculatedChecksum
		{
			get
			{
				uint chk = 0;
				for( int i = 0; i < Length; i += 4 )
				{
					chk += GetInt( i );
				}

				uint upper = ( chk >> 16 ) & 0xFFFF;
				uint lower = chk & 0xFFFF;
				return ( upper + lower ) & 0xFFFF;
			}
		}

		// Todo: Write-method, Save-method

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( "     Name: " + Name );
			sb.AppendLine( "       ID: " + ID );
			sb.AppendLine( " Checksum: " + Checksum );
			sb.AppendLine( "Checksum2: " + CalculatedChecksum );
			sb.AppendLine( "Saveindex: " + SaveIndex );
			sb.AppendLine( "   Length: " + Length );
			return sb.ToString();
		}
	}
}