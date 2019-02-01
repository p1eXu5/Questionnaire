using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.BusinessContext;

namespace Questionnaire.DesktopClient.ViewModels
{
    public interface IMainViewModel
    {
        IQuestionnaireBusinessContext Context { get; }
    }
}
