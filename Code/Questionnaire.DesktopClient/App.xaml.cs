using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Questionnaire.Data.BusinessContext;
using Questionnaire.DesktopClient.ViewModels;
using Questionnaire.DesktopClient.Views;

namespace Questionnaire.DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup ( StartupEventArgs e )
        {
            base.OnStartup( e );

            var businessContext = new QuestionnaireBusinessContext();
            var questionnaire = new QuestionnaireContextContext( businessContext );
            var mainViewModel = new MainViewModel( questionnaire );

            var wnd = new MainWindow();
            wnd.DataContext = mainViewModel;

            wnd.ShowDialog();
        }
    }
}