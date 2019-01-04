using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public interface IQuestionnaireBusinessContext
    {
        IEnumerable< Region > GetRegions ();
        IEnumerable< City > GetCities ();
        IEnumerable< Firm > GetFirms ();
        IEnumerable< Section > GetSections ();
        IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions ();
        IEnumerable< QuestionOpen > GetOpenQuestions ();

        void AddAnswers ( IEnumerable< AnswerMultipleChoice > answers );
        void AddAnswers ( IEnumerable< AnswerOpen > answers );

        IEnumerable< dynamic > GetMultipleChoiceAnswers ();
    }
}
