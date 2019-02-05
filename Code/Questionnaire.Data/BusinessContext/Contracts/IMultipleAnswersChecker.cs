using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.BusinessContext.Contracts
{
    public interface IMultipleAnswersChecker : IService
    {
        void CheckAnswers ();
    }
}
