using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.BusinessContext;

namespace Questionnaire
{
    public class Questionnaire
    {
        private readonly IQuestionnaireBusinessContext _context;

        public Questionnaire ( IQuestionnaireBusinessContext context )
        {
            _context = context ?? throw new ArgumentNullException( nameof( context ), "context cannot be null." ); ;
        }
    }
}
