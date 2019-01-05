using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            int i = 1;

            QuestionOpenCollection = new List< QuestionOpenViewModel >( _section.QuestionOpenCollection
                                                                                .Select( q => new QuestionOpenViewModel( q, i++ ) ) );

            QuestionMultipleChoiceCollection = new List< QuestionMiltipleChoiceViewModel >( _section.QuestionMultipleChoiceCollection
                                                                                                    .Select( q => new QuestionMiltipleChoiceViewModel( q, i++ ) ) ) ;

            NextSectionCommand = new MvvmCommand( NextSection, CanMoveNextSection );
        }

        #endregion



        #region Events

        public event EventHandler< NextSectionRequestedEventArgs > NextSectionRequested;

        #endregion



        #region Properties

        public bool IsStageA { get; set; }

        public int Id => _section.Id;

        public IEnumerable< QuestionOpenViewModel > QuestionOpenCollection { get; }
        public IEnumerable< QuestionMiltipleChoiceViewModel > QuestionMultipleChoiceCollection { get; }

        #endregion



        #region Commands

        public ICommand NextSectionCommand { get; }

        #endregion



        #region Methods

        private void NextSection ( object obj )
        {
            OnNextSectionRequested();
        }

        private bool CanMoveNextSection ( object obj )
        {
            return QuestionOpenCollection.All( q => q.IsAnswered )
                   && QuestionMultipleChoiceCollection.All( q => q.IsAnswered );
        }

        protected virtual void OnNextSectionRequested ()
        {
            var answers = new List< dynamic >( QuestionOpenCollection.Select( q => q.AnswerOpen ) );
            answers.AddRange( QuestionMultipleChoiceCollection.Select( q => q.AnswerMultipleChoice ) );

            NextSectionRequested?.Invoke( this, new NextSectionRequestedEventArgs( answers ) );
        }

        #endregion
    }
}
