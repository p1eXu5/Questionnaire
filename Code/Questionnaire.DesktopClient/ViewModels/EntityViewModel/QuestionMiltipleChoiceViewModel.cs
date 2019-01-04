using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels.EntityViewModel
{
    public class QuestionMiltipleChoiceViewModel : ViewModel
    {
        private readonly QuestionMultipleChoice _question;

        public QuestionMiltipleChoiceViewModel ( QuestionMultipleChoice question )
        {
            _question = question ?? throw new ArgumentNullException( nameof( question ), @"Question cannot be null." );
            AnswerMultipleChoice = new AnswerMultipleChoice() { Question = question };
        }

        public string Text => _question.Text;

        public bool IsAnswered => throw new NotImplementedException();

        public AnswerMultipleChoice AnswerMultipleChoice { get; }

    }
}
