using System;
using System.Collections.Generic;
using System.Linq;
using PokeSave;

namespace TestProject1
{
	public class TestComms : IComms
	{
		public event EventHandler<EventArgs> DisplayCalled;

		public void WriteLine( string data )
		{
			if( null != DisplayCalled )
			{
				DisplayCalled( data, EventArgs.Empty );
			}
		}

		public void Write( string data )
		{
			WriteLine( data );
		}

		public Queue<string> InputQueue = new Queue<string>();

		public string ReadLine()
		{
			return InputQueue.Any() ? InputQueue.Dequeue() : string.Empty;
		}
	}
}