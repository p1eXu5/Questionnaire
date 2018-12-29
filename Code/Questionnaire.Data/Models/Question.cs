using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public Section Section { get; set; }
    }
}
