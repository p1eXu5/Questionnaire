using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models.Interfaces;

namespace Questionnaire.Data.BusinessContext.Visitors
{
    public interface IVisitor
    {
        bool IsCorrectDynamic ( IEntity entity, IQuestionnaireBusinessContext context );
    }
}
