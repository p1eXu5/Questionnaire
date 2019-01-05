using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels.EntityViewModel
{
    public class QuestionMultipleChoiceViewModel : ViewModel
    {
        private readonly QuestionMultipleChoice _question;
        private bool _yesAnswer;
        private bool _noAnswer;
        private bool _undefinedAnswer;

        public QuestionMultipleChoiceViewModel ( QuestionMultipleChoice question, int index )
        {
            _question = question ?? throw new ArgumentNullException( nameof( question ), @"Question cannot be null." );
            AnswerMultipleChoice = new AnswerMultipleChoice() { Question = question };

            Num = $"{ question.SectionId }.{ index }";
        }

        public string Num { get; }

        public string Text => _question.Text;

        public bool IsAnswered => YesAnswer || NoAnswer || UndefinedAnswer;

        public AnswerMultipleChoice AnswerMultipleChoice { get; }


        public bool YesAnswer
        {
            get => _yesAnswer;
            set {
                _yesAnswer = value;
                OnPropertyChanged();
                OnPropertyChanged( nameof( IsAnswered ) );
            }
        }

        public bool NoAnswer
        {
            get => _noAnswer;
            set {
                _noAnswer = value;
                OnPropertyChanged();
                OnPropertyChanged( nameof( IsAnswered ) );
            }
        }

        public bool UndefinedAnswer
        {
            get => _undefinedAnswer;
            set {
                _undefinedAnswer = value;
                OnPropertyChanged();
                OnPropertyChanged( nameof( IsAnswered ) );
            }
        }
    }
}
