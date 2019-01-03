using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public class AnswerBase
    {
        public int Id { get; set; }

        public Firm Firm { get; set; }
    }
}
