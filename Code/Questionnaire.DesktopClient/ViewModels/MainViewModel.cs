using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public MainViewModel ( IQuestionnaireBusinessContext questionnaireBusinessContext )
        {
            _businessContext = questionnaireBusinessContext ?? throw new ArgumentNullException( nameof( questionnaireBusinessContext ), @"IQuestionnaireBusinessContext cannot be null." );

            Cities = _businessContext.GetCities();
            Firms = _businessContext.GetFirms();


            SelectedCity = Cities.FirstOrDefault();
            SelectedFirm = Firms.FirstOrDefault();
        }

        public IEnumerable< City > Cities { get; }
        public IEnumerable< Firm > Firms { get; }

        public City SelectedCity
        {
            get => _selectedCity;
            set {
                _selectedCity = value;
                OnPropertyChanged();
            }
        }

        public Firm SelectedFirm
        {
            get => _selectedFirm;
            set {
                _selectedFirm = value;
                OnPropertyChanged();
            }
        }
    }
}
