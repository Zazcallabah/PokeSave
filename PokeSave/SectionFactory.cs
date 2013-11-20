using System.IO;
using PokeSave.Sections;

namespace PokeSave
{
	public static class SectionFactory
	{
		public static GameSection Create( Stream instream, int section )
		{
			switch( (SectionId) section )
			{
				case SectionId.Trainer:
					return new TrainerSection( instream );
				case SectionId.Team:
					return new TeamSection( instream );
				case SectionId.Unknown1:
					return new NullSection( instream, 3968 );
				case SectionId.Unknown2:
					return new NullSection( instream, 3968 );
				case SectionId.Rival:
					return new RivalSection( instream );
				case SectionId.PCBufferA:
					return new NullSection( instream, 3968 );
				case SectionId.PCBufferB:
					return new NullSection( instream, 3968 );
				case SectionId.PCBufferC:
					return new NullSection( instream, 3968 );
				case SectionId.PCBufferD:
					return new NullSection( instream, 3968 );
				case SectionId.PCBufferE:
					return new NullSection( instream, 3968 );
				case SectionId.PCBufferF:
					return new NullSection( instream, 3968 );
				case SectionId.PCBufferG:
					return new NullSection( instream, 3968 );
				case SectionId.PCBufferH:
					return new NullSection( instream, 3968 );
				case SectionId.PCBufferI:
					return new NullSection( instream, 2000 );
			}
			return null;
		}
	}
}