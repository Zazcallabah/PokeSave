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
			_controllers.Add( "Gen III", new Gen3SearchController() );
			_controllers.Add( "Gen I", new Gen1SearchController() );
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

		public string Types
		{
			get
			{
				var t = Controller.Types( SelectedName );
				if( t.Length == 2 )
					return t[0] + "\r\n" + t[1];
				return t[0].ToString();
			}
		}

		public TypeEntry[] Notes { get { return Controller.DefendFilter( Controller.Types( SelectedName ) ); } }

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
		TypeEntry[] DefendFilter( MonsterType[] monsterType );
	}

	public class TypeEntry
	{
		static readonly IDictionary<decimal, string> Tagmap = new Dictionary<decimal, string>
		{
			{0m,"0x"},
			{0.25m,"¼x"},
			{.5m,"½x"},
			{1m,""},
			{2m,"2x"},
			{4m,"4x"}
		};
		public decimal Weight { get; set; }
		public string Tag
		{
			get
			{
				if( Tagmap.ContainsKey( Weight ) )
					return Tagmap[Weight];
				return string.Empty;
			}
		}
		public MonsterType Type { get; set; }
	}

	public abstract class SearchController : ISearchController
	{
		readonly TypeChart _chart;
		readonly ITypeInformation _info;
		protected SearchController( TypeChart chart, ITypeInformation info )
		{
			_chart = chart;
			_info = info;
		}

		public string[] Names { get { return _info.Names; } }

		public MonsterType[] Types( string selected )
		{
			return _info.GetTypeByName( selected );
		}

		public TypeEntry[] DefendFilter( MonsterType[] defendingType )
		{
			var line = _chart.For( defendingType );
			return _chart.Types.Select( t => new TypeEntry { Type = t, Weight = line.AttackedBy( t ) } ).Where( t => t.Weight != 1 ).ToArray();
		}
	}

	public class Gen1SearchController : SearchController
	{
		public Gen1SearchController()
			: base( new Gen1TypeChart(), new Gen1TypeInformation() )
		{
		}
	}
	
	public class Gen3SearchController : SearchController
	{
		public Gen3SearchController()
			: base( new Gen5TypeChart(), new Gen3TypeInformation() )
		{
		}
	}

	public class Gen5SearchController : SearchController
	{
		public Gen5SearchController()
			: base( new Gen5TypeChart(), new Gen5TypeInformation() )
		{
		}
	}

	public class Gen6SearchController : SearchController
	{
		public Gen6SearchController()
			: base( new Gen6TypeChart(), new Gen6TypeInformation() )
		{
		}
	}
}
