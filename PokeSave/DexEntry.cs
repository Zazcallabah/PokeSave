using System.ComponentModel;

namespace PokeSave
{
	public class DexEntry : INotifyPropertyChanged
	{
		static readonly StringList DexNames = new StringList( "dexnames.bin" );
		readonly GameSave _save;

		public DexEntry( int index, GameSave save )
		{
			_save = save;
			Index = index;
			Name = DexNames.Get( (uint) index );
		}

		public int Index { get; private set; }

		int Offset
		{
			get { return Index / 8; }
		}

		int Bit
		{
			get { return Index % 8; }
		}

		public string Name { get; private set; }


		public bool Seen
		{
			get { return _save.Section( 0 )[0x5c + Offset].IsSet( Bit ); }

			set
			{
				if( !value && Owned )
					Owned = false;

				byte buffervalue = _save.Section( 0 )[0x5c + Offset];
				byte result = buffervalue.AssignBit( Bit, value );
				if( result != buffervalue )
				{
					_save.Section( 0 )[0x5c + Offset] = result;
					if( Offset <= 30 )
					{
						_save.Section( 1 )[_save.Pointers["DexSeenOffset1"] + Offset] = result;
						_save.Section( 4 )[_save.Pointers["DexSeenOffset2"] + Offset] = result;
					}
					InvokePropertyChanged( "Seen" );
				}
			}
		}

		public bool Owned
		{
			get { return _save.Section( 0 )[40 + Offset].IsSet( Bit ); }
			set
			{
				if( value && !Seen )
					Seen = true;
				byte buffervalue = _save.Section( 0 )[40 + Offset];
				byte result = buffervalue.AssignBit( Bit, value );
				if( result != buffervalue )
				{
					_save.Section( 0 )[40 + Offset] = result;
					InvokePropertyChanged( "Owned" );
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void InvokePropertyChanged( string e )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( e ) );
		}
	}
}