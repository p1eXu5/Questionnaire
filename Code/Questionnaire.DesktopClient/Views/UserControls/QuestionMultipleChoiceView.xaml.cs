﻿using System;
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

namespace Questionnaire.DesktopClient.Views.UserControls
{
    /// <summary>
    /// Interaction logic for QuestionMiltipleChoiceView.xaml
    /// </summary>
    public partial class QuestionMultipleChoiceView : UserControl
    {
        public QuestionMultipleChoiceView ()
        {
            InitializeComponent();
        }

        private void Border_OnMouseDown ( object sender, MouseButtonEventArgs e )
        {
            if (!(sender is Border border) ) return;

            if (!(border.Child is ToggleButton tb)) return;

            tb.IsChecked ^= true;
        }
    }
}
