using System.IO;
using System.Text;
using PokeSave.Sections;

namespace PokeSave
{
	public class GameSave
	{
		readonly GameSection[] _sections;
		public GameSave( Stream instream )
		{
			_sections = new GameSection[14];
			for( int i = 0; i < _sections.Length; i++ )
			{
				_sections[i] = SectionFactory.Create( instream, i );
			}

			Cipher.Init( Trainer.SecretKey );

			Storage = new PcBuffer( _sections );
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( Trainer.ToString() );
			sb.AppendLine();
			sb.AppendLine( Team.ToString() );
			sb.AppendLine();
			sb.AppendLine( Rival.ToString() );
			sb.AppendLine();
			sb.AppendLine( Storage.ToString() );
			sb.AppendLine();
			return sb.ToString();
		}
		public TrainerSection Trainer { get { return (TrainerSection) _sections[0]; } }
		public TeamSection Team { get { return (TeamSection) _sections[1]; } }
		public RivalSection Rival { get { return (RivalSection) _sections[4]; } }
		public PcBuffer Storage { get; private set; }
	}
}