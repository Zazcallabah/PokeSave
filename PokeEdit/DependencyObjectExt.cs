using System.Windows;
using System.Windows.Media;

namespace PokeEdit
{
	public static class DependencyObjectExt
	{
		public static T FirstAncestorOfType<T>( this DependencyObject target ) where T : DependencyObject
		{
			if( target.GetType() == typeof( T ) )
				return (T) target;

			return FirstAncestorOfType<T>( VisualTreeHelper.GetParent( target ) );
		}

	}
}