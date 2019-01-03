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
        private readonly IQuestionnaireBusinessContext _businessContext;
        private City _selectedCity;
        private Firm _selectedFirm;
        private readonly ICollectionView _firmsView;

        private bool _isRunning;

        public MainViewModel ( IQuestionnaireBusinessContext questionnaireBusinessContext )
        {
            _businessContext = questionnaireBusinessContext ?? throw new ArgumentNullException( nameof( questionnaireBusinessContext ), @"IQuestionnaireBusinessContext cannot be null." );

            Cities = _businessContext.GetCities().ToArray();
            Firms = _businessContext.GetFirms().Where( f => f.Id > 1 ).ToArray();

            _firmsView = CollectionViewSource.GetDefaultView( Firms );

            SelectedCity = Cities.FirstOrDefault();

            StartTestCommand = new MvvmCommand( StartTest, CanStartTest );
        }


        public IEnumerable< City > Cities { get; }
        public IEnumerable< Firm > Firms { get; }

        public ICommand StartTestCommand { get; }

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

        public Firm SelectedFirm
        {
            get => _selectedFirm;
            set {
                if ( _selectedFirm?.Id != value?.Id ) {

                    _selectedFirm = value;
                    SelectedCity = _selectedFirm?.City;
                }

                OnPropertyChanged();
                ((MvvmCommand)StartTestCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set {
                _isRunning = value;
                OnPropertyChanged();
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

        private void StartTest ( object obj )
        {
            throw new NotImplementedException();
        }

        private bool CanStartTest ( object obj )
        {
            return SelectedFirm.Id > 1;
        }
    }
}
