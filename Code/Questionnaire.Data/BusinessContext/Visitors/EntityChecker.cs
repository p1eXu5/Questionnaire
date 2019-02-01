using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;
using Questionnaire.Data.Models.Interfaces;

namespace Questionnaire.Data.BusinessContext.Visitors
{
    internal class EntityChecker : IVisitor
    {
        public bool IsCorrectDynamic ( IEntity entity, IQuestionnaireBusinessContext context ) => IsCorrect( ( dynamic )entity, context );

        private bool IsCorrect ( Region region, IQuestionnaireBusinessContext context )
        {
            return true;
        }
    }
}
