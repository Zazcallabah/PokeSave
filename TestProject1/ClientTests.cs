using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using PokeConsoleClient;
using PokeSave;

namespace TestProject1
{
	[TestFixture]
	public class ClientTests
	{
		SimpleCommandLineClient _c;
		TestComms _com;
		const string Outfile = "tmp33.sav";

		[SetUp]
		public void Setup()
		{
			_com = new TestComms();
			_c = new SimpleCommandLineClient( new InjectionQueueCommunicator( _com ), "test.json" );
			if( File.Exists( Outfile ) )
				File.Delete( Outfile );
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

			Load( new[] { "ld p.sav", "r trainerid", "w team[1].originaltrainerid {}", "st " + Outfile, "q" } );
			_c.Run( null );

			var sf = new SaveFile( Outfile );
			Assert.AreEqual( sf.Latest.TrainerId, sf.Latest.Team[1].OriginalTrainerId );
		}

		[Test]
		public void ClientCanPrintCommandList()
		{
			var hit = false;
			_com.DisplayCalled += ( a, e ) =>
			{
				if( "Test: aoeu".Equals( (string) a ) )
					hit = true;
			};

			Load( new[] { "cl", "q" } );
			_c.Run( null );
			Assert.IsTrue( hit );
		}

		[Test]
		public void ClientCanExcecuteCommands()
		{
			var commands = new[] { new Command{
				Trigger = "makeown",
				Actions = new []
				{
					"r pcbuffer[0].originaltrainerid",
					"w team[1].originaltrainerid {}",
					"r pcbuffer[0].gameoforigin",
					"w team[1].gameoforigin {}",
					"r pcbuffer[0].originaltrainergender",
					"w team[1].originaltrainergender {}",
					"r pcbuffer[0].originaltrainername",
					"w team[1].originaltrainername {}"
				}
			} };
			_c = new SimpleCommandLineClient( new InjectionQueueCommunicator( _com ), new CommandHandler( commands ) );
			var sf = new SaveFile( "p2.sav" );
			Assert.AreNotEqual( sf.Latest.PcBuffer[0].OriginalTrainerName, sf.Latest.Team[1].OriginalTrainerName );
			Assert.AreNotEqual( sf.Latest.PcBuffer[0].OriginalTrainerGender, sf.Latest.Team[1].OriginalTrainerGender );
			Assert.AreNotEqual( sf.Latest.PcBuffer[0].OriginalTrainerId, sf.Latest.Team[1].OriginalTrainerId );
			Assert.AreNotEqual( sf.Latest.PcBuffer[0].GameOfOrigin, sf.Latest.Team[1].GameOfOrigin );

			Load( new[] { "ld p2.sav", "c makeown", "st " + Outfile, "q" } );
			_c.Run( null );
			var sf2 = new SaveFile( Outfile );
			Assert.AreEqual( sf2.Latest.PcBuffer[0].OriginalTrainerName, sf2.Latest.Team[1].OriginalTrainerName );
			Assert.AreEqual( sf2.Latest.PcBuffer[0].OriginalTrainerGender, sf2.Latest.Team[1].OriginalTrainerGender );
			Assert.AreEqual( sf2.Latest.PcBuffer[0].OriginalTrainerId, sf2.Latest.Team[1].OriginalTrainerId );
			Assert.AreEqual( sf2.Latest.PcBuffer[0].GameOfOrigin, sf2.Latest.Team[1].GameOfOrigin );
		}

		[Test]
		public void ClientCanDeserializeJson()
		{
			const string command = @"[{""Name"":""Test"",""Actions"":[""p""],""Trigger"":""helo""}]";
			var result = JsonConvert.DeserializeObject<IList<Command>>( command );

			Assert.AreEqual( 1, result.Count );
			Assert.AreEqual( 1, result[0].Actions.Length );
			Assert.AreEqual( "p", result[0].Actions[0] );
			Assert.AreEqual( "helo", result[0].Trigger );
		}

		List<Command> MakeCommands()
		{
			return new List<Command>{
				new Command{Name = "First", Description = "D1",Actions = new []{"1A","1B"},Trigger = "1"},
				new Command{Name = "Second", Description = "D2",Actions = new []{"2A","2B"},Trigger = "s"},
			};
		}

		[Test]
		public void HandlerCanFetchCommandsFromFile()
		{
			File.WriteAllText( "tst.json", @"[{""Name"":""TMP"",""Trigger"":""t""}]" );
			var h = new CommandHandler( "tst.json" );
			var a = h.All().ToList();

			Assert.AreEqual( 1, a.Count );
			Assert.AreEqual( "TMP", a[0].Name );
		}

		[Test]
		public void HandlerCanListAllCommands()
		{
			var h = new CommandHandler( MakeCommands() );
			var a = h.All().ToList();

			Assert.AreEqual( 2, a.Count );
			Assert.AreEqual( "First", a[0].Name );
		}

		[Test]
		public void CanFindCommandByTrigger()
		{
			var h = new CommandHandler( MakeCommands() );
			var c = h.Get( "s" );

			Assert.AreEqual( "Second", c.Name );
		}
	}
}
