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

            Questions = new List< QuestionViewModel >( _section.Questions.Select( q => new QuestionViewModel( q ) ) );
        }

        public int Id => _section.Id;

        public IEnumerable< QuestionViewModel > Questions { get; }

        public string 
    }
}
