using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Questionnaire.Data.Models;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels.EntityViewModel
{
    public class SectionViewModel : ViewModel, IStageViewModel
    {
        #region Fields

        private readonly Section _section;

        #endregion



        #region Ctor

        public SectionViewModel ( Section section )
        {
            _section = section ?? throw new ArgumentNullException( nameof( section ), @"Section cannot be null." );

            if ( section.QuestionMultipleChoiceCollection.Count <= 0 
                 && section.QuestionOpenCollection.Count <= 0 ) throw new ArgumentException("Section has no Questions.");

            int i = 1;

            QuestionOpenCollection = new List< QuestionOpenViewModel >( 
                _section.QuestionOpenCollection
                        .Select( q => 
                        {
                            var question = new QuestionOpenViewModel( q, i++ );
                            question.PropertyChanged += OnIsAnsweredChanged;
                            return question;
                        } ) 
            );


            QuestionMultipleChoiceCollection = new List< QuestionMultipleChoiceViewModel >( 
                _section.QuestionMultipleChoiceCollection
                        .Select( q =>
                        {
                            var question = new QuestionMultipleChoiceViewModel( q, i++ );
                            question.PropertyChanged += OnIsAnsweredChanged;
                            return question;
                        } ) 
            ) ;

            NextSectionCommand = new MvvmCommand( NextSection, CanMoveNextSection );
        }

        #endregion



        #region Events

        public event EventHandler< NextSectionRequestedEventArgs > NextSectionRequested;

        #endregion



        #region Properties

        public bool IsStageA { get; set; }

        public int Id => _section.Id;

        public IEnumerable< QuestionMultipleChoiceViewModel > QuestionMultipleChoiceCollection { get; }
        public IEnumerable< QuestionOpenViewModel > QuestionOpenCollection { get; }

        #endregion



        #region Commands

        public ICommand NextSectionCommand { get; }

        #endregion



        #region Methods

        private void OnIsAnsweredChanged ( object sender, PropertyChangedEventArgs args )
        {
            if ( args.PropertyName == "IsAnswered" ) {
                ((MvvmCommand)NextSectionCommand).RaiseCanExecuteChanged();
            }
        }

        private void NextSection ( object obj )
        {
            OnNextSectionRequested();
        }

        private bool CanMoveNextSection ( object obj )
        {
            return QuestionOpenCollection.All( q => q.IsAnswered )
                   && QuestionMultipleChoiceCollection.All( q => q.IsAnswered );
        }

        private void OnNextSectionRequested ()
        {
            var answers = new List< dynamic >( QuestionOpenCollection.Select( q => q.AnswerOpen ) );
            answers.AddRange( QuestionMultipleChoiceCollection.Select( q => q.AnswerMultipleChoice ) );

            NextSectionRequested?.Invoke( this, new NextSectionRequestedEventArgs( answers ) );
        }

        #endregion
    }
}
