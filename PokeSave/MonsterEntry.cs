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
			set { _data.SetText( _offset + 8, 10, value ); }
		}

		public uint Language
		{
			get { return _data.GetShort( _offset + 18 ); }
			set { _data.SetShort( _offset + 18, value ); }
		}

		public string OriginalTrainerName
		{
			get { return _data.GetText( _offset + 20, 7 ); }
			set { _data.SetTextRaw( _offset + 20, 7, value ); }
		}

		public uint Mark
		{
			get { return _data[_offset + 27]; }
			set { _data[_offset + 27] = (byte) value; }
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
			set
			{
				if( _storage )
					_data[84] = (byte) value;
			}
		}

		public uint Virus
		{
			get { return _storage ? 0u : _data[85]; }
			set
			{
				if( _storage )
					_data[85] = (byte) value;
			}
		}

		public uint CurrentHP
		{
			get { return _storage ? 0u : _data.GetShort( 86 ); }
			set
			{
				if( _storage )
					_data.SetShort( 86, value );
			}
		}

		public uint TotalHP
		{
			get { return _storage ? 0u : _data.GetShort( 88 ); }
			set
			{
				if( _storage )
					_data.SetShort( 88, value );
			}
		}

		public uint CurrentAttack
		{
			get { return _storage ? 0u : _data.GetShort( 90 ); }
			set
			{
				if( _storage )
					_data.SetShort( 90, value );
			}
		}

		public uint CurrentDefense
		{
			get { return _storage ? 0u : _data.GetShort( 92 ); }
			set
			{
				if( _storage )
					_data.SetShort( 92, value );
			}
		}

		public uint CurrentSpeed
		{
			get { return _storage ? 0u : _data.GetShort( 94 ); }
			set
			{
				if( _storage )
					_data.SetShort( 94, value );
			}
		}

		public uint CurrentSpAttack
		{
			get { return _storage ? 0u : _data.GetShort( 96 ); }
			set
			{
				if( _storage )
					_data.SetShort( 96, value );
			}
		}

		public uint CurrentSpDefense
		{
			get { return _storage ? 0u : _data.GetShort( 98 ); }
			set
			{
				if( _storage )
					_data.SetShort( 98, value );
			}
		}

		public uint MonsterId
		{
			get { return GetEncryptedWord( GrowthOffset, true ); }
			set { SetEncryptedWord( GrowthOffset, true, (ushort) value ); }
		}

		/// <summary>
		/// this property indicates if we need to recalculate subsection checksum before save
		/// </summary>
		public bool IsDirty { get; private set; }

		public uint Item
		{
			get { return GetEncryptedWord( GrowthOffset, false ); }
			set { SetEncryptedWord( GrowthOffset, false, (ushort) value ); }
		}

		public uint XP
		{
			get { return GetEncryptedDWord( GrowthOffset + 4 ); }
			set { SetEncryptedDWord( GrowthOffset + 4, value ); }
		}

		public uint Friendship
		{
			get { return GetEncryptedByte( GrowthOffset + 8, 1 ); }
			set { SetEncryptedByte( GrowthOffset + 8, 1, (byte) value ); }
		}

		/// <summary>
		/// Making it shiny means setting personality compatible with original trainer id.
		/// </summary>
		public bool Shiny
		{
			get { return ( ( SecurityKey & 0xFFFF ) ^ ( SecurityKey >> 16 ) ) < 8; }
			set { SetPersonality( new PersonalityEngine( this ) { OriginalTrainer = value ? (uint?) OriginalTrainerId : null } ); }
		}

		/// <summary>
		/// This is set using personalityengine or Gender property
		/// </summary>
		public uint GenderByte
		{
			get { return Personality & 0xff; }
		}

		public AbilityIndex Ability
		{
			get { return ( Personality & 0x1 ) == 0x0 ? AbilityIndex.First : AbilityIndex.Second; }
			set { SetPersonality( new PersonalityEngine( this ) { Ability = value } ); }
		}

		public MonsterNature Nature
		{
			get { return (MonsterNature) ( Personality % 25 ); }
			set { SetPersonality( new PersonalityEngine( this ) { Nature = value } ); }
		}

		public EvolutionDirection Evolution
		{
			get { return ( Personality & 0xffff ) % 10 < 5 ? EvolutionDirection.S : EvolutionDirection.C; }
			set { SetPersonality( new PersonalityEngine( this ) { Evolution = value } ); }
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

		public uint Move1
		{
			get { return GetEncryptedWord( ActionOffset, true ); }
			set { SetEncryptedWord( ActionOffset, true, (byte) value ); }
		}

		public string Move1Name
		{
			get { return MoveList.Get( Move1 ); }
		}

		public uint Move2
		{
			get { return GetEncryptedWord( ActionOffset, false ); }
			set { SetEncryptedWord( ActionOffset, false, (byte) value ); }
		}

		public string Move2Name
		{
			get { return MoveList.Get( Move2 ); }
		}

		public uint Move4
		{
			get { return GetEncryptedWord( ActionOffset + 4, false ); }
			set { SetEncryptedWord( ActionOffset + 4, false, (byte) value ); }
		}

		public string Move3Name
		{
			get { return MoveList.Get( Move3 ); }
		}

		public uint Move3
		{
			get { return GetEncryptedWord( ActionOffset + 4, true ); }
			set { SetEncryptedWord( ActionOffset + 4, true, (byte) value ); }
		}

		public string Move4Name
		{
			get { return MoveList.Get( Move4 ); }
		}

		public uint PP1
		{
			get { return GetEncryptedByte( ActionOffset + 8, 0 ); }
			set { SetEncryptedByte( ActionOffset + 8, 0, (byte) value ); }
		}

		public uint PP2
		{
			get { return GetEncryptedByte( ActionOffset + 8, 1 ); }
			set { SetEncryptedByte( ActionOffset + 8, 1, (byte) value ); }
		}

		public uint PP3
		{
			get { return GetEncryptedByte( ActionOffset + 8, 2 ); }
			set { SetEncryptedByte( ActionOffset + 8, 2, (byte) value ); }
		}

		public uint PP4
		{
			get { return GetEncryptedByte( ActionOffset + 8, 3 ); }
			set { SetEncryptedByte( ActionOffset + 8, 3, (byte) value ); }
		}

		public uint PPBonus
		{
			get { return GetEncryptedByte( GrowthOffset + 8, 0 ); }
			set { SetEncryptedByte( GrowthOffset + 8, 0, (byte) value ); }
		}

		public uint PPBonus1
		{
			get { return PPBonus & 0x3; }
			set { PPBonus = PPBonus.Mask( 0x3, value ); }
		}

		public uint PPBonus2
		{
			get { return ( PPBonus & 0xc ) >> 2; }
			set { PPBonus = PPBonus.Mask( 0xc, value << 2 ); }
		}

		public uint PPBonus3
		{
			get { return ( PPBonus & 0x30 ) >> 4; }
			set { PPBonus = PPBonus.Mask( 0x30, value << 4 ); }
		}

		public uint PPBonus4
		{
			get { return ( PPBonus & 0xc0 ) >> 6; }
			set { PPBonus = PPBonus.Mask( 0xc0, value << 6 ); }
		}


		public uint HPEV
		{
			get { return GetEncryptedByte( EVsOffset, 0 ); }
			set { SetEncryptedByte( EVsOffset, 0, (byte) value ); }
		}

		public uint AttackEV
		{
			get { return GetEncryptedByte( EVsOffset, 1 ); }
			set { SetEncryptedByte( EVsOffset, 1, (byte) value ); }
		}

		public uint DefenseEV
		{
			get { return GetEncryptedByte( EVsOffset, 2 ); }
			set { SetEncryptedByte( EVsOffset, 2, (byte) value ); }
		}

		public uint SpeedEV
		{
			get { return GetEncryptedByte( EVsOffset, 3 ); }
			set { SetEncryptedByte( EVsOffset, 4, (byte) value ); }
		}

		public uint SpAttackEV
		{
			get { return GetEncryptedByte( EVsOffset + 4, 0 ); }
			set { SetEncryptedByte( EVsOffset + 4, 0, (byte) value ); }
		}

		public uint SpDefenseEV
		{
			get { return GetEncryptedByte( EVsOffset + 4, 1 ); }
			set { SetEncryptedByte( EVsOffset + 4, 1, (byte) value ); }
		}

		public uint Coolness
		{
			get { return GetEncryptedByte( EVsOffset + 4, 2 ); }
			set { SetEncryptedByte( EVsOffset + 4, 2, (byte) value ); }
		}

		public uint Beauty
		{
			get { return GetEncryptedByte( EVsOffset + 4, 3 ); }
			set { SetEncryptedByte( EVsOffset + 4, 3, (byte) value ); }
		}

		public uint Cuteness
		{
			get { return GetEncryptedByte( EVsOffset + 8, 0 ); }
			set { SetEncryptedByte( EVsOffset + 8, 0, (byte) value ); }
		}

		public uint Smartness
		{
			get { return GetEncryptedByte( EVsOffset + 8, 1 ); }
			set { SetEncryptedByte( EVsOffset + 8, 1, (byte) value ); }
		}

		public uint Toughness
		{
			get { return GetEncryptedByte( EVsOffset + 8, 2 ); }
			set { SetEncryptedByte( EVsOffset + 8, 2, (byte) value ); }
		}

		public uint Feel
		{
			get { return GetEncryptedByte( EVsOffset + 8, 3 ); }
			set { SetEncryptedByte( EVsOffset + 8, 3, (byte) value ); }
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

		public uint VirusStatus
		{
			get { return GetEncryptedByte( MiscOffset, 0 ); }
			set { SetEncryptedByte( MiscOffset, 0, (byte) value ); }
		}

		public uint MetLocation
		{
			get { return GetEncryptedByte( MiscOffset, 1 ); }
			set { SetEncryptedByte( MiscOffset, 1, (byte) value ); }
		}

		public uint OriginInfo
		{
			get { return GetEncryptedWord( MiscOffset, false ); }
			set { SetEncryptedWord( MiscOffset, false, (byte) value ); }
		}

		public uint IVs
		{
			get { return GetEncryptedDWord( MiscOffset + 4 ); }
			set { SetEncryptedDWord( MiscOffset + 4, value ); }
		}

		public uint Ribbons
		{
			get { return GetEncryptedDWord( MiscOffset + 8 ); }
			set { SetEncryptedDWord( MiscOffset + 8, value ); }
		}

		public bool Immune
		{
			get { return ( VirusStatus & 0xf0 ) == 0; }
			set { VirusStatus = VirusStatus.Mask( 0xF0, value ? 0x80u : 0u ); }
		}

		public uint VirusFade
		{
			get { return VirusStatus & 0xf; }
			set { VirusStatus = VirusStatus.Mask( 0xF, value ); }
		}

		public string OriginalTrainerGender
		{
			get { return ( OriginInfo & 0x8000 ) == 0x8000 ? "Girl" : "Boy"; }
			set { OriginInfo = OriginInfo.Mask( 0x8000, value == "Girl" ? 0x8000u : 0 ); }
		}

		public uint BallCaught
		{
			get { return OriginInfo & 0x7800 >> 11; }
			set { OriginInfo = OriginInfo.Mask( 0x7800, value << 11 ); }
		}

		public uint GameOfOrigin
		{
			get { return OriginInfo & 0x780 >> 7; }
			set { OriginInfo = OriginInfo.Mask( 0x780, value << 7 ); }
		}

		public uint LevelMet
		{
			get { return OriginInfo & 0x78; }
			set { OriginInfo = OriginInfo.Mask( 0x78, value ); }
		}

		public MonsterGender Gender
		{
			get
			{
				byte t = TypeInformation.Gender;
				if( t == 0xFF )
					return MonsterGender.None;
				if( t == 0xFE )
					return MonsterGender.F;
				if( t == 0 )
					return MonsterGender.M;
				return GenderByte >= t ? MonsterGender.M : MonsterGender.F;
			}
			set
			{
				if( TypeInformation.Gender == 0xFF || value == MonsterGender.None )
					throw new ArgumentException( "Cant set gender for genderless and vice verse" );
				if( TypeInformation.Gender == 0xFE || TypeInformation.Gender == 0 )
					throw new ArgumentException( "Cant set gender for this kind" );

				SetPersonality( new PersonalityEngine( this ) { Gender = new GenderDecision( value, TypeInformation ) } );
			}
		}

		public uint Type
		{
			get { return MonsterId; }
			set { MonsterId = value; }
		}

		public MonsterInfo TypeInformation
		{
			get { return MonsterList.Get( MonsterId ); }
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
			return _specificXor.Selector( high )( _data.GetShort( offset + ( high ? 0 : 2 ) ) ) & 0xffff;
		}

		public void SetEncryptedWord( int offset, bool high, ushort data )
		{
			IsDirty = true;
			_data.SetShort( offset + ( high ? 0 : 2 ), _specificXor.Selector( high )( data ) );
		}

		public uint GetEncryptedByte( int offset, int index )
		{
			if( index < 0 || index > 3 )
				throw new ArgumentException( "four bytes in a dword, index not in range" );
			return _specificXor.RunByte( _data[offset + index], index );
		}

		public void SetEncryptedByte( int offset, int index, byte data )
		{
			if( index < 0 || index > 3 )
				throw new ArgumentException( "four bytes in a dword, index not in range" );
			IsDirty = true;
			_data[offset + index] = (byte) _specificXor.RunByte( data, index );
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

		public string Full()
		{
			if( Empty )
				return string.Empty;
			var sb = new StringBuilder();

			sb.AppendLine( TypeInformation.Name );
			sb.AppendLine( TypeInformation.EggGroup2.ToString() );
			MonsterInfo type = TypeInformation;

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

			sb.AppendLine( "Move 1: #" + Move1 + " " + Move1Name + " PP:" + PP1 + "+" + PPBonus1 );
			sb.AppendLine( "Move 2: #" + Move2 + " " + Move2Name + " PP:" + PP2 + "+" + PPBonus2 );
			sb.AppendLine( "Move 3: #" + Move3 + " " + Move3Name + " PP:" + PP3 + "+" + PPBonus3 );
			sb.AppendLine( "Move 4: #" + Move4 + " " + Move4Name + " PP:" + PP4 + "+" + PPBonus4 );
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
			return Full();
		}
	}
}