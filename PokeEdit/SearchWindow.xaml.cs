using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PokeSave;
using PokeSave.Sixth;

namespace PokeEdit
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class SearchWindow : Window
	{
		public SearchWindow()
		{
			InitializeComponent();
		}
	}

	public class SearchPresenter : INotifyPropertyChanged
	{
		readonly IDictionary<string, ISearchController> _controllers = new Dictionary<string, ISearchController>();

		string _selectedName;
		string _selectedController;

		public SearchPresenter()
		{
			_selectedController = "Gen VI";
			_controllers.Add( "Gen VI", new Gen6SearchController() );
			_controllers.Add( "Gen III", new Gen6SearchController() );
		}

		public string[] Controllers { get { return _controllers.Keys.ToArray(); } }

		public string[] Names { get { return Controller.Names; } }

		public string SelectedName
		{
			get { return _selectedName; }
			set
			{
				if( _selectedName != value )
				{
					_selectedName = value;
					InvokeAll();
				}
			}
		}

		public string SelectedController
		{
			get { return _selectedController; }
			set
			{
				if( _selectedController != value )
				{
					_selectedController = value;
					InvokeAll();
				}
			}
		}

		ISearchController Controller { get { return _controllers[_selectedController]; } }

		public string Type1
		{
			get
			{
				return Controller.Types( SelectedName ).First().ToString();
			}
		}

		public string Type2
		{
			get
			{
				var t = Controller.Types( SelectedName );
				if( t.Length == 1 || t[0] == t[1] )
					return string.Empty;
				return t[1].ToString();
			}
		}

		public string DefStrong1
		{
			get
			{
				var t = Controller.Types( SelectedName ).First();
				return Concat( Controller.DefendFilter( t, "½" ) );
			}
		}

		public string DefStrong2
		{
			get
			{
				var t = Controller.Types( SelectedName );
				if( t.Length == 1 || t[0] == t[1] )
					return string.Empty;
				return Concat( Controller.DefendFilter( t[1], "½" ) );
			}
		}

		public string AttStrong1
		{
			get
			{
				var t = Controller.Types( SelectedName ).First();
				return Concat( Controller.AttackFilter( t, "2" ) );
			}
		}

		public string AttStrong2
		{
			get
			{
				var t = Controller.Types( SelectedName );
				if( t.Length == 1 || t[0] == t[1] )
					return string.Empty;
				return Concat( Controller.AttackFilter( t[1], "2" ) );
			}
		}
		public string DefWeak1
		{
			get
			{
				var t = Controller.Types( SelectedName ).First();
				return Concat( Controller.DefendFilter( t, "2" ) );
			}
		}

		public string DefWeak2
		{
			get
			{
				var t = Controller.Types( SelectedName );
				if( t.Length == 1 || t[0] == t[1] )
					return string.Empty;
				return Concat( Controller.DefendFilter( t[1], "2" ) );
			}
		}

		public string DefImmune1
		{
			get
			{
				var t = Controller.Types( SelectedName ).First();
				return Concat( Controller.DefendFilter( t, "0" ) );
			}
		}

		public string DefImmune2
		{
			get
			{
				var t = Controller.Types( SelectedName );
				if( t.Length == 1 || t[0] == t[1] )
					return string.Empty;
				return Concat( Controller.DefendFilter( t[1], "0" ) );
			}
		}

		public string AttWeak1
		{
			get
			{
				var t = Controller.Types( SelectedName ).First();
				return Concat( Controller.AttackFilter( t, "½" ) );
			}
		}

		public string AttWeak2
		{
			get
			{
				var t = Controller.Types( SelectedName );
				if( t.Length == 1 || t[0] == t[1] )
					return string.Empty;
				return Concat( Controller.AttackFilter( t[1], "½" ) );
			}
		}

		public string AttImmune1
		{
			get
			{
				var t = Controller.Types( SelectedName ).First();
				return Concat( Controller.AttackFilter( t, "0" ) );
			}
		}

		public string AttImmune2
		{
			get
			{
				var t = Controller.Types( SelectedName );
				if( t.Length == 1 || t[0] == t[1] )
					return string.Empty;
				return Concat( Controller.AttackFilter( t[1], "0" ) );
			}
		}

		string Concat( MonsterType[] types )
		{
			var sb = new StringBuilder();
			foreach( var type in types )
				sb.AppendLine( type.ToString() );
			return sb.ToString();
		}


		public event PropertyChangedEventHandler PropertyChanged;

		void InvokeAll()
		{
			if( PropertyChanged != null )
				foreach( var prop in GetType().GetProperties() )
					PropertyChanged( this, new PropertyChangedEventArgs( prop.Name ) );
		}
	}

	internal interface ISearchController
	{
		string[] Names { get; }
		MonsterType[] Types( string selected );
		MonsterType[] DefendFilter( MonsterType monsterType, string lookup );
		MonsterType[] AttackFilter( MonsterType monsterType, string lookup );
	}


	public class Gen6SearchController : ISearchController
	{
		public Gen6SearchController()
		{
			Names = Gen6TypeInformation.Names;
		}

		public string[] Names { get; private set; }

		public MonsterType[] Types( string selected )
		{
			return Gen6TypeInformation.GetTypeByName( selected );
		}

		public MonsterType[] DefendFilter( MonsterType monsterType, string lookup )
		{
			return TypeChart.DefendFilter( monsterType, lookup );
		}

		public MonsterType[] AttackFilter( MonsterType monsterType, string lookup )
		{
			return TypeChart.AttackFilter( monsterType, lookup );
		}
	}

	public class Gen3SearchController : ISearchController
	{
		public Gen3SearchController()
		{
			Names = NameList.All();
		}

		public string[] Names { get; private set; }

		public MonsterType[] Types( string selected )
		{
			var info = MonsterList.Get( selected );
			if( info.Type1 == info.Type2 )
				return new[] { info.Type1 };
			return new[] { info.Type1, info.Type2 };
		}

		public MonsterType[] DefendFilter( MonsterType monsterType, string lookup )
		{
			throw new NotImplementedException();
		}

		public MonsterType[] AttackFilter( MonsterType monsterType, string lookup )
		{
			throw new NotImplementedException();
		}
	}
}
