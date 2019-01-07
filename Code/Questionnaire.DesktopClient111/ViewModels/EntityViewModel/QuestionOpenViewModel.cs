using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels.EntityViewModel
{
    public class QuestionOpenViewModel : ViewModel
    {
        private readonly QuestionOpen _question;

        // We create
        private readonly AnswerOpen _answerOpen;

        public QuestionOpenViewModel ( QuestionOpen question, int index )
        {
            _question = question ?? throw new ArgumentNullException( nameof( question ), @"Question cannot be null." );

            _answerOpen = new AnswerOpen() { Question = question };

            QuestionGeneratedNum = $"{ question.SectionId }.{ index }";
        }

        public string QuestionGeneratedNum { get; }

        public bool IsAnswered => !String.IsNullOrWhiteSpace( Answer );

        public AnswerOpen AnswerOpen => _answerOpen;

        public string Answer
        {
            get => _answerOpen.Answer;
            set {
                _answerOpen.Answer = value;
                OnPropertyChanged();
                OnPropertyChanged( nameof( IsAnswered ) );
            }
        }
        public string Text => _question.Text;
    }
}
