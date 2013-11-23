﻿
using System;

namespace PokeSave
{
	class ConsoleReader : IComms
	{
		public string ReadLine()
		{
			return Console.ReadLine();
		}

		public void Write( string str )
		{
			Console.Write( str );
		}

		public void WriteLine( string line )
		{
			Console.WriteLine( line );
		}
	}
}
