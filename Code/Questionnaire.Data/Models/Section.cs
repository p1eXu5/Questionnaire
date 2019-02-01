using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;

namespace Questionnaire.Data.Models
{
    public class Section
    {
        public Section ()
        {
            QuestionMultipleChoiceCollection = new List< QuestionMultipleChoice >();
            QuestionOpenCollection = new List< QuestionOpen >();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }

        [ Hidden ]
        public Category Category { get; set; }

        [ Hidden ]
        public ICollection< QuestionMultipleChoice > QuestionMultipleChoiceCollection { get; set; }

        [ Hidden ]
        public ICollection< QuestionOpen > QuestionOpenCollection { get; set; }
    }
}
