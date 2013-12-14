using System.ComponentModel;
using System.Text;

namespace PokeSave
{
	public class ItemEntry : INotifyPropertyChanged
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

		public bool Empty
		{
			get { return ID == 0 && Count == 0; }
		}

		public uint ID
		{
			get { return _data.GetShort( _offset ); }
			set
			{
				if( value != ID )
				{
					_data.SetShort( _offset, value );
					InvokePropertyChanged( "ID" );
					InvokePropertyChanged( "Name" );
					InvokePropertyChanged( "Empty" );
				}
			}
		}

		public string[] Names
		{
			get { return ItemList.All(); }
		}

		public string Name
		{
			get { return ItemList.Get( ID ); }
			set { ID = ItemList.First( value ); }
		}

		public uint Count
		{
			get
			{
				uint data = _data.GetShort( _offset + 2 );
				return _xor == null ? data : _xor.RunLower( data );
			}
			set
			{
				if( value != Count )
				{
					_data.SetShort( _offset + 2, ( _xor == null ) ? value : _xor.RunLower( value ) );
					InvokePropertyChanged( "Count" );
					InvokePropertyChanged( "Empty" );
				}
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

		public void Clear()
		{
			Count = ID = 0;
		}

		public void InvokePropertyChanged( string e )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( e ) );
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}