using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.DataContext.CheckerExtensions
{ 
    public static class QuestionnaireDbContextExtension
    {
        public static List< AnswerMultipleChoice > GetIncorrectMultipleChoiceAnswers ( this QuestionnaireDbContext dbContext )
        {
            IQueryable< AnswerMultipleChoice > query = dbContext.MultipleChoiceAnswers;

            var groups = query.GroupBy( q => q.Num ).Where( g => g.Any( a => a.Answer > 1 ) ).Select( g => g.ToList() ).ToList();
            if ( groups.Count == 0 ) return new List< AnswerMultipleChoice >(0);

            var answers = groups.Aggregate( ( total, next ) => total.Union( next ).ToList() );
            return answers;
        }
    }
}
