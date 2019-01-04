using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.Models;

namespace Questionnaire
{
    public class Questionnaire : IQuestionnaire
    {
        private readonly IQuestionnaireBusinessContext _context;
        private readonly IAnswerValueConverter _converter = new AnswerValueConverter();

        public Questionnaire ( IQuestionnaireBusinessContext context )
        {
            _context = context ?? throw new ArgumentNullException( nameof( context ), "context cannot be null." ); ;
        }

        public IEnumerable< City > GetCities () => _context.GetCities();
        public IEnumerable< Firm > GetFirms () => _context.GetFirms();
        public IEnumerable< Section > GetSections () => _context.GetSections();
        public IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions () => _context.GetMultipleChoiceQuestions();
        public IEnumerable< QuestionOpen > GetOpenQuestions () => _context.GetOpenQuestions();

        public IEnumerable< AnswerOpen > GetOpenAnswers () => _context.GetOpenAnswers();

        public void AddAnswer ( AnswerMultipleChoice answer ) => _context.AddAnswer( _converter.Convert( answer ) );
        public void AddAnswer ( AnswerOpen answer ) => _context.AddAnswer( answer );

        public void MakeReport ( string fileName ) =>
            ReportMaker.MakeReport( fileName, _context.GetMultipleChoiceAnswers().Cast< AnswerMultipleChoice >() );

        public void SaveChanges () => _context.SaveChanges();
    }
}
