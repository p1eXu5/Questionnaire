using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Questionnaire.Data.Models;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels.EntityViewModel
{
    public class SectionViewModel : ViewModel
    {
        #region Fields

        private readonly Section _section;
        private ObservableCollection< QuestionMultipleChoiceViewModel > _questionMultipleChoiceVmCollection;
        private ObservableCollection< QuestionOpenViewModel > _questionOpenVmCollection; 

        #endregion



        #region Ctor

        public SectionViewModel ( Section section )
        {
            _section = section ?? throw new ArgumentNullException( nameof( section ), @"Section cannot be null." );

            if ( section.QuestionMultipleChoiceCollection.Count <= 0 
                 && section.QuestionOpenCollection.Count <= 0 ) throw new ArgumentException("Section has no Questions.");

            int i = 0;

            _questionMultipleChoiceVmCollection = new ObservableCollection< QuestionMultipleChoiceViewModel >( 
                _section.QuestionMultipleChoiceCollection
                        .Select( q =>
                        {
                            var question = new QuestionMultipleChoiceViewModel( q, ++i );
                            question.PropertyChanged += OnIsAnsweredChanged;
                            return question;
                        } ) 
            ) ;

            QuestionMultipleChoiceVmCollection =
                new ReadOnlyObservableCollection< QuestionMultipleChoiceViewModel
                >( _questionMultipleChoiceVmCollection );


            _questionOpenVmCollection = new ObservableCollection< QuestionOpenViewModel >( 
                _section.QuestionOpenCollection
                        .Select( q => 
                        {
                            var question = new QuestionOpenViewModel( q, ++i );
                            question.PropertyChanged += OnIsAnsweredChanged;
                            return question;
                        } ) 
            );

            QuestionOpenVmCollection = new ReadOnlyObservableCollection< QuestionOpenViewModel >( _questionOpenVmCollection );
            NextSectionCommand = new MvvmCommand( NextSection, CanMoveNextSection );
        }

        #endregion



        #region Events

        public event EventHandler< NextSectionRequestedEventArgs > NextSectionRequested;

        #endregion



        #region Properties

        public int Id => _section.Id;
        public string Name => _section.Name;

        public ReadOnlyObservableCollection< QuestionMultipleChoiceViewModel > QuestionMultipleChoiceVmCollection { get; }
        public ReadOnlyObservableCollection< QuestionOpenViewModel > QuestionOpenVmCollection { get; }

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
            return QuestionOpenVmCollection.All( q => q.IsAnswered )
                   && QuestionMultipleChoiceVmCollection.All( q => q.IsAnswered );
        }

        private void OnNextSectionRequested ()
        {
            var answers = new List< dynamic >( QuestionOpenVmCollection.Select( q => q.AnswerOpen ) );
            answers.AddRange( QuestionMultipleChoiceVmCollection.Select( q => q.AnswerMultipleChoice ) );

            NextSectionRequested?.Invoke( this, new NextSectionRequestedEventArgs( answers ) );
        }

        #endregion
    }
}
