using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Questionnaire.DesktopClient.Behaviors
{
    public class ToggleIsCheckedBehavior : Behavior< Decorator >
    {
        protected override void OnAttached ()
        {
            base.OnAttached();

            AssociatedObject.MouseDown += OnMouseDown;
        }

        protected override void OnDetaching ()
        {
            base.OnDetaching();

            AssociatedObject.MouseDown -= OnMouseDown;
        }

        private void OnMouseDown ( object sender, MouseButtonEventArgs args )
        {
            if (!(sender is Decorator decorator)) return;

            if (!(decorator.Child is ToggleButton tb)) return;

            tb.IsChecked ^= true;
        }
    }
}
