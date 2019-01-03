using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel.Attributes;

namespace Questionnaire.Data.Models
{
    public class QuestionMultipleChoice : QuestionBase
    {
        public QuestionMultipleChoice () : base()
        {
            Answers = new List< AnswerMultipleChoice >();
        }

        [ Hidden ]
        public ICollection< AnswerMultipleChoice > Answers { get; set; }
    }
}
