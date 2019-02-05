using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.BusinessContext.Contracts;
using Questionnaire.Data.DataContext;

namespace Questionnaire.Data.BusinessContext
{
    public class Service : IService
    {
        private bool _disposed;

        public Service ( QuestionnaireDbContext dbContext )
        {
            DbContext = dbContext;
        }

        public QuestionnaireDbContext DbContext { get; }

        public void Dispose ()
        {
            Dispose( true );
        }

        private void Dispose ( bool disposing )
        {
            if ( !disposing || _disposed) return;

            DbContext.Dispose();
            _disposed = true;
        }
    }
}
