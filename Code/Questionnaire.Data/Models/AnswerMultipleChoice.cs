using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public class AnswerMultipleChoice : AnswerBase
    {
        public int Answer { get; set; }

        public QuestionMultipleChoice Question { get; set; }
    }
}
