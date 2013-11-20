using System.IO;
using System.Text;

namespace PokeSave.Sections
{
	public class RivalSection : GameSection
	{
		public RivalSection( Stream stream )
			: base( stream, 3848 ) { }

		public string Name
		{
			get
			{
				return TextTable.ConvertArray( Data, 0xBCC, 8 );
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine( base.ToString().TrimEnd() );
			sb.AppendLine( "\tName:\t" + Name );
			return sb.ToString();
		}
	}
}