using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public abstract class AnswerBase
    {
        public int Id { get; set; }
        public int Num { get; set; }

        public Firm Firm { get; set; }
    }
}
