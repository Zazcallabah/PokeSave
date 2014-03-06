using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PokeSave.Sixth
{
	public abstract class TypeInformation : ITypeInformation
	{
		readonly IDictionary<string, MonsterType[]> List;
		protected TypeInformation( string path )
		{
			List = new Dictionary<string, MonsterType[]>();
			using( var textstream = new StreamReader( Assembly.GetExecutingAssembly().GetManifestResourceStream( "PokeSave.Resources." + path ) ) )
			{
				string line;
				while( ( line = textstream.ReadLine() ) != null )
				{
					var entries = line.Split( ',', ';' );
					List.Add( entries[0], entries.Skip( 1 ).Select( s => (MonsterType) Enum.Parse( typeof( MonsterType ), s ) ).ToArray() );
				}
			}
		}

		public string[] Names { get { return List.Keys.ToArray(); } }

		public MonsterType[] GetTypeByName( string name )
		{
			return List[name];
		}
	}

	public class Gen6TypeInformation : TypeInformation
	{
		public Gen6TypeInformation()
			: base( "gen6dextypes.bin" ) { }
	}

	public class Gen3TypeInformation : ITypeInformation
	{
		public Gen3TypeInformation()
		{
			Names = NameList.All();
		}

		public string[] Names { get; private set; }
		public MonsterType[] GetTypeByName( string name )
		{
			var info = MonsterList.Get( name );
			if( info.Type1 == info.Type2 )
				return new[] { info.Type1 };
			return new[] { info.Type1, info.Type2 };
		}
	}


	public class Gen1TypeInformation : TypeInformation
	{
		public Gen1TypeInformation()
			: base( "gen1dextypes.bin" )
		{
		}
	}



	public class Gen5TypeInformation : ITypeInformation
	{
		public Gen5TypeInformation()
		{
			Names = NameList.All();
		}

		public string[] Names { get; private set; }
		public MonsterType[] GetTypeByName( string name )
		{
			var info = MonsterList.Get( name );
			if( info.Type1 == info.Type2 )
				return new[] { info.Type1 };
			return new[] { info.Type1, info.Type2 };
		}
	}


	public interface ITypeInformation
	{
		string[] Names { get; }
		MonsterType[] GetTypeByName( string name );
	}
}
