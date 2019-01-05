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

        public QuestionMiltipleChoiceViewModel ( QuestionMultipleChoice question, int index )
        {
            _question = question ?? throw new ArgumentNullException( nameof( question ), @"Question cannot be null." );
            AnswerMultipleChoice = new AnswerMultipleChoice() { Question = question };

            Num = $"{ question.SectionId }.{ index }";
        }

        public string Num { get; }

        public string Text => _question.Text;

        public bool IsAnswered => YesAnswer || NoAnswer || UndefinedAnswer;

        public AnswerMultipleChoice AnswerMultipleChoice { get; }

        private bool _yesAnswer;

        public bool YesAnswer
        {
            get => _yesAnswer;
            set {
                _yesAnswer = value;
                OnPropertyChanged();
            }
        }

        private bool _noAnswer;

        public bool NoAnswer
        {
            get => _noAnswer;
            set {
                _noAnswer = value;
                OnPropertyChanged();
            }
        }

        private bool _undefinedAnswer;

        public bool UndefinedAnswer
        {
            get => _undefinedAnswer;
            set {
                _undefinedAnswer = value;
                OnPropertyChanged();
            }
        }
    }
}
