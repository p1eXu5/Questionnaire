using System;
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
        private readonly IQuestionnaire _businessContext;
        private City _selectedCity;
        private Firm _selectedFirm;
        private readonly ICollectionView _firmsView;

        private bool _isRunning;

        public MainViewModel ( IQuestionnaire questionnaireBusinessContext )
        {
            _businessContext = questionnaireBusinessContext ?? throw new ArgumentNullException( nameof( questionnaireBusinessContext ), @"IQuestionnaire cannot be null." );

            Cities = _businessContext.GetCities().ToArray();
            Firms = _businessContext.GetFirms().Where( f => f.Id > 1 ).ToArray();

            _firmsView = CollectionViewSource.GetDefaultView( Firms );

            SelectedCity = Cities.FirstOrDefault();

            StartTestCommand = new MvvmCommand( StartTest, CanStartTest );

            QuestionnairRunnerViewModel = new QuestionnaireRunnerViewModel( _businessContext );
            QuestionnairRunnerViewModel.QuestionnaireStoped += OnStopped;
        }

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


        public ICommand StartTestCommand { get; }


        private void SetFirmsFilter ( int cityId )
        {
            if ( cityId == 1 ) {

                _firmsView.Filter = null;
                return;
            }

            _firmsView.Filter = ( firm ) => (( Firm )firm).CityId == cityId;
        }

        private void StartTest ( object obj )
        {
            QuestionnairRunnerViewModel.Firm = _selectedFirm;
            IsRunning = true;
        }

        private bool CanStartTest ( object obj )
        {
            return SelectedFirm?.Id > 1 && QuestionnairRunnerViewModel.Count > 0;
        }

        private void OnStopped ( object sender, EventArgs args )
        {
            IsRunning = false;
            QuestionnairRunnerViewModel.Reload();
            ((MvvmCommand)StartTestCommand).RaiseCanExecuteChanged();
        }
    }
}
