using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace PokeSave
{
	public class MonsterEntry : INotifyPropertyChanged
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
			set
			{
				if( Personality != value )
				{
					Recrypt( value, OriginalTrainerId );
					InvokePropertyChanged( "Personality" );
					InvokePropertyChanged( "Shiny" );
					InvokePropertyChanged( "Gender" );
					InvokePropertyChanged( "GenderByte" );
					InvokePropertyChanged( "Ability" );
					InvokePropertyChanged( "AbilityName" );
					InvokePropertyChanged( "Nature" );
					InvokePropertyChanged( "Evolution" );
				}
			}
		}

		/// <summary>
		/// When setting this value, we take some care to preserve shiny-ness.
		/// </summary>
		public uint OriginalTrainerId
		{
			get { return _data.GetInt( _offset + 4 ); }
			set
			{
				if( OriginalTrainerId != value )
				{
					if( Shiny )
					{
						Recrypt( new PersonalityEngine( this ) { OriginalTrainer = value }.Generate(), value );
						InvokePropertyChanged( "Personality" );
					}
					else
					{
						Recrypt( Personality, value );
					}
					InvokePropertyChanged( "OriginalTrainerId" );
				}
			}
		}

		public string Name
		{
			get { return _data.GetText( _offset + 8, 10 ); }
			set
			{
				if( Name != value )
				{
					_data.SetText( _offset + 8, 10, value );
					InvokePropertyChanged( "Name" );
				}
			}
		}

		public uint Language
		{
			get { return _data.GetShort( _offset + 18 ); }
			set
			{
				if( Language != value )
				{
					_data.SetShort( _offset + 18, value );
					InvokePropertyChanged( "Language" );
				}
			}
		}

		public string OriginalTrainerName
		{
			get { return _data.GetText( _offset + 20, 7 ); }
			set
			{
				if( OriginalTrainerName != value )
				{
					_data.SetTextRaw( _offset + 20, 7, value );
					InvokePropertyChanged( "OriginalTrainerName" );
				}
			}
		}

		public uint Mark
		{
			get { return _data[_offset + 27]; }
			set
			{
				if( Mark != value )
				{
					_data[_offset + 27] = (byte) value;
					InvokePropertyChanged( "Mark" );
				}
			}
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
				if( !_storage && StatusByte != value )
				{
					_data[_offset + 80] = (byte) ( value & 0xff );
					InvokePropertyChanged( "StatusByte" );
					InvokePropertyChanged( "Poisoned" );
					InvokePropertyChanged( "Burned" );
					InvokePropertyChanged( "Frozen" );
					InvokePropertyChanged( "Paralyzed" );
					InvokePropertyChanged( "BadPoisoned" );
					InvokePropertyChanged( "Sleeping" );
				}
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
			get { return _storage ? 0u : _data[_offset + 84]; }
			set
			{
				if( !_storage && Level != value )
				{
					_data[_offset + 84] = (byte) value;
					InvokePropertyChanged( "Level" );
				}
			}
		}

		public uint Virus
		{
			get { return _storage ? 0u : _data[_offset + 85]; }
			set
			{
				if( !_storage && Virus != value )
				{
					_data[_offset + 85] = (byte) value;
					InvokePropertyChanged( "Virus" );
				}
			}
		}

		public uint CurrentHP
		{
			get { return _storage ? 0u : _data.GetShort( _offset + 86 ); }
			set
			{
				if( !_storage && CurrentHP != value )
				{
					_data.SetShort( _offset + 86, value );
					InvokePropertyChanged( "CurrentHP" );
				}
			}
		}

		public uint TotalHP
		{
			get { return _storage ? 0u : _data.GetShort( _offset + 88 ); }
			set
			{
				if( !_storage && TotalHP != value )
				{
					_data.SetShort( _offset + 88, value );
					InvokePropertyChanged( "TotalHP" );
				}
			}
		}

		public uint CurrentAttack
		{
			get { return _storage ? 0u : _data.GetShort( _offset + 90 ); }
			set
			{
				if( !_storage && CurrentAttack != value )
				{
					_data.SetShort( _offset + 90, value );
					InvokePropertyChanged( "CurrentAttack" );
				}
			}
		}

		public uint CurrentDefense
		{
			get { return _storage ? 0u : _data.GetShort( _offset + 92 ); }
			set
			{
				if( !_storage && CurrentDefense != value )
				{
					_data.SetShort( _offset + 92, value );
					InvokePropertyChanged( "CurrentDefense" );
				}
			}
		}

		public uint CurrentSpeed
		{
			get { return _storage ? 0u : _data.GetShort( _offset + 94 ); }
			set
			{
				if( !_storage && CurrentSpeed != value )
				{
					_data.SetShort( _offset + 94, value );
					InvokePropertyChanged( "CurrentSpeed" );
				}
			}
		}

		public uint CurrentSpAttack
		{
			get { return _storage ? 0u : _data.GetShort( _offset + 96 ); }
			set
			{
				if( !_storage && CurrentSpAttack != value )
				{
					_data.SetShort( _offset + 96, value );
					InvokePropertyChanged( "CurrentSpAttack" );
				}
			}
		}

		public uint CurrentSpDefense
		{
			get { return _storage ? 0u : _data.GetShort( _offset + 98 ); }
			set
			{
				if( !_storage && CurrentSpDefense != value )
				{
					_data.SetShort( _offset + 98, value );
					InvokePropertyChanged( "CurrentSpDefense" );
				}
			}
		}

		public uint MonsterId
		{
			get { return GetEncryptedWord( GrowthOffset, true ); }
			set
			{
				if( MonsterId != value )
				{
					SetEncryptedWord( GrowthOffset, true, (ushort) value );
					InvokePropertyChanged( "Empty" );
					InvokePropertyChanged( "MonsterId" );
					InvokePropertyChanged( "Type" );
					InvokePropertyChanged( "TypeInformation" );
				}
			}
		}

		/// <summary>
		/// this property indicates if we need to recalculate subsection checksum before save
		/// </summary>
		public bool IsDirty { get; private set; }

		public uint Item
		{
			get { return GetEncryptedWord( GrowthOffset, false ); }
			set
			{
				if( Item != value )
				{
					SetEncryptedWord( GrowthOffset, false, (ushort) value );
					InvokePropertyChanged( "Item" );
				}
			}
		}

		public uint XP
		{
			get { return GetEncryptedDWord( GrowthOffset + 4 ); }
			set
			{
				if( XP != value )
				{
					SetEncryptedDWord( GrowthOffset + 4, value );
					InvokePropertyChanged( "XP" );
				}
			}
		}

		public uint Friendship
		{
			get { return GetEncryptedByte( GrowthOffset + 8, 1 ); }
			set
			{
				if( Friendship != value )
				{
					SetEncryptedByte( GrowthOffset + 8, 1, (byte) value );
					InvokePropertyChanged( "Friendship" );
				}
			}
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
			set
			{
				if( Move1 != value )
				{
					SetEncryptedWord( ActionOffset, true, (byte) value );
					InvokePropertyChanged( "Move1" );
					InvokePropertyChanged( "Move1Name" );
				}
			}
		}

		public string Move1Name
		{
			get { return MoveList.Get( Move1 ); }
		}

		public uint Move2
		{
			get { return GetEncryptedWord( ActionOffset, false ); }
			set
			{
				if( Move2 != value )
				{
					SetEncryptedWord( ActionOffset, false, (byte) value );
					InvokePropertyChanged( "Move2" );
					InvokePropertyChanged( "Move2Name" );
				}
			}
		}

		public string Move2Name
		{
			get { return MoveList.Get( Move2 ); }
		}

		public uint Move3
		{
			get { return GetEncryptedWord( ActionOffset + 4, true ); }
			set
			{

				if( Move3 != value )
				{
					SetEncryptedWord( ActionOffset + 4, true, (byte) value );
					InvokePropertyChanged( "Move3" );
					InvokePropertyChanged( "Move3Name" );
				}
			}
		}
		public string Move3Name
		{
			get { return MoveList.Get( Move3 ); }
		}

		public uint Move4
		{
			get { return GetEncryptedWord( ActionOffset + 4, false ); }
			set
			{

				if( Move4 != value )
				{
					SetEncryptedWord( ActionOffset + 4, false, (byte) value );
					InvokePropertyChanged( "Move4" );
					InvokePropertyChanged( "Move4Name" );
				}
			}
		}

		public string Move4Name
		{
			get { return MoveList.Get( Move4 ); }
		}

		public uint PP1
		{
			get { return GetEncryptedByte( ActionOffset + 8, 0 ); }
			set
			{
				if( PP1 != value )
				{
					SetEncryptedByte( ActionOffset + 8, 0, (byte) value );
					InvokePropertyChanged( "PP1" );
				}
			}
		}

		public uint PP2
		{
			get { return GetEncryptedByte( ActionOffset + 8, 1 ); }
			set
			{
				if( PP2 != value )
				{
					SetEncryptedByte( ActionOffset + 8, 1, (byte) value );
					InvokePropertyChanged( "PP2" );
				}
			}
		}

		public uint PP3
		{
			get { return GetEncryptedByte( ActionOffset + 8, 2 ); }
			set
			{
				if( PP3 != value )
				{
					SetEncryptedByte( ActionOffset + 8, 2, (byte) value );
					InvokePropertyChanged( "PP3" );
				}
			}
		}

		public uint PP4
		{
			get { return GetEncryptedByte( ActionOffset + 8, 3 ); }
			set
			{
				if( PP4 != value )
				{
					SetEncryptedByte( ActionOffset + 8, 3, (byte) value );
					InvokePropertyChanged( "PP4" );
				}
			}
		}

		public uint PPBonus
		{
			get { return GetEncryptedByte( GrowthOffset + 8, 0 ); }
			set
			{
				if( PPBonus != value )
				{
					SetEncryptedByte( GrowthOffset + 8, 0, (byte) value );
					InvokePropertyChanged( "PPBonus" );
					InvokePropertyChanged( "PPBonus1" );
					InvokePropertyChanged( "PPBonus2" );
					InvokePropertyChanged( "PPBonus3" );
					InvokePropertyChanged( "PPBonus4" );
				}
			}
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
			set
			{
				if( HPEV != value )
				{
					SetEncryptedByte( EVsOffset, 0, (byte) value );
					InvokePropertyChanged( "HPEV" );
				}
			}
		}

		public uint AttackEV
		{
			get { return GetEncryptedByte( EVsOffset, 1 ); }
			set
			{
				if( AttackEV != value )
				{
					SetEncryptedByte( EVsOffset, 1, (byte) value );
					InvokePropertyChanged( "AttackEV" );
				}
			}
		}

		public uint DefenseEV
		{
			get { return GetEncryptedByte( EVsOffset, 2 ); }
			set
			{
				if( DefenseEV != value )
				{
					SetEncryptedByte( EVsOffset, 2, (byte) value );
					InvokePropertyChanged( "DefenseEV" );
				}
			}
		}

		public uint SpeedEV
		{
			get { return GetEncryptedByte( EVsOffset, 3 ); }
			set
			{
				if( SpeedEV != value )
				{
					SetEncryptedByte( EVsOffset, 3, (byte) value );
					InvokePropertyChanged( "SpeedEV" );
				}
			}
		}

		public uint SpAttackEV
		{
			get { return GetEncryptedByte( EVsOffset + 4, 0 ); }
			set
			{
				if( SpAttackEV != value )
				{
					SetEncryptedByte( EVsOffset + 4, 0, (byte) value );
					InvokePropertyChanged( "SpAttackEV" );
				}
			}
		}

		public uint SpDefenseEV
		{
			get { return GetEncryptedByte( EVsOffset + 4, 1 ); }
			set
			{
				if( SpDefenseEV != value )
				{
					SetEncryptedByte( EVsOffset + 4, 1, (byte) value );
					InvokePropertyChanged( "SpDefenseEV" );
				}
			}
		}

		public uint Coolness
		{
			get { return GetEncryptedByte( EVsOffset + 4, 2 ); }
			set
			{
				if( Coolness != value )
				{
					SetEncryptedByte( EVsOffset + 4, 2, (byte) value );
					InvokePropertyChanged( "Coolness" );
				}
			}
		}

		public uint Beauty
		{
			get { return GetEncryptedByte( EVsOffset + 4, 3 ); }
			set
			{
				if( Beauty != value )
				{
					SetEncryptedByte( EVsOffset + 4, 3, (byte) value );
					InvokePropertyChanged( "Beauty" );
				}
			}
		}

		public uint Cuteness
		{
			get { return GetEncryptedByte( EVsOffset + 8, 0 ); }
			set
			{
				if( Cuteness != value )
				{
					SetEncryptedByte( EVsOffset + 8, 0, (byte) value );
					InvokePropertyChanged( "Cuteness" );
				}
			}
		}

		public uint Smartness
		{
			get { return GetEncryptedByte( EVsOffset + 8, 1 ); }
			set
			{
				if( Smartness != value )
				{
					SetEncryptedByte( EVsOffset + 8, 1, (byte) value );
					InvokePropertyChanged( "Smartness" );
				}
			}
		}

		public uint Toughness
		{
			get { return GetEncryptedByte( EVsOffset + 8, 2 ); }
			set
			{
				if( Toughness != value )
				{
					SetEncryptedByte( EVsOffset + 8, 2, (byte) value );
					InvokePropertyChanged( "Toughness" );
				}
			}
		}

		public uint Feel
		{
			get { return GetEncryptedByte( EVsOffset + 8, 3 ); }
			set
			{
				if( Feel != value )
				{
					SetEncryptedByte( EVsOffset + 8, 3, (byte) value );
					InvokePropertyChanged( "Feel" );
				}
			}
		}

		public uint UnownShape
		{
			get
			{
				uint p = Personality;
				return ( ( p & 3 ) |
					( ( p >> 6 ) & 12 ) |
						( ( p >> 12 ) & 48 ) |
							( ( p >> 18 ) & 192 ) ) % 28;
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
			set
			{
				if( VirusStatus != value )
				{
					SetEncryptedByte( MiscOffset, 0, (byte) value );
					InvokePropertyChanged( "VirusStatus" );
					InvokePropertyChanged( "Immune" );
					InvokePropertyChanged( "VirusStrain" );
					InvokePropertyChanged( "VirusFade" );
				}
			}
		}

		public uint MetLocation
		{
			get { return GetEncryptedByte( MiscOffset, 1 ); }
			set
			{
				if( MetLocation != value )
				{
					SetEncryptedByte( MiscOffset, 1, (byte) value );
					InvokePropertyChanged( "MetLocation" );
				}
			}
		}

		public uint OriginInfo
		{
			get { return GetEncryptedWord( MiscOffset, false ); }
			set
			{
				if( OriginInfo != value )
				{
					SetEncryptedWord( MiscOffset, false, (ushort) value );
					InvokePropertyChanged( "OriginInfo" );
					InvokePropertyChanged( "OriginalTrainerGender" );
					InvokePropertyChanged( "BallCaught" );
					InvokePropertyChanged( "GameOfOrigin" );
					InvokePropertyChanged( "LevelMet" );
				}
			}
		}

		public uint IVs
		{
			get { return GetEncryptedDWord( MiscOffset + 4 ); }
			set
			{
				if( IVs != value )
				{
					SetEncryptedDWord( MiscOffset + 4, value );
					InvokePropertyChanged( "IVs" );
				}
			}
		}

		public uint Ribbons
		{
			get { return GetEncryptedDWord( MiscOffset + 8 ); }
			set
			{
				if( Ribbons != value )
				{
					SetEncryptedDWord( MiscOffset + 8, value );
					InvokePropertyChanged( "Ribbons" );
				}
			}
		}

		/// <summary>
		/// 0 is possible but illegitimate
		/// 1-15 valid
		/// </summary>
		public void AssignVirusStrain( byte strain )
		{
			VirusStrain = strain;
			VirusFade = (uint) ( strain % 4 ) + 1;
		}

		/// <summary>
		/// Immune if already has virus
		/// </summary>
		public bool Immune
		{
			get { return VirusStrain != 0; }
			set { VirusStrain = value ? 7u : 0; }
		}

		/// <summary>
		/// 0 is no virus, 1-15 is valid
		/// </summary>
		public uint VirusStrain
		{
			get { return ( VirusStatus & 0xf0 ) >> 4; }
			set { VirusStatus = VirusStatus.Mask( 0xF0, value << 4 ); }
		}

		/// <summary>
		/// Decreases 1 every midnight, valid values are 0-4, initial is strain mod 4 + 1
		/// </summary>
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
			get { return ( OriginInfo & 0x7800 ) >> 11; }
			set { OriginInfo = OriginInfo.Mask( 0x7800, value << 11 ); }
		}

		public uint GameOfOrigin
		{
			get { return ( OriginInfo & 0x780 ) >> 7; }
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

		/// <summary>
		/// Writing to team from pc buffer requires you to deposit and withdraw from pc ingame to get correct values.
		/// </summary>
		// trigger empty
		public byte[] RawData
		{
			get
			{
				var arr = new byte[_storage ? 80 : 100];
				for( var i = 0; i < arr.Length; i++ )
				{
					arr[i] = _data[_offset + i];
				}
				return arr;
			}

			set
			{
				if( value.Length != 80 && value.Length != 100 )
					throw new ArgumentException( "invalid array length" );
				for( var i = 0; i < ( _storage ? 80 : 100 ); i++ )
				{
					if( i >= value.Length )
						_data[_offset + i] = 0;
					else
						_data[_offset + i] = value[i];
				}

				if( !_storage && value.Length == 80 )
				{
					Level = 1;
					CurrentHP = TotalHP = HPEV;
					CurrentAttack = AttackEV;
					CurrentDefense = DefenseEV;
					CurrentSpAttack = SpAttackEV;
					CurrentSpDefense = SpDefenseEV;
					CurrentSpeed = SpeedEV;
				}

				foreach( var prop in GetType().GetProperties() )
				{
					InvokePropertyChanged( prop.Name );
					Debug.WriteLine( prop.Name );
				}
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
			sb.Append( " o:" + OriginInfo.ToString( "X" ) );
			return sb.ToString();
		}

		public override string ToString()
		{
			return Full();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void InvokePropertyChanged( string e )
		{
			if( PropertyChanged != null )
				PropertyChanged( this, new PropertyChangedEventArgs( e ) );
		}

		public void MakeOwn( GameSave save )
		{
			OriginalTrainerId = save.TrainerId;
			OriginalTrainerGender = save.Gender;
			OriginalTrainerName = save.Name;
			GameOfOrigin = save.GameCode; // NOT CORRECT
		}

		public void Clear()
		{
			RawData = new byte[100];
		}
	}
}