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
        private readonly IQuestionnaire _businessContext;

        private Queue< SectionViewModel > _sections;

        private SectionViewModel _sectionA;
        private SectionViewModel _sectionB;

        private bool _isNextSectionA;
        private int _testedNum;

        public QuestionnaireRunnerViewModel ( IQuestionnaire businessContext )
        {
            _businessContext = businessContext ?? throw new ArgumentNullException( nameof( businessContext ), @"IQuestionnaire cannot be null." );

            _sections = new Queue< SectionViewModel >();
        }

        public event EventHandler< EventArgs > StopRequested; 

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

        public Firm Firm { get; private set; }

        public void SetFirm ( Firm firm )
        {
            Firm = firm ?? throw new ArgumentNullException( nameof( firm ), @"Firm cannot be null." );

            _testedNum = _businessContext.GetOpenAnswers().Where( a => a.FirmId == Firm.Id ).Max( a => a.Num ) + 1;
        }

        public void Reload ()
        {
            _sections = new Queue< SectionViewModel >( _businessContext.GetSections().Select( s => new SectionViewModel( s ) ) );

            _isNextSectionA = true;

            if ( _sections.Any() ) {

                SectionA = _sections.Dequeue();
                _isNextSectionA = false;
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

            return;
        }


        private void AddAnswers ( IEnumerable< dynamic > answers )
        {
            if ( answers == null ) throw new ArgumentNullException( nameof( answers ), @"Answers cannot be null." );

            if ( !answers.Any() ) return;

            foreach ( var answer in answers ) {
                answer.Firm = Firm;
                answer.Num = _testedNum;
                _businessContext.AddAnswer( answer );
            }
        }

        private void OnStopRequested ()
        {
            _businessContext.SaveChanges();
            StopRequested?.Invoke( this, EventArgs.Empty );
        }
    }
}
