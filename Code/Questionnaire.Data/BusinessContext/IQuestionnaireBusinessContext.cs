using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.BusinessContext.AnswerConverters;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public interface IQuestionnaireBusinessContext
    {
        IAnswerValueConverter Converter { get; }

        int GetNextNumOfTested ( int firmId );
        bool HasMultipleChoiceAnswers ();

        IEnumerable< Region > GetRegions ();
        IEnumerable< City > GetCities ();
        IEnumerable< Firm > GetFirms ();
        IEnumerable< Section > GetSections ();
        IEnumerable< Category > GetCategories ();

        IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions ();
        IEnumerable< QuestionOpen > GetOpenQuestions ();

        IEnumerable< AnswerOpen > GetOpenAnswers ();
        IEnumerable< AnswerMultipleChoice > GetMultipleChoiceAnswers ();

        void AddRegions ( IEnumerable< Region > regions );

        void DeleteRegion ( Region region );


        void AddCities( IEnumerable< City > cities );
        void AddFirmTypes( IEnumerable< FirmType > firmTypes );
        void AddFirms( IEnumerable< Firm > firms );
        void AddCategories( IEnumerable< Category > categories );
        void AddSections( IEnumerable< Section > sections );
        void AddMultipleChoiceQuestions( IEnumerable< QuestionMultipleChoice > questions );
        void AddOpenQuestions( IEnumerable< QuestionOpen > questions );

        void AddAnswer ( AnswerMultipleChoice answer );
        void AddAnswer ( AnswerOpen answer );

        void DeleteAnswers ();
        void DeleteAnswers ( int firmId, int employeeNum );

        IEnumerable< dynamic > GetGruppedMultipleChoiceAnswers ();

        void SaveChanges ();
    }
}
