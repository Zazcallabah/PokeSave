using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PokeSave.Sixth
{
	public class Gen5TypeChart : TypeChart
	{
		public Gen5TypeChart() : base( "gen5typechart.bin" ) { }
	}

	public class Gen1TypeChart : TypeChart
	{
		public Gen1TypeChart() : base( "gen1typechart.bin" ) { }
	}

	public class Gen6TypeChart : TypeChart
	{
		public Gen6TypeChart() : base( "gen6typechart.bin" ) { }
	}
}