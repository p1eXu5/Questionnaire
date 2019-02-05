
// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable RedundantCatchClause

using System;
using System.IO;
using System.Windows;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.DataContext;
using Questionnaire.Data.Models;
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
    public partial class App
    {
        protected override void OnStartup ( StartupEventArgs e )
        {
            //FunctionsAssemblyResolver.RedirectAssembly();

            base.OnStartup( e );

            try {
                var startupWnd = new StartupWindow();
                startupWnd.Show();

                var wnd = new MainWindow();
                var businessContext = new QuestionnaireBusinessContext( new DataSeeder() );

                using ( var checker = new MultipleAnswersChecker( new QuestionnaireDbContext() ) ) {
                    checker.CheckAnswers();
                }
                
                var mainViewModel = new MainViewModel( businessContext, GetDialogRegistrator( wnd ) );

                wnd.DataContext = mainViewModel;

                startupWnd.Close();

                wnd.ShowDialog();
            }
            catch
#if RELEASE
                ( Exception ex ) 
#endif
            {
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

        private IDialogRegistrator GetDialogRegistrator ( Window ownerWindow )
        {
            IDialogRegistrator dialogRegistrator = new DialogRegistrator( ownerWindow );
            dialogRegistrator.Register<ResumeClearDialogViewModel, ResumeClearDialogWindow>();
            dialogRegistrator.Register<CannotExitViewModel, CannotExitWindow>();
            dialogRegistrator.Register<AboutProgramViewModel, AboutProgramWindow>();
            dialogRegistrator.Register<AreYouSureViewModel, AreYouSureDialog>();

            return dialogRegistrator;
        }
    }
}
