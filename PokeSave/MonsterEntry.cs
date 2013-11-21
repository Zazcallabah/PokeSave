using System.Text;

namespace PokeSave
{
	public class MonsterEntry
	{
		readonly GameSection _data;
		readonly int _offset;
		readonly bool _storage;
		Cipher _specificXor;


		public MonsterEntry( GameSection data, int offset, bool storage )
		{
			_data = data;
			_offset = offset;
			_storage = storage;
			_specificXor = new Cipher( Personality, OriginalTrainerId );
		}

		public void SetPersonality( PersonalityEngine engine )
		{
			Personality = engine.Generate();
		}

		void Recrypt( uint newpersonality, uint newOTid )
		{
			var oldG = ReadSubSection( GrowthOffset );
			var oldA = ReadSubSection( ActionOffset );
			var oldE = ReadSubSection( EVsOffset );
			var oldM = ReadSubSection( MiscOffset );

			_data.SetInt( _offset, newpersonality );
			_data.SetInt( _offset + 4, newOTid );
			_specificXor = new Cipher( Personality, OriginalTrainerId );

			WriteSubSection( oldG, GrowthOffset );
			WriteSubSection( oldA, ActionOffset );
			WriteSubSection( oldE, EVsOffset );
			WriteSubSection( oldM, MiscOffset );
		}

		public uint Personality
		{
			get { return _data.GetInt( _offset ); }
			set { Recrypt( value, OriginalTrainerId ); }
		}

		public uint OriginalTrainerId
		{
			get { return _data.GetInt( _offset + 4 ); }
			set { Recrypt( Personality, value ); }
		}

		public string Name { get { return _data.GetText( _offset + 8, 10 ); } }
		public uint Language { get { return _data.GetShort( _offset + 18 ); } }
		public string OriginalTrainerName { get { return _data.GetText( _offset + 20, 7 ); } }
		public uint Mark { get { return _data[_offset + 27]; } }
		public uint Checksum { get { return _data.GetShort( _offset + 28 ); } }
		public uint SecurityKey { get { return _specificXor.Key; } }

		public uint Status
		{
			get { return _storage ? 0 : _data.GetInt( _offset + 80 ); }
		}

		public uint Level
		{
			get { return _storage ? 0U : _data[84]; }
		}

		public uint Virus
		{
			get { return _storage ? 0U : _data[85]; }
		}

		public uint CurrentHP
		{
			get { return _storage ? 0U : _data.GetShort( 86 ); }
		}

		public uint TotalHP
		{
			get { return _storage ? 0U : _data.GetShort( 88 ); }
		}

		public uint CurrentAttack
		{
			get { return _storage ? 0U : _data.GetShort( 90 ); }
		}
		public uint CurrentDefense
		{
			get { return _storage ? 0U : _data.GetShort( 92 ); }
		}

		public uint CurrentSpeed
		{
			get { return _storage ? 0U : _data.GetShort( 94 ); }
		}

		public uint CurrentSpAttack
		{
			get { return _storage ? 0U : _data.GetShort( 96 ); }
		}

		public uint CurrentSpDefense
		{
			get { return _storage ? 0U : _data.GetShort( 98 ); }
		}


		public uint MonsterId { get { return _specificXor.Run( _data.GetShort( GrowthOffset ) ) & 0xffff; } }
		public uint Item { get { return _specificXor.RunLower( _data.GetShort( GrowthOffset ) ) >> 16; } }
		public uint XP { get { return _specificXor.Run( _data.GetInt( GrowthOffset + 4 ) ); } }
		public uint PP { get { return _specificXor.Run( _data.GetInt( GrowthOffset + 8 ) ) & 0xff; } }
		public uint Friendship { get { return ( _specificXor.Run( _data.GetInt( GrowthOffset + 8 ) ) >> 8 ) & 0xff; } }

		uint[] ReadSubSection( int offset )
		{
			var r = new uint[3];
			r[0] = _specificXor.Run( _data.GetInt( offset ) );
			r[1] = _specificXor.Run( _data.GetInt( offset + 4 ) );
			r[2] = _specificXor.Run( _data.GetInt( offset + 8 ) );
			return r;
		}

		void WriteSubSection( uint[] data, int offset )
		{
			_data.SetInt( offset, _specificXor.Run( data[0] ) );
			_data.SetInt( offset + 4, _specificXor.Run( data[1] ) );
			_data.SetInt( offset + 8, _specificXor.Run( data[2] ) );
		}

		public bool Shiny
		{
			get
			{
				return ( ( SecurityKey & 0xFFFF ) ^ ( SecurityKey >> 16 ) ) < 8;
			}
		}
		public uint GenderByte { get { return Personality & 0xff; } }
		public Ability Ability { get { return ( Personality & 0x1 ) == 0x0 ? Ability.First : Ability.Second; } }
		public uint Nature { get { return Personality % 25; } }
		public EvolutionDirection Evolution { get { return ( Personality & 0xffff ) % 10 < 5 ? EvolutionDirection.S : EvolutionDirection.C; } }

		public int GrowthOffset { get { return SubstructureOffset.Growth( Personality ) * 12 + 32 + _offset; } }
		public int ActionOffset { get { return SubstructureOffset.Action( Personality ) * 12 + 32 + _offset; } }
		public int EVsOffset { get { return SubstructureOffset.EVs( Personality ) * 12 + 32 + _offset; } }
		public int MiscOffset { get { return SubstructureOffset.Misc( Personality ) * 12 + 32 + _offset; } }

		public uint CalculatedChecksum
		{
			get
			{
				uint sum = 0;
				for( int i = 0; i < 12; i++ )
				{
					var encrypted = _data.GetInt( _offset + 32 + ( i * 4 ) );
					var decrypted = _specificXor.Run( encrypted );

					var high = ( decrypted >> 16 ) & 0xFFFF;
					var low = decrypted & 0xFFFF;

					sum += high + low;
				}
				return sum & 0xffff;
			}
		}

		public uint Unknown { get { return _data.GetShort( _offset + 30 ); } }

		public bool Empty
		{
			get
			{
				for( int i = _offset; i < _offset + ( _storage ? 80 : 100 ); i++ )
					if( _data[i] != 0 )
						return false;
				return true;
			}
		}

		public override string ToString()
		{
			if( Empty )
				return string.Empty;
			var sb = new StringBuilder();
			sb.Append( "\t" );
			if( Shiny )
				sb.Append( "shiny " );
			sb.Append( Name );
			var type = MonsterList.Get( MonsterId );
			if( Name != type )
				sb.Append( " (" + type + ")" );
			if( MonsterId >= 290 && MonsterId <= 294 )
				sb.Append( " " + Evolution );
			sb.Append( " xp" + XP );
			sb.Append( " pp" + PP );
			sb.Append( " f" + Friendship );
			if( !_storage )
			{
				sb.Append( " st" + Status );
				sb.Append( " l" + Level );
				if( Virus != 0 )
					sb.Append( " v:" + Virus );
				sb.Append( " hp:" + CurrentHP );
				sb.Append( "/" + TotalHP );
				sb.Append( " s:" + CurrentAttack );
				sb.Append( "," + CurrentDefense );
				sb.Append( "," + CurrentSpeed );
				sb.Append( "," + CurrentSpAttack );
				sb.Append( "," + CurrentSpDefense );
			}
			sb.AppendLine();
			return sb.ToString();
		}
	}
}