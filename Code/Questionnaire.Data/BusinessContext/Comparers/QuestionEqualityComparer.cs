using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext.Comparers
{
    public class QuestionEqualityComparer : IEqualityComparer< QuestionBase >
    {
        public bool Equals ( QuestionBase x, QuestionBase y )
        {
            if ( ReferenceEquals( x, y ) ) return true;
            if ( ReferenceEquals( x, null ) || ReferenceEquals( y, null ) ) return false;

            return x.Text.Equals( y.Text );
        }

        public int GetHashCode ( QuestionBase obj )
        {
            return obj.Text?.GetHashCode() ?? 0;
        }
    }
}
