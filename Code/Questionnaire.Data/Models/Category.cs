using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel.Attributes;

namespace Questionnaire.Data.Models
{
    public class Category
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
