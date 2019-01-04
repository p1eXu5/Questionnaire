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

        IEnumerable< AnswerOpen > GetOpenAnswers ();

        void AddAnswer ( AnswerMultipleChoice answers );
        void AddAnswer ( AnswerOpen answers );
        void MakeReport ( string fileName );

        void SaveChanges ();
    }
}