﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;

using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.Models;
using Questionnaire.DesktopClient.ViewModels.DialogViewModels;
using Questionnaire.DesktopClient.ViewModels.EntityViewModel;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel, IMainViewModel
    {
        #region Fields

        private readonly IQuestionnaireBusinessContext _questionnaireContext;
        private readonly IDialogRegistrator _dialogRegistrator;
        private City _selectedCity;
        private readonly ObservableCollection< FirmViewModel > _firms;
        private FirmViewModel _selectedFirm;
        private readonly ICollectionView _firmsView;

        private bool _isRunning;

        #endregion



        #region Ctor

        public MainViewModel ( IQuestionnaireBusinessContext questionnaireContext, IDialogRegistrator dialogRegistrator )
        {
            _questionnaireContext = questionnaireContext ?? throw new ArgumentNullException( nameof( questionnaireContext ), @"IQuestionnaireContext cannot be null." );
            _dialogRegistrator = dialogRegistrator ?? throw new ArgumentNullException( nameof( dialogRegistrator ), @"IDialogRegistrator cannot be null." );

            Cities = _questionnaireContext.GetCities().ToArray();

            // firm with id equaled 1 is undefined firm
            _firms = new ObservableCollection< FirmViewModel >( _questionnaireContext.GetFirms().Where( f => f.Id > 1 ).Select( f => new FirmViewModel( f, this ) ) );
            if (!_firms.Any() ) throw new ArgumentException( "Firms is empty" );
            Firms = new ReadOnlyObservableCollection< FirmViewModel >( _firms );

            _firmsView = CollectionViewSource.GetDefaultView( Firms );

            SelectedCity = Cities.FirstOrDefault();

            RunTestCommand = new MvvmCommand( RunTest, CanRunTest );
            DeleteAnswersCommand = new MvvmCommand( DeleteAnswers, CanDeleteAnswers );
            ExportAnswersCommand = new MvvmCommand( ExportAnswers, CanDeleteAnswers );

            QuestionnaireRunner = new QuestionnaireRunnerViewModel( _questionnaireContext );
            QuestionnaireRunner.StopRequested += OnStopped;
        }

        #endregion



        #region Properties

        public IQuestionnaireBusinessContext Context => _questionnaireContext;

        public IEnumerable< City > Cities { get; }

        public City SelectedCity
        {
            get => _selectedCity;
            set {
                _selectedCity = value;
                OnPropertyChanged();

                if ( value != null ) {
                    SetFirmsFilter( value.Id );
                }

            }
        }

        public ReadOnlyObservableCollection< FirmViewModel > Firms { get; }

        public FirmViewModel SelectedFirm
        {
            get => _selectedFirm;
            set {
                if ( _selectedFirm?.Id != value?.Id ) {

                    _selectedFirm = value;
                    SelectedCity = Cities.First( c => c.Id == value?.CityId );

                    OnPropertyChanged();
                    ((MvvmCommand)RunTestCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public QuestionnaireRunnerViewModel QuestionnaireRunner { get; }

        public bool IsRunning
        {
            get => _isRunning;
            set {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        #endregion



        #region Commands

        public ICommand RunTestCommand { get; }

        public ICommand CheckAnswersCommand => new MvvmCommand( CheckAnswers );

        public ICommand DeleteAnswersCommand { get; } 

        public ICommand ExportAnswersCommand { get; } 

        public ICommand AboutProgramCommand => new MvvmCommand( AboutProgram );

        public ICommand ClosingCommand => new MvvmCommand( Closing );

        public ICommand ExitCommand => new MvvmCommand( Exit );

        #endregion



        #region Methods

        private void CheckAnswers ( object obj )
        {
            if ( !CanDeleteAnswers( null ) ) return;

            var dialog = _dialogRegistrator.GetView( new ResumeClearDialogViewModel() );

            if ( dialog == null ) throw new InvalidOperationException( "Cannot find ResumeClearDialogWindow." );

            if ( dialog.ShowDialog() == true ) {
                DeleteAnswers( null );
            }
        }

        private void SetFirmsFilter ( int cityId )
        {
            if ( cityId == 1 ) {

                _firmsView.Filter = null;
                return;
            }

            _firmsView.Filter = ( firm ) => (( FirmViewModel )firm).CityId == cityId;
        }

        private void RunTest ( object obj )
        {
            QuestionnaireRunner.LoadTestQuestions( _selectedFirm.Firm );
            IsRunning = true;
        }

        private bool CanRunTest ( object obj )
        {
            return SelectedFirm?.Id > 1 && _questionnaireContext.GetMultipleChoiceQuestions().Any();
        }

        private void OnStopped ( object sender, EventArgs args )
        {
            IsRunning = false;
            SelectedFirm.UpdateName();
            ((MvvmCommand)RunTestCommand).RaiseCanExecuteChanged();
            ((MvvmCommand)DeleteAnswersCommand).RaiseCanExecuteChanged();
            ((MvvmCommand)ExportAnswersCommand).RaiseCanExecuteChanged();
        }

        private void DeleteAnswers ( object obj )
        {
            var dialog = _dialogRegistrator.GetView( new AreYouSureViewModel() );

            if ( dialog.ShowDialog() == true ) {

                _questionnaireContext.DeleteAnswers();
                (( MvvmCommand )DeleteAnswersCommand).RaiseCanExecuteChanged();
                (( MvvmCommand )ExportAnswersCommand).RaiseCanExecuteChanged();
            }
        }

        private bool CanDeleteAnswers ( object obj )
        {
            return _questionnaireContext.HasMultipleChoiceAnswers();
        }

        private void ExportAnswers ( object obj )
        {
            var sfd = new SaveFileDialog {

                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Filter = "Excel file|*.xlsx",
                RestoreDirectory = true,
            };

            if ( sfd.ShowDialog() == true ) {

                ReportMaker.MakeReport( sfd.FileName, _questionnaireContext );
            }
        }

        private void AboutProgram ( object obj )
        {
            var dialog = _dialogRegistrator.GetView( new AboutProgramViewModel() );
            if ( dialog == null ) throw new InvalidOperationException( "Cannot find ResumeClearDialogWindow." );

            dialog.ShowDialog();
        }

        private void Closing ( object obj )
        {
            if ( !IsRunning ) return;

            _questionnaireContext.DeleteAnswers( _selectedFirm.Id, QuestionnaireRunner.EmployeeNum );
        }

        private void Exit ( object obj )
        {
            if ( IsRunning ) {

                var dialog = _dialogRegistrator.GetView( new CannotExitViewModel() );
                if ( dialog == null ) throw new InvalidOperationException( "Cannot find ResumeClearDialogWindow." );

                if ( dialog.ShowDialog() == true ) {
                    return;
                }
            }

            Application.Current.Shutdown();
        }

        #endregion
    }
}
