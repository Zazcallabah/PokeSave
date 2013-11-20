using System.Text;

namespace PokeSave
{
	public class ItemEntry
	{
		readonly byte[] _data;
		readonly int _offset;
		readonly bool _pc;

		public ItemEntry( byte[] data, int offset, bool pc )
		{
			_data = data;
			_offset = offset;
			_pc = pc;
		}

		public int ID
		{
			get
			{
				return ByteConverter.ToShort( _data, _offset );
			}
		}

		public string Name
		{
			get
			{
				return ItemList.Get( ID );
			}
		}

		public int CountEncrypted
		{
			get
			{
				return ByteConverter.ToShort( _data, _offset + 2 );
			}
		}

		public int Count
		{
			get
			{
				if( _pc )
					return CountEncrypted;
				return Cipher.Run( CountEncrypted );
			}
		}

		public override string ToString()
		{
			if( ID == 0 )
				return string.Empty;
			var sb = new StringBuilder();
			sb.Append( "(" + ID + Name + ") " + CountEncrypted );
			return sb.ToString();
		}
	}
}