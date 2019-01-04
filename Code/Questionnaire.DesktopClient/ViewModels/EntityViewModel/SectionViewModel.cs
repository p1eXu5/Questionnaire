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
        private readonly Section _section;

        public SectionViewModel ( Section section )
        {
            _section = section ?? throw new ArgumentNullException( nameof( section ), @"Section cannot be null." );

            QuestionOpenCollection = new List< QuestionOpenViewModel >( _section.QuestionOpenCollection
                                                                                .Select( q => new QuestionOpenViewModel( q ) ) );

            QuestionMultipleChoiceCollection = new List< QuestionMiltipleChoiceViewModel >( _section.QuestionMultipleChoiceCollection
                                                                                                    .Select( q => new QuestionMiltipleChoiceViewModel( q ) ) ) ;

            NextSectionCommand = new MvvmCommand( NextSection, CanMoveNextSection );
        }

        public event EventHandler< NextSectionRequested > NextSectionRequested; 

        public int Id => _section.Id;

        public IEnumerable< QuestionOpenViewModel > QuestionOpenCollection { get; }
        public IEnumerable< QuestionMiltipleChoiceViewModel > QuestionMultipleChoiceCollection { get; }

        public bool IsStageA { get; set; }


        public ICommand NextSectionCommand { get; }


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

            NextSectionRequested?.Invoke( this, new NextSectionRequested( answers ) );
        }


    }
}
