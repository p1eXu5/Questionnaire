using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public class EmployeeAnswer
    {
        public int Id { get; set; }

        public Firm Firm { get; set; }
        public ICollection< AnswerMultipleChoice > AnswerMultipleChoiceCollection { get; set; }
        public ICollection< AnswerOpen > AnswerOpenCollection { get; set; }
    }
}
