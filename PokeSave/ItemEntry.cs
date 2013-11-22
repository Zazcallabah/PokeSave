using System.Text;

namespace PokeSave
{
	public class ItemEntry
	{
		readonly GameSection _data;
		readonly int _offset;
		readonly Cipher _xor;

		public ItemEntry( GameSection data, int offset )
			: this( data, offset, null ) { }

		public ItemEntry( GameSection data, int offset, Cipher xor )
		{
			_data = data;
			_offset = offset;
			_xor = xor;
		}

		public uint ID
		{
			get { return _data.GetShort( _offset ); }
		}

		public string Name
		{
			get { return ItemList.Get( ID ); }
		}

		public uint Count
		{
			get
			{
				uint data = _data.GetShort( _offset + 2 );
				return _xor == null ? data : _xor.RunLower( data );
			}
		}

		public override string ToString()
		{
			if( ID == 0 )
				return string.Empty;
			var sb = new StringBuilder();
			sb.Append( "(" + ID + Name + ") " + Count );
			return sb.ToString();
		}
	}
}