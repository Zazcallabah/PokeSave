using System.IO;

namespace PokeSave.Sections
{
	public class NullSection : GameSection
	{
		public NullSection( Stream stream, int length )
			: base( stream, length ) { }

		public byte[] RawData { get { return Data; } }
	}
}