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
using Questionnaire.DesktopClient.ViewModels.DialogViewModels;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Fields -

        private readonly IQuestionnaireContext _questionnaireContext;
        private readonly IDialogRegistrator _dialogRegistrator;
        private City _selectedCity;
        private Firm _selectedFirm;
        private readonly ICollectionView _firmsView;

        private bool _isRunning;

        #endregion



        #region Ctor

        public MainViewModel ( IQuestionnaireContext questionnaireContext, IDialogRegistrator dialogRegistrator )
        {
            _questionnaireContext = questionnaireContext ?? throw new ArgumentNullException( nameof( questionnaireContext ), @"IQuestionnaireContext cannot be null." );
            _dialogRegistrator = dialogRegistrator ?? throw new ArgumentNullException( nameof( dialogRegistrator ), @"IDialogRegistrator cannot be null." );

            Cities = _questionnaireContext.GetCities().ToArray();
            Firms = _questionnaireContext.GetFirms().Where( f => f.Id > 1 ).ToArray();

            if ( !Firms.Any() ) throw new ArgumentException( "Firms is empty" );

            _firmsView = CollectionViewSource.GetDefaultView( Firms );

            SelectedCity = Cities.FirstOrDefault();

            RunTestCommand = new MvvmCommand( RunTest, CanRunTest );

            QuestionnaireRunner = new QuestionnaireRunnerViewModel( _questionnaireContext );
            QuestionnaireRunner.StopRequested += OnStopped;

            CheckAnswers();
        }

        #endregion



        #region Properties

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

        public IEnumerable< Firm > Firms { get; }

        public Firm SelectedFirm
        {
            get => _selectedFirm;
            set {
                if ( _selectedFirm?.Id != value?.Id ) {

                    _selectedFirm = value;
                    SelectedCity = Cities.First( c => c.Id == value.CityId );

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

        #endregion



        #region Methods

        private void CheckAnswers ()
        {
            if ( !_questionnaireContext.HasMultipleChoiceAnswers() ) return;

            var dialog = _dialogRegistrator.GetView( new ResumeClearDialogViewModel() );
            if ( dialog == null ) throw new InvalidOperationException( "Cannot find ResumeClearDialogWindow." );

            if ( dialog.ShowDialog() == true ) {
                _questionnaireContext.DeleteAnswers();
            }
        }

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
            QuestionnaireRunner.SetFirm ( _selectedFirm );
            IsRunning = true;
        }

        private bool CanRunTest ( object obj )
        {
            return SelectedFirm?.Id > 1 && _questionnaireContext.GetMultipleChoiceQuestions().Any();
        }

        private void OnStopped ( object sender, EventArgs args )
        {
            IsRunning = false;
            QuestionnaireRunner.Reload();
            ((MvvmCommand)RunTestCommand).RaiseCanExecuteChanged();
        }

        #endregion
    }
}
