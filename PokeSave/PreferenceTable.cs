using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PokeSave
{
	public static class PreferenceTable
	{
		static PreferenceTable()
		{
			_store = new Dictionary<MonsterNature, PreferenceEntry>();
			using( var textstream = new StreamReader( Assembly.GetExecutingAssembly().GetManifestResourceStream( "PokeSave.Resources.types.bin" ) ) )
			{
				string line;
				while( ( line = textstream.ReadLine() ) != null )
				{
					string[] d = line.Split( '\t' );
					Add(
						(MonsterNature) Enum.Parse( typeof( MonsterNature ), d[0] ),
						(Stat) Enum.Parse( typeof( Stat ), d[1] ),
						(Stat) Enum.Parse( typeof( Stat ), d[2] ),
						(Flavor) Enum.Parse( typeof( Flavor ), d[3] ),
						(Flavor) Enum.Parse( typeof( Flavor ), d[4] )
						);
				}
			}
		}
		public static double CalculateFactorForStat( MonsterNature nature, Stat stat )
		{
			if( _store.ContainsKey( nature ) )
			{
				if( _store[nature].PreferredStat == stat )
					return 1.1;
				if( _store[nature].DiminishedStat == stat )
					return 0.9;
			}
			return 1;
		}
		static void Add( MonsterNature nature, Stat pref, Stat dim, Flavor fav, Flavor dis )
		{
			_store.Add( nature, new PreferenceEntry { PreferredStat = pref, DiminishedStat = dim, FavoriteFlavor = fav, DislikedFlavor = dis } );
		}
		static IDictionary<MonsterNature, PreferenceEntry> _store;
	}
	public class PreferenceEntry
	{
		public Stat PreferredStat;
		public Stat DiminishedStat;
		public Flavor FavoriteFlavor;
		public Flavor DislikedFlavor;
	}

}