using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.Models;

namespace Questionnaire
{
    public class Questionnaire
    {
        private readonly IQuestionnaireBusinessContext _context;

        public Questionnaire ( IQuestionnaireBusinessContext context )
        {
            _context = context ?? throw new ArgumentNullException( nameof( context ), "context cannot be null." ); ;
        }

        public IEnumerable< City > GetCities => _context.GetCities();
        public IEnumerable< Firm > GetFirms => _context.GetFirms();
        public IEnumerable< Section > GetSections => _context.GetSections();
        public IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions () => _context.GetMultipleChoiceQuestions();
        public IEnumerable< QuestionOpen > GetOpenQuestions () => _context.GetOpenQuestions();

        public void AddAnswers ( IEnumerable< AnswerMultipleChoice > answers ) => _context.AddAnswers( answers );
        public void AddAnswers ( IEnumerable< AnswerOpen > answers ) => _context.AddAnswers( answers );

        public void MakeReport ( string fileName ) =>
            ReportMaker.MakeReport( fileName, _context.GetMultipleChoiceAnswers() );
    }
}
