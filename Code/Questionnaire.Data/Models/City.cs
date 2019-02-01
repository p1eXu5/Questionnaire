using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;
using Questionnaire.Data.Models.Interfaces;

namespace Questionnaire.Data.Models
{
    public class City : INameEntity
    {
        public City ()
        {
            FirmCollection = new List< Firm >(242);
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public int RegionId { get; set; }

        [ Hidden ]
        public Region Region { get; set; }

        [ Hidden ]
        public ICollection< Firm > FirmCollection { get; set; }
    }
}
