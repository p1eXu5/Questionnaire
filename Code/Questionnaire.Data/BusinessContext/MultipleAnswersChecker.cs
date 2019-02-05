
using System.Linq;
using Questionnaire.Data.BusinessContext.Contracts;
using Questionnaire.Data.DataContext;
using Questionnaire.Data.DataContext.CheckerExtensions;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public class MultipleAnswersChecker : Service, IMultipleAnswersChecker
    {
        public MultipleAnswersChecker ( QuestionnaireDbContext dbContext ) : base( dbContext ) { }

        public void CheckAnswers ()
        {
            var query = DbContext.GetIncorrectMultipleChoiceAnswers();
            if ( query.Count == 0 ) return;

            var min = query.Min( a => a.Answer );
            var max = query.Max( a => a.Answer );

            foreach ( var answerMultipleChoice in query.Where( a => a.Answer == min ) ) {
                answerMultipleChoice.Answer = -1;
            }

            foreach ( var answerMultipleChoice in query.Where( a => a.Answer == max ) ) {
                answerMultipleChoice.Answer = 1;
            }

            foreach ( var answerMultipleChoice in query.Where( a => a.Answer > min && a.Answer < max ) ) {
                answerMultipleChoice.Answer = 0;
            }

            DbContext.Set< AnswerMultipleChoice >().UpdateRange( query );
            DbContext.SaveChanges();
        }
    }
}
