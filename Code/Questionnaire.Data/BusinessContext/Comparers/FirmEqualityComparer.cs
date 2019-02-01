using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext.Comparers
{
    public class FirmEqualityComparer : IEqualityComparer< Firm >
    {
        public bool Equals ( Firm x, Firm y )
        {
            if ( ReferenceEquals( x, y ) ) return true;
            if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) ) return false;

            return x.Id == y.Id;
        }

        public int GetHashCode ( Firm obj )
        {
            return obj?.Id.GetHashCode() ?? 0;
        }
    }
}
