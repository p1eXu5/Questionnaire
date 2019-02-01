using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;
using Questionnaire.Data.Models.Interfaces;

namespace Questionnaire.Data.Models
{
    public class Region : IEntity
    {
        public Region ()
        {
            CityCollection = new List< City >( 67 );
        }

        public int Id { get; set; }
        public string Name { get; set; }

        [ Hidden ]
        public ICollection< City > CityCollection { get; set; }
    }
}
