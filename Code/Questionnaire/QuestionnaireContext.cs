using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.Models;

namespace Questionnaire
{
    public class QuestionnaireContext : IQuestionnaireContext
    {
        private readonly IQuestionnaireBusinessContext _context;
        private readonly IAnswerValueConverter _converter = new AnswerValueConverter();

        public QuestionnaireContext ( IQuestionnaireBusinessContext context )
        {
            _context = context ?? throw new ArgumentNullException( nameof( context ), "context cannot be null." ); ;
        }

        public IEnumerable< City > GetCities () => _context.GetCities();
        public IEnumerable< Firm > GetFirms () => _context.GetFirms();
        public IEnumerable< Section > GetSections () => _context.GetSections();
        public IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions () => _context.GetMultipleChoiceQuestions();
        public IEnumerable< QuestionOpen > GetOpenQuestions () => _context.GetOpenQuestions();

        public IEnumerable< AnswerOpen > GetOpenAnswers () => _context.GetOpenAnswers();

        public bool HasMultipleChoiceAnswers () => _context.GetMultipleChoiceAnswers().Any();

        public void AddAnswer ( AnswerMultipleChoice answer ) => _context.AddAnswer( _converter.Convert( answer ) );
        public void AddAnswer ( AnswerOpen answer ) => _context.AddAnswer( answer );

        public void DeleteAnswers () => _context.DeleteAnswers();

        public void MakeReport ( string fileName ) =>
            ReportMaker.MakeReport( fileName, _context );

        public void SaveChanges () => _context.SaveChanges();

        public int GetNextNumOfTested ( int firmId )
        {
            var openAnsw = _context.GetOpenAnswers().Where( a => a.FirmId == firmId ).ToArray();
            var multiAnsw = _context.GetMultipleChoiceAnswers().Where( a => a.FirmId == firmId ).ToArray();

            if ( openAnsw.Length == 0 && multiAnsw.Length == 0 ) return 1;

            if ( openAnsw.Length == 0 ) return multiAnsw.Max( a => a.Num ) + 1;
            if ( multiAnsw.Length == 0 ) return openAnsw.Max( a => a.Num ) + 1;

            var multiNum = multiAnsw.Max( a => a.Num );
            var openNum = openAnsw.Max( a => a.Num );

            return multiNum >= openNum ? multiNum + 1 : openNum + 1;
        }
    }
}
