using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel.Attributes;

namespace Questionnaire.Data.Models
{
    /// <summary>
    /// Question with three answer options.
    ///     No answer is -1 and less.
    ///     Yes answer is 1 and greater.
    ///     Undefined answer is 0.
    /// </summary>
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
