using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using PokeSave;

namespace PokeConsoleClient
{
	public class CommandLineParser
	{
		public class PropertyCarrier
		{
			public object Parent { get; set; }
			public PropertyInfo Property { get; set; }
			public string Error { get; set; }
			public int? Index { get; set; }
		}

		static readonly Regex ExtractIndex = new Regex( @"(?<property>\w+)\[(?<index>\d+)\]", RegexOptions.Compiled );

		public string Read( SaveFile sf, string line )
		{
			var carry = GetPropertyForString( sf, line );
			if( !string.IsNullOrEmpty( carry.Error ) )
				return carry.Error;
			if( carry.Property == null )
				return carry.Parent.ToString();

			var target = carry.Property.GetValue( carry.Parent, null );

			if( !( target is IList ) )
				return target.ToString();

			if( carry.Index != null )
				return ( (IList) target )[carry.Index.Value].ToString();

			var sb = new StringBuilder();
			foreach( var entry in (IList) target )
				sb.AppendLine( entry.ToString() );
			sb.AppendLine( "index into array using []" );
			return sb.ToString();
		}

		public string[] ExtractCommands( string line )
		{
			line = line.Trim();
			var ignorelist = new[] { "A", "B", "LATEST" };
			if( ignorelist.All( ignore =>
					!line.StartsWith( ignore, StringComparison.InvariantCultureIgnoreCase )
					&& !line.StartsWith( ignore, StringComparison.InvariantCultureIgnoreCase ) ) )
				line = "latest." + line;
			return line.Split( new[] { '.' }, StringSplitOptions.RemoveEmptyEntries );
		}

		public void SetCurrentParentFromProperty( PropertyCarrier carrier )
		{
			if( carrier.Property != null )
			{
				carrier.Parent = carrier.Property.GetValue( carrier.Parent, null );
				if( carrier.Parent is IList && carrier.Index.HasValue )
					carrier.Parent = ( (IList) carrier.Parent )[carrier.Index.Value];
			}
		}

		public PropertyCarrier GetPropertyForString( SaveFile savefile, string line )
		{
			var commandchain = ExtractCommands( line );
			var current = new PropertyCarrier() { Parent = savefile };

			foreach( var command in commandchain )
			{
				SetCurrentParentFromProperty( current );

				var t = current.Parent.GetType();
				var match = ExtractIndex.Match( command );
				var propertyname = command;
				if( match.Success )
				{
					propertyname = match.Groups["property"].Captures[0].Value;
					var indexstring = match.Groups["index"].Captures[0].Value;
					int index = Int32.Parse( indexstring );
					current.Index = index;
				}
				else
					current.Index = null;

				current.Property = t.GetProperty( propertyname, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance );
				if( current.Property == null )
				{
					current.Error = "Not valid property";
					return current;
				}
			}
			return current;
		}

		public string Write( SaveFile sf, string line )
		{
			var parts = line.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
			if( parts.Length < 2 )
				return "not enough arguments";
			object value = parts[1];
			var carry = GetPropertyForString( sf, parts[0] );
			if( !string.IsNullOrEmpty( carry.Error ) )
				return carry.Error;
			if( carry.Property == null )
				return "you cant set root property";

			if( !carry.Property.CanWrite )
				return "property is readonly";

			var propertyType = carry.Property.PropertyType;

			if( carry.Property.PropertyType.IsGenericType && carry.Property.PropertyType.GetGenericTypeDefinition() == typeof( BindingList<> ) )
				return "you cant set array";

			try
			{
				if( propertyType == typeof( uint ) )
					value = UInt32.Parse( (string) value );
				else if( propertyType == typeof( bool ) )
					value = Boolean.Parse( (string) value );
				else if( propertyType.IsEnum )
					value = Enum.IsDefined( propertyType, value )
						? Enum.Parse( propertyType, (string) value, true )
						: Enum.ToObject( propertyType, UInt32.Parse( (string) value ) );

				carry.Property.SetValue( carry.Parent, value, null );
				return "OK, set " + value;
			}
			catch( FormatException )
			{
				return "bad argument value";
			}
			catch( ArgumentException )
			{
				return "bad argument value";
			}
		}

		public string ConstructPropertyListFromType( Type type )
		{
			var sb = new StringBuilder();
			foreach( var p in type.GetProperties( BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance ) )
			{
				sb.AppendLine( string.Format( "{0} ({1})", p.Name, p.PropertyType.Name ) );
			}
			return sb.ToString();
		}

		public string ConstructEnumValuesList( Type type )
		{
			var sb = new StringBuilder();
			foreach( var n in System.Enum.GetNames( type ) )
			{
				sb.AppendLine( n );
			}
			return sb.ToString();
		}

		public string List( SaveFile sf, string line )
		{
			var carry = GetPropertyForString( sf, line );
			if( !string.IsNullOrEmpty( carry.Error ) )
				return carry.Error;

			if( carry.Property == null )
				return ConstructPropertyListFromType( carry.Parent.GetType() );

			if( carry.Property.PropertyType.IsGenericType && carry.Property.PropertyType.GetGenericTypeDefinition() == typeof( BindingList<> ) )
				return ConstructPropertyListFromType( carry.Property.PropertyType.GetGenericArguments()[0] );

			if( carry.Property.PropertyType.IsEnum )
				return ConstructEnumValuesList( carry.Property.PropertyType );

			return ConstructPropertyListFromType( carry.Property.PropertyType );
		}
	}
}