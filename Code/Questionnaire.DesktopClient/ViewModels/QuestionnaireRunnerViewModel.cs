﻿using System;
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

        private readonly IQuestionnaireBusinessContext _questionnaireContext;

        private Queue< SectionViewModel > _sections;

        private SectionViewModel _sectionA;
        private SectionViewModel _sectionB;

        private bool _isNextSectionA;
        private int _employeeNum;

        #endregion



        #region Ctor

        public QuestionnaireRunnerViewModel ( IQuestionnaireBusinessContext questionnairContext )
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

        public int EmployeeNum => _employeeNum;

        public bool IsNextSectionA
        {
            get => _isNextSectionA;
            set {
                _isNextSectionA = value;
                OnPropertyChanged();
            }
        }

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

            _employeeNum = _questionnaireContext.GetNextNumOfTested( firm.Id );

            Reload();
        }

        private void Reload ()
        {
            _sections = new Queue< SectionViewModel >( _questionnaireContext.GetSections().Select( s => new SectionViewModel( s ) ) );

            if ( _sections.Any() ) {
                SectionB = null;
                SectionA = _sections.Dequeue();
                SectionA.NextSectionRequested += OnNextSectionRequested;
                IsNextSectionA = false;
            }
            else {
                throw new ArgumentException( "Sections have not been returned from db." );
            }
        }


        private void AddAnswers ( IEnumerable< dynamic > answers )
        {
            if ( answers == null ) throw new ArgumentNullException( nameof( answers ), @"Answers cannot be null." );

            if ( !answers.Any() ) return;

            foreach ( var answer in answers ) {
                answer.Firm = Firm;
                answer.Num = _employeeNum;
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

                    IsNextSectionA = false;
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

                    IsNextSectionA = true;
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
