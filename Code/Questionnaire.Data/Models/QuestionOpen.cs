using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;

namespace Questionnaire.Data.Models
{
    public class QuestionOpen : QuestionBase
    {
        public QuestionOpen () : base()
        {
            Answers = new List< AnswerOpen >();
        }

        [ Hidden ]
        public ICollection< AnswerOpen > Answers { get; set; }
    }
}
