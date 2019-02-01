using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;
using Questionnaire.Data.Models.Interfaces;

namespace Questionnaire.Data.BusinessContext.Comparers
{
    public class NameEntityComparer : IEqualityComparer< INameEntity >
    {
        public bool Equals ( INameEntity x, INameEntity y )
        {
            if ( ReferenceEquals( x, y ) ) return true;
            if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) ) return false;

            return x.Name.Equals( y.Name );
        }

        public int GetHashCode ( INameEntity obj )
        {
            return obj.Name?.GetHashCode() ?? 0;
        }
    }
}
