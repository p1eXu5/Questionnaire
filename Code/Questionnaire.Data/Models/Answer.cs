using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public int PositiveAnswerCount { get; set; }
        public int IndefiniteAnswerCount { get; set; }
        public int NegativeAnswerCount { get; set; }  

        public Question Question { get; set; }
        public Firm Firm { get; set; }
    }
}
