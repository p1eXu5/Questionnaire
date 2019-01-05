﻿using System;
using System.Collections.Generic;

using System.ComponentModel;
using System.Windows.Data;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.Models;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Fields

        private readonly IQuestionnaireContext _questionnaireContext;
        private City _selectedCity;
        private Firm _selectedFirm;
        private readonly ICollectionView _firmsView;

        private bool _isRunning;

        #endregion


        #region Ctor

        public MainViewModel ( IQuestionnaireContext questionnaireContext )
        {
            _questionnaireContext = questionnaireContext ?? throw new ArgumentNullException( nameof( questionnaireContext ), @"IQuestionnaireContext cannot be null." );

            Cities = _questionnaireContext.GetCities().ToArray();
            Firms = _questionnaireContext.GetFirms().Where( f => f.Id > 1 ).ToArray();

            _firmsView = CollectionViewSource.GetDefaultView( Firms );

            SelectedCity = Cities.FirstOrDefault();

            StartTestCommand = new MvvmCommand( RunTest, CanRunTest );

            QuestionnairRunnerViewModel = new QuestionnaireRunnerViewModel( _questionnaireContext );
            QuestionnairRunnerViewModel.StopRequested += OnStopped;
        }

        #endregion


        #region Properties

        public IEnumerable< City > Cities { get; }
        public City SelectedCity
        {
            get => _selectedCity;
            set {
                _selectedCity = value;

                if ( _selectedFirm != null ) {
                    SetFirmsFilter( _selectedCity.Id );
                }

                OnPropertyChanged();
            }
        }

        public IEnumerable< Firm > Firms { get; }
        public Firm SelectedFirm
        {
            get => _selectedFirm;
            set {
                if ( _selectedFirm?.Id != value?.Id ) {

                    _selectedFirm = value;
                    SelectedCity = value?.City;
                }

                OnPropertyChanged();
                ((MvvmCommand)StartTestCommand).RaiseCanExecuteChanged();
            }
        }

        public QuestionnaireRunnerViewModel QuestionnairRunnerViewModel { get; }

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

        public ICommand StartTestCommand { get; }

        #endregion


        #region Methods

        private void SetFirmsFilter ( int cityId )
        {
            if ( cityId == 1 ) {

                _firmsView.Filter = null;
                return;
            }

            _firmsView.Filter = ( firm ) => (( Firm )firm).CityId == cityId;
        }

        private void RunTest ( object obj )
        {
            QuestionnairRunnerViewModel.SetFirm ( _selectedFirm );
            IsRunning = true;
        }

        private bool CanRunTest ( object obj )
        {
            return SelectedFirm?.Id > 1 && QuestionnairRunnerViewModel.Count > 0;
        }

        private void OnStopped ( object sender, EventArgs args )
        {
            IsRunning = false;
            QuestionnairRunnerViewModel.Reload();
            ((MvvmCommand)StartTestCommand).RaiseCanExecuteChanged();
        }

        #endregion
    }
}
