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
        private Firm _selectedFirm;

        public MainViewModel ( IQuestionnaireBusinessContext questionnaireBusinessContext )
        {
            _businessContext = questionnaireBusinessContext ?? throw new ArgumentNullException( nameof( questionnaireBusinessContext ), @"IQuestionnaireBusinessContext cannot be null." );
        }

        public IEnumerable< City > Cities => _businessContext.GetCities();
        public IEnumerable< Firm > Firms => _businessContext.GetFirms();

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
