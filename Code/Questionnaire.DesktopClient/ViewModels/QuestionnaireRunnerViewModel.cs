using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Questionnaire.Data.BusinessContext;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels
{
    public class QuestionnaireRunnerViewModel : ViewModel
    {
        private readonly IQuestionnaireBusinessContext _businessContext;

        private Queue< QuestionViewModel > _questions;

        private QuestionViewModel _questionA;
        private QuestionViewModel _questionB;

        private bool _isNextQuestionA;
        private int _iter;

        public QuestionnaireRunnerViewModel ( IQuestionnaireBusinessContext businessContext )
        {
            _businessContext = businessContext ?? throw new ArgumentNullException( nameof( businessContext ), @"IBusinessContext cannot be null." );

            _questions = new Queue< QuestionViewModel >( _businessContext.GetQuestions() );

            if ( _questions.Any() ) {

                QuestionA = _questions.Dequeue();
                _questions.Enqueue( QuestionA );

                _iter = Count;
            }

            NextQuestionCommand = new MvvmCommand( NextQuestion, CanNextQuestion );
        }

        public event EventHandler< EventArgs > QuestionnaireStoped; 

        public int Count => _questions.Count;

        public QuestionViewModel QuestionA { get; private set; }
        public QuestionViewModel QuestionB { get; private set; }

        public ICommand NextQuestionCommand;

        private void NextQuestion ( object obj )
        {
            if ( _iter > 0 ) {



                --_iter;
            }
            else {
                QuestionnaireStoped?.Invoke( this, EventArgs.Empty );
            }
        }

        private bool CanNextQuestion ( object obj )
        {
            return true;
        }
    }
}
