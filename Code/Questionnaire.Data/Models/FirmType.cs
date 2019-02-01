using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;
using Questionnaire.Data.Models.Interfaces;

namespace Questionnaire.Data.Models
{
    public class FirmType : INameEntity
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
