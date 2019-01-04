using System.Collections.Generic;
using Questionnaire.Data.Models;

namespace Questionnaire
{
    public interface IQuestionnaire
    {
        IEnumerable< City > GetCities ();
        IEnumerable< Firm > GetFirms ();
        IEnumerable< Section > GetSections ();
        IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions ();
        IEnumerable< QuestionOpen > GetOpenQuestions ();
        void AddAnswers ( IEnumerable< AnswerMultipleChoice > answers );
        void AddAnswers ( IEnumerable< AnswerOpen > answers );
        void MakeReport ( string fileName );
    }
}