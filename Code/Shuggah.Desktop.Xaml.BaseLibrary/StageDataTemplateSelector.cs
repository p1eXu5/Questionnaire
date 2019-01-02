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

        public StageDataTemplateSelector ( ) : base()
        {
            _map = new Dictionary< Type, string >();
        }

        public StageDataTemplateSelector ( IDictionary< Type, string > stageMap ) : base()
        {
            if ( stageMap == null ) throw new ArgumentNullException( nameof( stageMap ), "Stage map cannot be null." );

            _map = new Dictionary< Type, string >( stageMap );
        }

        public void AddStage ( object stage )
        {
            var dtName = $"dt_{ stage.GetType().Name }";
        }

        public IDictionary< Type, string > Map => _map;

        public override DataTemplate SelectTemplate ( object item, DependencyObject container )
        {
            if ( item != null && container is FrameworkElement element ) {

                foreach ( var type in _map.Keys ) {

                    if ( item.GetType().IsAssignableFrom( type ) ) {

                        try {
                            var tmp = element.FindResource( _map[ type ] ) as DataTemplate;
                            return tmp;
                        }
                        catch ( ResourceReferenceKeyNotFoundException ) {
                            return null;
                        }
                    }
                }
            }

            return null;
        }
    }
}
