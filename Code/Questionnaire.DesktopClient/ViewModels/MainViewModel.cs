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
        private readonly IQuestionnaireBusinessContext businessContext;

        public MainViewModel ( IQuestionnaireBusinessContext questionnaireBusinessContext )
        {
            businessContext = questionnaireBusinessContext ?? throw new ArgumentNullException( nameof( questionnaireBusinessContext ), @"IQuestionnaireBusinessContext cannot be null." );
        }

        public IEnumerable< Firm > Firms => businessContext.GetFirms();

    }
}
