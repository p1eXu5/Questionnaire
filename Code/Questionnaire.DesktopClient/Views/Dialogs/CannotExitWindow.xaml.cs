﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for CannotExitWindow.xaml
    /// </summary>
    public partial class CannotExitWindow : Window, IDialog
    {
        public CannotExitWindow ()
        {
            InitializeComponent();
        }
    }
}
