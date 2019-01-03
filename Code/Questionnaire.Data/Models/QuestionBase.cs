using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel.Attributes;

namespace Questionnaire.Data.Models
{
    public class QuestionBase
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int SectionId { get; set; }

        [ Hidden ]
        public Section Section { get; set; }
    }
}
