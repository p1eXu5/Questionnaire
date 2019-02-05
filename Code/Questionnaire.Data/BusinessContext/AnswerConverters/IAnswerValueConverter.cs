using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext.AnswerConverters
{
    public interface IAnswerValueConverter
    {
        int Convert ( AnswerMultipleChoice answer );

        bool IsYesAnswer ( AnswerMultipleChoice answer );
        bool IsNoAnswer ( AnswerMultipleChoice answer );
        bool IsUndefinedAnswer ( AnswerMultipleChoice answer );
    }
}
