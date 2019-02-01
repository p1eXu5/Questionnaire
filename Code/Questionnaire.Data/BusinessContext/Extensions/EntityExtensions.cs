using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.BusinessContext.Visitors;
using Questionnaire.Data.Models.Interfaces;

namespace Questionnaire.Data.BusinessContext.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsCorrect ( this IEntity entity, IVisitor visitor, IQuestionnaireBusinessContext context )
        {
            return visitor.IsCorrectDynamic( entity, context );
        }
    }
}
