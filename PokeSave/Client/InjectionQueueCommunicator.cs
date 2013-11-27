using System.Collections.Generic;
using System.Linq;

namespace PokeSave.Client
{
	public class InjectionQueueCommunicator
	{
		readonly IComms _com;
		readonly Queue<string> _injectionQueue = new Queue<string>();

		public InjectionQueueCommunicator( IComms com )
		{
			_com = com;
		}

		public void Inject( IEnumerable<string> lines )
		{
			foreach( string s in lines )
				_injectionQueue.Enqueue( s );
		}

		public void Write( string str )
		{
			_com.Write( str );
		}

		public void WriteLine( string line )
		{
			_com.WriteLine( line );
		}

		public string ReadLine()
		{
			return _injectionQueue.Any() ? _injectionQueue.Dequeue() : _com.ReadLine();
		}
	}
}