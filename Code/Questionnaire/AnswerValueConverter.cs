using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire
{
    public class AnswerValueConverter : IAnswerValueConverter
    {
        public const int NO_ANSWER = 1;
        public const int UNDEFINED_ANSWER = 2;
        public const int YES_UNSWER = 3;

        public AnswerMultipleChoice Convert ( AnswerMultipleChoice answer )
        {
            if ( answer.Answer <= -1 ) {
                answer.Answer = NO_ANSWER;
                return answer;
            }

            if ( answer.Answer >= 1 ) {
                answer.Answer = YES_UNSWER;
                return answer;
            }

            answer.Answer = UNDEFINED_ANSWER;
            return answer;
        }
    }
}
