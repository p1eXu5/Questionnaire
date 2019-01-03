using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public class AnswerOpen : AnswerBase
    {
        public string Answer { get; set; }

        public QuestionOpen Question { get; set; }

        public int EmployeeAnswerId { get; set; }
        public EmployeeAnswer EmployeeAnswer { get; set; }
    }
}
