using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;

namespace Questionnaire.Data.Models
{
    public class FirmType
    {
        public FirmType ()
        {
            FirmCollection = new List< Firm >();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [ Hidden ]
        public ICollection< Firm > FirmCollection { get; set; }
    }
}
