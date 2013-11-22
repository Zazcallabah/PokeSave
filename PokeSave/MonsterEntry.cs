using System;
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

		public uint Personality
		{
			get { return _data.GetInt( _offset ); }
			set { Recrypt( value, OriginalTrainerId ); }
		}

		/// <summary>
		/// When setting this value, we take some care to preserve shiny-ness.
		/// </summary>
		public uint OriginalTrainerId
		{
			get { return _data.GetInt( _offset + 4 ); }
			set { Recrypt( Shiny ? new PersonalityEngine( this ) { OriginalTrainer = value }.Generate() : Personality, value ); }
		}

		public string Name
		{
			get { return _data.GetText( _offset + 8, 10 ); }
		}

		public uint Language
		{
			get { return _data.GetShort( _offset + 18 ); }
		}

		public string OriginalTrainerName
		{
			get { return _data.GetText( _offset + 20, 7 ); }
		}

		public uint Mark
		{
			get { return _data[_offset + 27]; }
		}

		/// <summary>
		/// This should only be set right before a save.
		/// </summary>
		public uint Checksum
		{
			get { return _data.GetShort( _offset + 28 ); }
			private set { _data.SetShort( _offset + 28, value ); }
		}

		/// <summary>
		/// Xor key used to encrypt sub sections
		/// </summary>
		public uint SecurityKey
		{
			get { return _specificXor.Key; }
		}

		/// <summary>
		/// Set all statuses at the same time. Should possibly be a byte but meh.
		/// </summary>
		public uint StatusByte
		{
			get { return _storage ? (byte) 0 : (byte) ( _data.GetInt( _offset + 80 ) & 0xFF ); }
			set
			{
				if( !_storage )
					_data[_offset + 80] = (byte) ( value & 0xff );
			}
		}

		public bool Poisoned
		{
			get { return !_storage && StatusByte.IsSet( 3 ); }
			set { SetStatus( value, 3 ); }
		}

		public bool Burned
		{
			get { return !_storage && StatusByte.IsSet( 4 ); }
			set { SetStatus( value, 4 ); }
		}

		public bool Frozen
		{
			get { return !_storage && StatusByte.IsSet( 5 ); }
			set { SetStatus( value, 5 ); }
		}

		public bool Paralyzed
		{
			get { return !_storage && StatusByte.IsSet( 6 ); }
			set { SetStatus( value, 6 ); }
		}

		public bool BadPoisoned
		{
			get { return !_storage && StatusByte.IsSet( 7 ); }
			set { SetStatus( value, 7 ); }
		}

		public uint Level
		{
			get { return _storage ? 0u : _data[84]; }
		}

		public uint Virus
		{
			get { return _storage ? 0u : _data[85]; }
		}

		public uint CurrentHP
		{
			get { return _storage ? 0u : _data.GetShort( 86 ); }
		}

		public uint TotalHP
		{
			get { return _storage ? 0u : _data.GetShort( 88 ); }
		}

		public uint CurrentAttack
		{
			get { return _storage ? 0u : _data.GetShort( 90 ); }
		}

		public uint CurrentDefense
		{
			get { return _storage ? 0u : _data.GetShort( 92 ); }
		}

		public uint CurrentSpeed
		{
			get { return _storage ? 0u : _data.GetShort( 94 ); }
		}

		public uint CurrentSpAttack
		{
			get { return _storage ? 0u : _data.GetShort( 96 ); }
		}

		public uint CurrentSpDefense
		{
			get { return _storage ? 0u : _data.GetShort( 98 ); }
		}

		public uint MonsterId
		{
			get { return GetEncryptedWord( GrowthOffset, false ); }
			set { SetEncryptedWord( GrowthOffset, false, (ushort) value ); }
		}

		public bool IsDirty { get; private set; }

		public uint Item
		{
			get { return GetEncryptedWord( GrowthOffset, true ); }
			set { SetEncryptedWord( GrowthOffset, true, (ushort) value ); }
		}

		public uint XP
		{
			get { return GetEncryptedDWord( GrowthOffset + 4 ); }
			set { SetEncryptedDWord( GrowthOffset + 4, value ); }
		}

		public uint Friendship
		{
			get { return ( _specificXor.Run( _data.GetInt( GrowthOffset + 8 ) ) >> 8 ) & 0xff; }
		}

		/// <summary>
		/// Making it shiny means setting personality compatible with original trainer id.
		/// </summary>
		public bool Shiny
		{
			get { return ( ( SecurityKey & 0xFFFF ) ^ ( SecurityKey >> 16 ) ) < 8; }
			set
			{
				SetPersonality( new PersonalityEngine( this ) { OriginalTrainer = value ? (uint?) OriginalTrainerId : null } );
			}
		}

		/// <summary>
		/// This is set using personalityengine
		/// </summary>
		public uint GenderByte
		{
			get { return Personality & 0xff; }
		}

		public AbilityIndex Ability
		{
			get { return ( Personality & 0x1 ) == 0x0 ? AbilityIndex.First : AbilityIndex.Second; }
		}

		public MonsterNature Nature
		{
			get { return (MonsterNature) ( Personality % 25 ); }
		}

		public EvolutionDirection Evolution
		{
			get { return ( Personality & 0xffff ) % 10 < 5 ? EvolutionDirection.S : EvolutionDirection.C; }
		}

		public int GrowthOffset
		{
			get { return SubstructureOffset.Growth( Personality ) * 12 + 32 + _offset; }
		}

		public int ActionOffset
		{
			get { return SubstructureOffset.Action( Personality ) * 12 + 32 + _offset; }
		}

		public int EVsOffset
		{
			get { return SubstructureOffset.EVs( Personality ) * 12 + 32 + _offset; }
		}

		public int MiscOffset
		{
			get { return SubstructureOffset.Misc( Personality ) * 12 + 32 + _offset; }
		}

		public string AbilityName
		{
			get
			{
				MonsterInfo type = MonsterList.Get( MonsterId );

				if( type.Ability2 == 0 )
					return AbilityList.Get( type.Ability1 );

				return AbilityList.Get( Ability == AbilityIndex.First ? type.Ability1 : type.Ability2 );
			}
		}

		public uint CalculatedChecksum
		{
			get
			{
				uint sum = 0;
				for( int i = 0; i < 12; i++ )
				{
					uint encrypted = _data.GetInt( _offset + 32 + ( i * 4 ) );
					uint decrypted = _specificXor.Run( encrypted );

					uint high = ( decrypted >> 16 ) & 0xFFFF;
					uint low = decrypted & 0xFFFF;

					sum += high + low;
				}
				return sum & 0xffff;
			}
		}

		public uint Unknown
		{
			get { return _data.GetShort( _offset + 30 ); }
		}

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

		public uint GetEncryptedDWord( int offset )
		{
			return _specificXor.Run( _data.GetInt( offset ) );
		}

		public void SetEncryptedDWord( int offset, uint data )
		{
			IsDirty = true;
			_data.SetInt( offset, _specificXor.Run( data ) );
		}

		public uint GetEncryptedWord( int offset, bool high )
		{
			return _specificXor.Selector( high )( _data.GetShort( offset + ( high ? 2 : 0 ) ) ) & 0xffff;
		}

		public void SetEncryptedWord( int offset, bool high, ushort data )
		{
			IsDirty = true;
			_data.SetShort( offset + ( high ? 2 : 0 ), _specificXor.Selector( high )( data ) );
		}

		public uint GetEncryptedByte( int offset, int index )
		{
			if( index < 0 || index > 3 )
				throw new ArgumentException( "four bytes in a dword, index not in range" );
			var byteOrderAdjustedIndex = 3 - index;
			return _specificXor.RunByte( _data[offset + byteOrderAdjustedIndex], byteOrderAdjustedIndex );
		}

		public void SetEncryptedByte( int offset, int index, byte data )
		{
			if( index < 0 || index > 3 )
				throw new ArgumentException( "four bytes in a dword, index not in range" );
			var byteOrderAdjustedIndex = 3 - index;
			_data[offset + byteOrderAdjustedIndex] = (byte) _specificXor.RunByte( (uint) data, byteOrderAdjustedIndex );

		}

		public uint Move1
		{
			get { return _specificXor.Run( _data.GetInt( ActionOffset ) ) & 0xffff; }
		}

		public string Move1Name
		{
			get { return MoveList.Get( Move1 ); }
		}

		public uint Move2
		{
			get { return ( _specificXor.Run( _data.GetInt( ActionOffset ) ) >> 16 ) & 0xffff; }
		}

		public string Move2Name
		{
			get { return MoveList.Get( Move2 ); }
		}

		public uint Move3
		{
			get { return _specificXor.Run( _data.GetInt( ActionOffset + 4 ) ) & 0xffff; }
		}

		public string Move3Name
		{
			get { return MoveList.Get( Move3 ); }
		}

		public uint Move4
		{
			get { return ( _specificXor.Run( _data.GetInt( ActionOffset + 4 ) ) >> 16 ) & 0xffff; }
		}

		public string Move4Name
		{
			get { return MoveList.Get( Move4 ); }
		}

		public uint PP1
		{
			get { return _specificXor.Run( _data.GetInt( ActionOffset + 8 ) ) & 0xff; }
		}

		public uint PP2
		{
			get { return ( _specificXor.Run( _data.GetInt( ActionOffset + 8 ) ) >> 8 ) & 0xff; }
		}

		public uint PP3
		{
			get { return ( _specificXor.Run( _data.GetInt( ActionOffset + 8 ) ) >> 16 ) & 0xff; }
		}

		public uint PP4
		{
			get { return ( _specificXor.Run( _data.GetInt( ActionOffset + 8 ) ) >> 24 ) & 0xff; }
		}

		public uint PP1Bonus
		{
			get { return _specificXor.Run( _data.GetInt( GrowthOffset + 8 ) ) & 3; }
		}
		public uint PP2Bonus
		{
			get { return _specificXor.Run( _data.GetInt( GrowthOffset + 8 ) ) & 12; }
		}
		public uint PP3Bonus
		{
			get { return _specificXor.Run( _data.GetInt( GrowthOffset + 8 ) ) & 48; }
		}
		public uint PP4Bonus
		{
			get { return _specificXor.Run( _data.GetInt( GrowthOffset + 8 ) ) & 192; }
		}

		public uint UnownShape
		{
			get
			{
				uint p = Personality;
				return ( ( p & 0x3 ) |
					( ( p >> 8 ) & 12 ) |
						( ( p >> 16 ) & 48 ) |
							( ( p >> 24 ) & 192 ) ) % 28;
			}
		}

		public uint Sleeping
		{
			get { return StatusByte & 0x7; }
			set { StatusByte = ( StatusByte & 0xF8 ) | ( value & 0x7 ); }
		}

		public MonsterGender Gender
		{
			get;
			set;
		}

		public void SetPersonality( PersonalityEngine engine )
		{
			Personality = engine.Generate();
		}


		/// <summary>
		/// When personality or original trainer changes, subsections need to move and be re-encrypted with new key.
		/// </summary>
		void Recrypt( uint newpersonality, uint newOTid )
		{
			uint[] oldG = ReadSubSection( GrowthOffset );
			uint[] oldA = ReadSubSection( ActionOffset );
			uint[] oldE = ReadSubSection( EVsOffset );
			uint[] oldM = ReadSubSection( MiscOffset );

			_data.SetInt( _offset, newpersonality );
			_data.SetInt( _offset + 4, newOTid );
			_specificXor = new Cipher( Personality, OriginalTrainerId );

			WriteSubSection( oldG, GrowthOffset );
			WriteSubSection( oldA, ActionOffset );
			WriteSubSection( oldE, EVsOffset );
			WriteSubSection( oldM, MiscOffset );
		}

		void SetStatus( bool value, int statusbit )
		{
			if( !_storage )
				StatusByte = value ? StatusByte.SetBit( statusbit ) : StatusByte.ClearBit( statusbit );
		}

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

		public void Save()
		{
			if( IsDirty )
			{
				Checksum = CalculatedChecksum;
				IsDirty = false;
			}
		}

		public MonsterInfo Type { get { return MonsterList.Get( MonsterId ); } }

		public string Full()
		{
			if( Empty )
				return string.Empty;
			var sb = new StringBuilder();

			sb.AppendLine( Type.Name );
			sb.AppendLine( Type.EggGroup2.ToString() );
			MonsterInfo type = Type;

			if( Shiny )
				sb.AppendLine( "Shiny" );
			if( type.Type1 != type.Type2 )
				sb.Append( type.Type1 + " " );
			sb.AppendLine( type.Type2.ToString() );
			if( Name != type.Name )
				sb.AppendLine( "Name: " + Name );
			if( MonsterId >= 290 && MonsterId <= 294 )
				sb.AppendLine( "Evolution type: " + Evolution );
			if( MonsterId == 201 )
				sb.AppendLine( "Unown shape: " + UnownShape );
			sb.AppendLine( "XP: " + XP );
			sb.AppendLine( "Friendship: " + Friendship + "/" + type.BaseFriendship );

			sb.AppendLine( "Move 1: #" + Move1 + " " + Move1Name + " PP:" + PP1 + "+" + PP1Bonus );
			sb.AppendLine( "Move 2: #" + Move2 + " " + Move2Name + " PP:" + PP2 + "+" + PP2Bonus );
			sb.AppendLine( "Move 3: #" + Move3 + " " + Move3Name + " PP:" + PP3 + "+" + PP3Bonus );
			sb.AppendLine( "Move 4: #" + Move4 + " " + Move4Name + " PP:" + PP4 + "+" + PP4Bonus );
			if( type.Gender == 0xFF )
				sb.AppendLine( "Genderless" );
			else if( type.Gender == 0x00 )
				sb.AppendLine( "Always male" );
			else if( type.Gender == 0xFE )
				sb.AppendLine( "Always female" );
			else
				sb.AppendLine( GenderByte >= type.Gender ? "Male" : "Female" );

			sb.AppendLine( "Ability: " + AbilityName );

			sb.AppendLine( "Nature: " + Nature );
			sb.AppendLine( "OT ID: " + OriginalTrainerId );
			sb.AppendLine( "OT Name: " + OriginalTrainerName );
			if( Mark != 0 )
				sb.AppendLine( "Marks: " + Mark );

			if( !_storage )
			{
				if( StatusByte == 0 )
					sb.AppendLine( "No status ailment" );
				uint sleep = ( StatusByte & 7 );
				if( sleep != 0 )
					sb.AppendLine( "Sleep: " + sleep );
				if( Poisoned )
					sb.AppendLine( "Poisoned" );
				if( Burned )
					sb.AppendLine( "Burned" );
				if( Frozen )
					sb.AppendLine( "Frozen" );
				if( Paralyzed )
					sb.AppendLine( "Paralyzed" );
				if( BadPoisoned )
					sb.AppendLine( "Bad poison" );

				sb.AppendLine( "Level: " + Level );
				if( Virus != 0 )
					sb.AppendLine( "Virus remaining: " + Virus );
				sb.AppendLine( "Current HP: " + CurrentHP + "/" + TotalHP );
				sb.AppendLine( "Current A/D:" + CurrentAttack + "/" + CurrentDefense );
				sb.AppendLine( "Current Speed:" + CurrentSpeed );
				sb.AppendLine( "Current SP A/D:" + CurrentSpAttack + "/" + CurrentSpDefense );
			}
			sb.AppendLine();
			return sb.ToString();
		}

		public string Brief()
		{
			if( Empty )
				return string.Empty;
			var sb = new StringBuilder();
			if( Shiny )
				sb.Append( "shiny " );
			sb.Append( Name );
			string type = NameList.Get( MonsterId );
			if( Name != type )
				sb.Append( " (" + type + ")" );
			if( MonsterId >= 290 && MonsterId <= 294 )
				sb.Append( " " + Evolution );
			sb.Append( " xp" + XP );
			sb.Append( " f" + Friendship );
			if( !_storage )
			{
				sb.Append( " st" + StatusByte );
				sb.Append( " l" + Level );
				if( Virus != 0 )
					sb.Append( " v:" + Virus );
				sb.Append( " hp:" + CurrentHP );
				sb.Append( "/" + TotalHP );
				sb.Append( " s:" + CurrentAttack );
				sb.Append( "," + CurrentDefense );
			}
			return sb.ToString();
		}

		public override string ToString()
		{
			return Brief();
		}
	}
}