using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel.Attributes;

namespace Questionnaire.Data.Models
{
    public class Firm
    {
        public Firm ()
        {
            OpenAnswers = new List< AnswerOpen >();
            MultipleChoiceAnswers = new List< AnswerMultipleChoice >();
        }

        [ DatabaseGenerated( DatabaseGeneratedOption.None ) ]
        public int Id { get; set; }
        public string Name { get; set; }

        public int FirmTypeId { get; set; }
        public int CityId { get; set; }

        [ Hidden ]
        public FirmType FirmType { get; set; }
        [ Hidden ]
        public City City { get; set; }

        [ Hidden ]
        public ICollection< AnswerOpen > OpenAnswers { get; set; }

        [ Hidden ]
        public ICollection< AnswerMultipleChoice > MultipleChoiceAnswers { get; set; }
    }
}
