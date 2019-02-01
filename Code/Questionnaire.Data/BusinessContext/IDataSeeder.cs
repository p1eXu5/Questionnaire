using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.DataContext;

namespace Questionnaire.Data.BusinessContext
{
    public interface IDataSeeder
    {
        void SeedData ( QuestionnaireBusinessContext context );
    }
}
