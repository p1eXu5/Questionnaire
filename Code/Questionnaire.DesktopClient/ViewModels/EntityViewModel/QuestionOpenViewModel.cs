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

        public QuestionOpenViewModel ( QuestionOpen question, int index )
        {
            _question = question ?? throw new ArgumentNullException( nameof( question ), @"Question cannot be null." );
            AnswerOpen = new AnswerOpen() { Question = question };

            Num = $"{ question.SectionId }.{ index }";
        }

        public string Num { get; }

        public bool IsAnswered => !String.IsNullOrWhiteSpace( Answer );

        public AnswerOpen AnswerOpen { get; }

        public string Answer
        {
            get => AnswerOpen.Answer;
            set {
                AnswerOpen.Answer = value;
                OnPropertyChanged(  );
            }
        }
        public string Text => _question.Text;
    }
}
