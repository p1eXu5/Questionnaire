using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Shuggah.Desktop.Xaml.BaseLibrary
{
    public class StageDataTemplateSelector : DataTemplateSelector
    {
        private readonly Dictionary< Type, string > _map;

        public StageDataTemplateSelector ( IDictionary< Type, string > map) : base()
        {
            if ( map == null ) throw new ArgumentNullException( nameof( map ), "map cannot be null." );

            _map = new Dictionary< Type, string >( map );
        }

        public IDictionary< Type, string > Map => _map;

        public override DataTemplate SelectTemplate ( object item, DependencyObject container )
        {
            if ( item != null && container is FrameworkElement element ) {

                foreach ( var type in _map.Keys ) {

                    if ( item.GetType().IsAssignableFrom( type ) ) {
                        return element.FindResource(_map[ type ]) as DataTemplate;
                    }
                }
            }

            return null;
        }
    }
}
