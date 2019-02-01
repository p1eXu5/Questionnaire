using Agbm.NpoiExcel.Attributes;

namespace Questionnaire.Data.Models
{
    public abstract class QuestionBase
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int SectionId { get; set; }

        [ Hidden ]
        public Section Section { get; set; }
    }
}
