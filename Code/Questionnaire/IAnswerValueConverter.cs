using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire
{
    interface IAnswerValueConverter
    {
        AnswerMultipleChoice Convert ( AnswerMultipleChoice answer );
    }
}
