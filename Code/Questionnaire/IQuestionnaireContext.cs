using System.Collections.Generic;
using Questionnaire.Data.Models;

namespace Questionnaire
{
    public interface IQuestionnaireContext
    {
        IEnumerable< City > GetCities ();
        IEnumerable< Firm > GetFirms ();
        IEnumerable< Section > GetSections ();
        IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions ();
        IEnumerable< QuestionOpen > GetOpenQuestions ();

        IEnumerable< AnswerOpen > GetOpenAnswers ();

        bool HasMultipleChoiceAnswers ();

        void AddAnswer ( AnswerMultipleChoice answers );
        void AddAnswer ( AnswerOpen answers );
        void DeleteAnswers ();

        void MakeReport ( string fileName );

        void SaveChanges ();

        int GetNextNumOfTested ( int firmId );
    }
}

