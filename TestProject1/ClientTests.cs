using System.IO;
using NUnit.Framework;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class ClientTests
	{
		SimpleCommandLineClient _c;
		TestComms _com;

		[SetUp]
		public void Setup()
		{
			_com = new TestComms();
			_c = new SimpleCommandLineClient( _com );
		}

		public void Load( string[] str )
		{
			foreach( var s in str )
				_com.InputQueue.Enqueue( s );
		}

		[Test]
		public void ClientCanSaveLastValue()
		{

			var init = new SaveFile( "p.sav" );
			Assert.AreNotEqual( init.Latest.TrainerId, init.Latest.Team[1].OriginalTrainerId );
			if( File.Exists( "tmp33.sav" ) )
				File.Delete( "tmp33.sav" );

			Load( new[] { "ld p.sav", "r trainerid", "w team[1].originaltrainerid {}", "st tmp33.sav", "q" } );
			_c.Run( null );

			var sf = new SaveFile( "tmp33.sav" );
			Assert.AreEqual( sf.Latest.TrainerId, sf.Latest.Team[1].OriginalTrainerId );
		}

	}
}
