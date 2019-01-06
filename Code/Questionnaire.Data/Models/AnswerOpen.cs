using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public class AnswerOpen : AnswerBase
    {
        public AnswerOpen () : base() { }

        [ MinLength(7) ]
        public string Answer { get; set; }

        public QuestionOpen Question { get; set; }
    }
}
