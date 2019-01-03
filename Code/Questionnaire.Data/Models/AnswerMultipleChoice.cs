using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public class AnswerMultipleChoice : AnswerBase
    {
        public bool? Answer { get; set; }

        public QuestionMultipleChoice Question { get; set; }

        public int EmployeeAnswerId { get; set; }
        public EmployeeAnswer EmployeeAnswer { get; set; }
    }
}
