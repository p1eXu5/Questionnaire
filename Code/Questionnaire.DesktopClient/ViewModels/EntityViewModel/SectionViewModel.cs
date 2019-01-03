using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;
using Questionnaire.MvvmBase;

namespace Questionnaire.DesktopClient.ViewModels.EntityViewModel
{
    public class SectionViewModel : ViewModel
    {
        private readonly Section _section;

        public SectionViewModel ( Section section )
        {
            _section = section ?? throw new ArgumentNullException( nameof( section ), @"Section cannot be null." ); ;

            QuestionOpenCollection = new List< QuestionOpenViewModel >( _section.QuestionOpenCollection.Select( q => new QuestionOpenViewModel( q ) ) );
        }

        public int Id => _section.Id;

        public IEnumerable< QuestionOpenViewModel > QuestionOpenCollection { get; }

    }
}
