using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NUnit.Framework;

namespace Shuggah.Desktop.Xaml.BaseLibrary.Tests
{
    [ TestFixture ]
    public class StageDataTemplateSelector_UnitTests
    {
        [ Test ]
        public void Ctor_DictIsNull_Throws ()
        {
            // Action:
            Assert.Catch< ArgumentNullException >(() => new StageDataTemplateSelector( null ) );
        }

        [ Test ]
        public void Ctro_MapIsNotEmpty_AddsElements ()
        {
            // Arrange:
            var map = new Dictionary< Type, string > { [ typeof( StageDataTemplateSelector_UnitTests ) ] = "asd" };

            // Action:
            var selector = new StageDataTemplateSelector( map );

            // Assert:
            Assert.That( selector.Map.Any() );
        }

        [ Test ]
        [ Apartment( ApartmentState.STA ) ]
        public void SelectTemplate_ItemValid_ReturnsDataTemplate ()
        {
            // Arrange:
            var resourceName = "test";

            var map = new Dictionary< Type, string > { [ typeof( StageDataTemplateSelector_UnitTests ) ] = resourceName };
            var selector = new StageDataTemplateSelector( map );

            var elem = new FrameworkElement();
            elem.Resources.Add( resourceName, new DataTemplate() );

            object obj = new StageDataTemplateSelector_UnitTests();

            // Action:
            var template = selector.SelectTemplate( obj, elem );

            // Assert:
            Assert.That( template != null );
        }
    }
}
