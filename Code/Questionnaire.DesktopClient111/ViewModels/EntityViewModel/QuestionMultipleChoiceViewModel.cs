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
        private readonly AnswerMultipleChoice _answerMultipleChoice;
        private int? _localAnswer = null;

        public QuestionMultipleChoiceViewModel ( QuestionMultipleChoice question, int index )
        {
            _question = question ?? throw new ArgumentNullException( nameof( question ), @"Question cannot be null." );
            _answerMultipleChoice = new AnswerMultipleChoice() { Question = _question };


            QuestionGeneratedNum = $"{ question.SectionId }.{ index }";
        }

        public string QuestionGeneratedNum { get; }

        public string Text => _question.Text;

        public bool IsAnswered => YesAnswer || NoAnswer || UndefinedAnswer;

        public AnswerMultipleChoice AnswerMultipleChoice => _answerMultipleChoice;


        public bool YesAnswer
        {
            get => _localAnswer >= 1;
            set {
                if ( _localAnswer == 1 ) {
                    _localAnswer = 0;
                    _answerMultipleChoice.Answer = 0;
                }
                else {
                    _answerMultipleChoice.Answer = 1;
                    _localAnswer = 1;
                }
                OnPropertyChanged();
                OnPropertyChanged( nameof( NoAnswer ) );
                OnPropertyChanged( nameof( UndefinedAnswer ) );
                OnPropertyChanged( nameof( IsAnswered ) );
            }
        }

        public bool NoAnswer
        {
            get => _localAnswer <= -1;
            set {
                if ( _localAnswer == -1 ) {
                    _localAnswer = 0;
                    _answerMultipleChoice.Answer = 0;
                }
                else {
                    _answerMultipleChoice.Answer = -1;
                    _localAnswer = -1;
                }

                OnPropertyChanged();
                OnPropertyChanged( nameof( YesAnswer ) );
                OnPropertyChanged( nameof( UndefinedAnswer ) );
                OnPropertyChanged( nameof( IsAnswered ) );
            }
        }

        public bool UndefinedAnswer
        {
            get => _localAnswer == 0;
            set {
                if ( _localAnswer == 0 ) {
                    _localAnswer = null;
                }
                else {
                    _answerMultipleChoice.Answer = 0;
                    _localAnswer = 0;
                }

                OnPropertyChanged();
                OnPropertyChanged( nameof( YesAnswer ) );
                OnPropertyChanged( nameof( NoAnswer) );
                OnPropertyChanged( nameof( IsAnswered ) );
            }
        }
    }
}
