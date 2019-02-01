using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;
using Questionnaire.Data.Models.Interfaces;

namespace Questionnaire.Data.Models
{
    public class Category : INameEntity
    {
        public Category ()
        {
            Sections = new List< Section >();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [ Hidden ]
        public ICollection< Section > Sections { get; set; }
    }
}
