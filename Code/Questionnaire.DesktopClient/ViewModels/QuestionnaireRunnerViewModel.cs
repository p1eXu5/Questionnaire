using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        #region Fields

        private readonly IQuestionnaireContext _questionnaireContext;

        private Queue< SectionViewModel > _sections;

        private SectionViewModel _sectionA;
        private SectionViewModel _sectionB;

        private bool _isNextSectionA;
        private int _testedNum;

        #endregion



        #region Ctor

        public QuestionnaireRunnerViewModel ( IQuestionnaireContext questionnairContext )
        {
            _questionnaireContext = questionnairContext ?? throw new ArgumentNullException( nameof( questionnairContext ), @"IQuestionnaireContext cannot be null." );

            _sections = new Queue< SectionViewModel >();
        }

        #endregion



        #region Events

        public event EventHandler< EventArgs > StopRequested;

        #endregion



        #region Properties

        public Firm Firm { get; private set; }

        public int Count => _sections.Count;

        public SectionViewModel SectionA
        {
            get => _sectionA;
            private set {
                _sectionA = value;
                OnPropertyChanged();
            }
        }

        public SectionViewModel SectionB
        {
            get => _sectionB;
            private set {
                _sectionB = value;
                OnPropertyChanged();
            }
        }

        #endregion
        


        #region Methods

        public void LoadTestQuestions ( Firm firm )
        {
            Firm = firm ?? throw new ArgumentNullException( nameof( firm ), @"Firm cannot be null." );

            _testedNum = _questionnaireContext.GetNextNumOfTested( firm.Id );

            Reload();
        }

        private void Reload ()
        {
            _sections = new Queue< SectionViewModel >( _questionnaireContext.GetSections().Select( s => new SectionViewModel( s ) ) );

            _isNextSectionA = true;

            if ( _sections.Any() ) {

                SectionA = _sections.Dequeue();
                SectionA.NextSectionRequested += OnNextSectionRequested;
                _isNextSectionA = false;
            }
        }


        private void AddAnswers ( IEnumerable< dynamic > answers )
        {
            if ( answers == null ) throw new ArgumentNullException( nameof( answers ), @"Answers cannot be null." );

            if ( !answers.Any() ) return;

            foreach ( var answer in answers ) {
                answer.Firm = Firm;
                answer.Num = _testedNum;
                _questionnaireContext.AddAnswer( answer );
            }
        }

        private void OnNextSectionRequested ( object obj, NextSectionRequestedEventArgs args )
        {
            AddAnswers( args.Answers );

            if ( _isNextSectionA ) {

                if ( SectionB != null ) {
                    SectionB.NextSectionRequested -= OnNextSectionRequested;
                }


                if ( _sections.Any() ) {

                    SectionA = _sections.Dequeue();
                    SectionA.NextSectionRequested += OnNextSectionRequested;

                    _isNextSectionA = false;
                    return;
                }
            }
            else {

                if ( SectionA != null ) {
                    SectionA.NextSectionRequested -= OnNextSectionRequested;
                }

                if ( _sections.Any() ) {

                    SectionB = _sections.Dequeue();
                    SectionB.NextSectionRequested += OnNextSectionRequested;

                    _isNextSectionA = true;
                    return;
                }
            }

            OnStopRequested();
        }

        private void OnStopRequested ()
        {
            _questionnaireContext.SaveChanges();
            StopRequested?.Invoke( this, EventArgs.Empty );
        }

        #endregion
    }
}
