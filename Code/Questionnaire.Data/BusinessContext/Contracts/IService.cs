using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.DataContext;

namespace Questionnaire.Data.BusinessContext.Contracts
{
    public interface IService : IDisposable
    {
        QuestionnaireDbContext DbContext { get; }
    }
}
