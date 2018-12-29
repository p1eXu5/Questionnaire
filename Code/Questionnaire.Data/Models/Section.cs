using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Data.Models
{
    public class Section
    {
        public Section ()
        {
            Questions = new List< Question >();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Note Note { get; set; }
        public ICollection< Question > Questions { get; set; }
    }
}
