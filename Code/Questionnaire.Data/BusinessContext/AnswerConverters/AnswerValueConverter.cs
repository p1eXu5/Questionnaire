using System;
using System.Runtime.CompilerServices;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext.AnswerConverters
{
    public class AnswerValueConverter : IAnswerValueConverter
    {
        private const int NO_ANSWER = 1;
        private const int UNDEFINED_ANSWER = 2;
        private const int YES_ANSWER = 3;

        public int Convert ( AnswerMultipleChoice answer )
        {
            if ( answer?.Answer > 0 ) return YES_ANSWER;
            if ( answer?.Answer < 0 ) return NO_ANSWER;
            
            return UNDEFINED_ANSWER;
        }

        public bool IsYesAnswer ( AnswerMultipleChoice answer )
        {
            return answer?.Answer > 0;
        }

        public bool IsNoAnswer ( AnswerMultipleChoice answer )
        {
            return answer?.Answer < 0;
        }

        public bool IsUndefinedAnswer ( AnswerMultipleChoice answer )
        {
            return answer?.Answer == 0;
        }
    }
}
