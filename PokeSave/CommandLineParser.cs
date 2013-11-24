
using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace PokeSave
{
	public class CommandLineParser
	{
		static readonly Regex ExtractIndex = new Regex( @"(?<property>\w+)\[(?<index>\d+)\]", RegexOptions.Compiled );
		public string Read( SaveFile sf, string line )
		{
			var commandchain = line.Trim().Split( '.' );

			object current = sf.Latest;
			foreach( var command in commandchain )
			{
				if( "A".Equals( command, StringComparison.InvariantCultureIgnoreCase ) )
				{
					current = sf.A;
					continue;
				}
				if( "B".Equals( command, StringComparison.InvariantCultureIgnoreCase ) )
				{
					current = sf.B;
					continue;
				}

				var t = current.GetType();
				var match = ExtractIndex.Match( command );
				if( match.Success )
				{
					var propertyname = match.Groups["property"].Captures[0].Value;
					var indexstring = match.Groups["index"].Captures[0].Value;
					int index;
					if( !Int32.TryParse( indexstring, out index ) )
					{
						return "not valid index";
					}
					var p = t.GetProperty( propertyname, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance );
					if( !p.PropertyType.IsArray )
						return "Property isnt an array, dont index into it";
					var arr = (Array) p.GetValue( current, null );
					current = arr.GetValue( index );
					continue;
				}

				var prop = t.GetProperty( command, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance );
				if( prop == null )
					return "Not valid property";
				if( prop.PropertyType.IsArray )
					return "Property is array, index into it using []";
				if( prop.PropertyType == typeof( string ) )
					return (string) prop.GetValue( current, null );
				current = prop.GetValue( current, null );
			}
			return current.ToString();
		}

		public string Write( SaveFile sf, string line )
		{
			var parts = line.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
			if( parts.Length < 2 )
				return "not enough arguments";
			var commandchain = parts[0].Split( '.' );
			var value = parts[1];

			object current = sf.Latest;
			foreach( var command in commandchain )
			{
				if( "A".Equals( command, StringComparison.InvariantCultureIgnoreCase ) )
				{
					current = sf.A;
					continue;
				}
				if( "B".Equals( command, StringComparison.InvariantCultureIgnoreCase ) )
				{
					current = sf.B;
					continue;
				}

				var t = current.GetType();
				var match = ExtractIndex.Match( command );
				if( match.Success )
				{
					var propertyname = match.Groups["property"].Captures[0].Value;
					var indexstring = match.Groups["index"].Captures[0].Value;
					int index;
					if( !Int32.TryParse( indexstring, out index ) )
					{
						return "not valid index";
					}
					var p = t.GetProperty( propertyname, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance );
					if( !p.PropertyType.IsArray )
						return "Property isnt an array, dont index into it";
					var arr = (Array) p.GetValue( current, null );
					current = arr.GetValue( index );
					continue;
				}

				var prop = t.GetProperty( command, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance );
				if( prop == null )
					return "Not valid property";
				if( prop.PropertyType.IsArray )
					return "Property is array, index into it using []";
				if( !prop.CanWrite )
					return "Property is readonly";
				if( prop.PropertyType == typeof( string ) )
				{
					prop.SetValue( current, value, null );
					return "Ok string " + value;
				}
				if( prop.PropertyType == typeof( uint ) )
				{
					uint uintval;
					if( !UInt32.TryParse( value, out uintval ) )
					{
						return "not valid number";
					}

					prop.SetValue( current, uintval, null );
					return "Ok uint " + uintval;
				}
				if( prop.PropertyType == typeof( bool ) )
				{
					var boolval = "true".Equals( value, StringComparison.InvariantCultureIgnoreCase );
					prop.SetValue( current, boolval, null );
					return "Ok bool " + boolval;
				}
				current = prop.GetValue( current, null );
			}
			return "Strange type found, couldnt write";
		}

		public string List( SaveFile sf, string line )
		{
			var commandchain = line.Trim().Split( '.' );

			object current = sf.Latest;
			foreach( var command in commandchain )
			{
				if( "A".Equals( command, StringComparison.InvariantCultureIgnoreCase ) )
				{
					current = sf.A;
					continue;
				}
				if( "B".Equals( command, StringComparison.InvariantCultureIgnoreCase ) )
				{
					current = sf.B;
					continue;
				}

				var t = current.GetType();
				var match = ExtractIndex.Match( command );
				if( match.Success )
				{
					var propertyname = match.Groups["property"].Captures[0].Value;
					var indexstring = match.Groups["index"].Captures[0].Value;
					int index;
					if( !Int32.TryParse( indexstring, out index ) )
					{
						return "not valid index";
					}
					var p = t.GetProperty( propertyname, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance );
					if( !p.PropertyType.IsArray )
						return "Property isnt an array, dont index into it";
					var arr = (Array) p.GetValue( current, null );
					current = arr.GetValue( index );
					continue;
				}
				if( string.IsNullOrEmpty( command ) )
					break;
				var prop = t.GetProperty( command, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance );
				if( prop == null )
					return "Not valid property";
				if( prop.PropertyType.IsArray )
					return "Property is array, index into it using []";
				current = prop.GetValue( current, null );
			}

			var type = current.GetType();
			var sb = new StringBuilder();
			foreach( var p in type.GetProperties( BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance ) )
			{
				sb.AppendLine( string.Format( "{0} ({1})", p.Name, p.PropertyType ) );
			}
			return sb.ToString();
		}
	}
}