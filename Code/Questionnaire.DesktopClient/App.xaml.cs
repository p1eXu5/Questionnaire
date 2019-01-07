using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Questionnaire.Data.BusinessContext;
using Questionnaire.DesktopClient.ViewModels;
using Questionnaire.DesktopClient.ViewModels.DialogViewModels;
using Questionnaire.DesktopClient.Views;
using Questionnaire.DesktopClient.Views.Dialogs;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup ( StartupEventArgs e )
        {
            //FunctionsAssemblyResolver.RedirectAssembly();

            base.OnStartup( e );

            try {

                var wnd = new MainWindow();

                var businessContext = new QuestionnaireBusinessContext();
                var questionnaire = new QuestionnaireContext( businessContext );

                // IDialogRegistrator:
                DialogRegistrator dialogRegistrator = new DialogRegistrator( wnd );
                dialogRegistrator.Register< ResumeClearDialogViewModel, ResumeClearDialogWindow >();
                dialogRegistrator.Register< CannotExitViewModel, CannotExitWindow >();
                dialogRegistrator.Register< AboutProgramViewModel, AboutProgramWindow >();

                var mainViewModel = new MainViewModel( questionnaire, dialogRegistrator );

                wnd.DataContext = mainViewModel;

                wnd.ShowDialog();
            }
            catch ( Exception ex ) {
#if DEBUG
                throw;
#endif

#if RELEASE
                string message = $"{ex.Message} \n {ex.InnerException?.Message}";
                File.AppendAllText( "questionnaire.log", message );

                new ExceptionWindow().ShowDialog();
                App.Current.Shutdown( 0x00040000 );
#endif
            }
        }
    }
}
