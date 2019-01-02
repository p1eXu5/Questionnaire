using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shuggah.Desktop.Xaml.BaseLibrary;

namespace Shuggah.Desktop.Xaml.BaseCustomControlLibrary
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Shuggah.Desktop.Xaml.BaseCustomControlLibrary"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Shuggah.Desktop.Xaml.BaseCustomControlLibrary;assembly=Shuggah.Desktop.Xaml.BaseCustomControlLibrary"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:StageSwitcher/>
    ///
    /// </summary>
    public class StageSwitcher : Selector
    {


        static StageSwitcher ()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( StageSwitcher ), new FrameworkPropertyMetadata( typeof( StageSwitcher ) ) );
        }

        public StageSwitcher ()
        {

        }



        public IStageViewModel StageA
        {
            get { return ( IStageViewModel )GetValue( StageAProperty ); }
            set { SetValue( StageAProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for StageA.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StageAProperty =
            DependencyProperty.Register( "StageA", typeof( IStageViewModel ), typeof( StageSwitcher ), new PropertyMetadata( null ) );



        public IStageViewModel StageB
        {
            get { return ( IStageViewModel )GetValue( StageBProperty ); }
            set { SetValue( StageBProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for StageB.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StageBProperty =
            DependencyProperty.Register( "StageB", typeof( IStageViewModel ), typeof( StageSwitcher ), new PropertyMetadata( null ) );


    }
}
