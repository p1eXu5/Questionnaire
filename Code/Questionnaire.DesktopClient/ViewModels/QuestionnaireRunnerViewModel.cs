using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.Models;
using Questionnaire.DesktopClient.ViewModels.EntityViewModel;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels
{
    public class QuestionnaireRunnerViewModel : ViewModel
    {
        private readonly IQuestionnaire _businessContext;

        private Queue< SectionViewModel > _sections;

        private SectionViewModel _questionA;
        private SectionViewModel _questionB;

        private bool _isNextQuestionA;
        private int _iter;

        public QuestionnaireRunnerViewModel ( IQuestionnaire businessContext )
        {
            _businessContext = businessContext ?? throw new ArgumentNullException( nameof( businessContext ), @"IQuestionnaire cannot be null." );

            _sections = new Queue< SectionViewModel >();
        }

        public event EventHandler< EventArgs > QuestionnaireStoped; 

        public int Count => _sections.Count;

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

        public Firm Firm { get; set; }

        public void Reload ()
        {
            _sections = new Queue< SectionViewModel >( _businessContext.GetSections().Select( s => new SectionViewModel( s, _businessContext ) ) );

            _isNextQuestionA = true;

            if ( _sections.Any() ) {

                QuestionA = _sections.Dequeue();
                _isNextQuestionA = false;
            }
        }

        private void OnStageChangeRequested ( object obj, EventArgs args )
        {
            if ( _sections.Any() ) {

                if ( _isNextQuestionA ) {
                    QuestionA = _sections.Dequeue();
                }
                else {
                    QuestionB = _sections.Dequeue();
                }

                return;
            }


        }

        private void OnQuestionnaireStopped ()
        {
            QuestionnaireStoped?.Invoke( this, EventArgs.Empty );
        }
    }
}
