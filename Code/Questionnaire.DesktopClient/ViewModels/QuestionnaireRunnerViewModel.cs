using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Questionnaire.Data.BusinessContext;
using Questionnaire.DesktopClient.ViewModels.EntityViewModel;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels
{
    public class QuestionnaireRunnerViewModel : ViewModel
    {
        private readonly IQuestionnaireBusinessContext _businessContext;

        private Queue< SectionViewModel > _questions;

        private SectionViewModel _questionA;
        private SectionViewModel _questionB;

        private bool _isNextQuestionA;
        private int _iter;

        public QuestionnaireRunnerViewModel ( IQuestionnaireBusinessContext businessContext )
        {
            _businessContext = businessContext ?? throw new ArgumentNullException( nameof( businessContext ), @"IBusinessContext cannot be null." );

            _questions = new Queue< SectionViewModel >( _businessContext.GetSections().Select( s => new SectionViewModel( s ) ) );

            if ( _questions.Any() ) {

                QuestionA = _questions.Dequeue();
                _questions.Enqueue( QuestionA );

                _iter = Count;
            }

            NextQuestionCommand = new MvvmCommand( NextQuestion, CanNextQuestion );
        }

        public event EventHandler< EventArgs > QuestionnaireStoped; 

        public int Count => _questions.Count;

        public SectionViewModel QuestionA
        {
            get => _questionA;
            private set {
                _questionA = value;
                OnPropertyChanged();
            }
        }

        public SectionViewModel QuestionB
        {
            get => _questionB;
            private set {
                _questionB = value;
                OnPropertyChanged();
            }
        }

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
