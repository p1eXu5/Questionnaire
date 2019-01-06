using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    /// <summary>
    /// Answer on multiple choice question.
    /// </summary>
    public class AnswerMultipleChoice : AnswerBase
    {
        public AnswerMultipleChoice () : base() { }

        public int Answer { get; set; }

        public QuestionMultipleChoice Question { get; set; }
    }
}
