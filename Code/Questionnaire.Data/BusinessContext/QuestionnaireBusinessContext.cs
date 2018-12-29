using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public class QuestionnaireBusinessContext
    {
        private readonly List< Firm > _firms;
        private readonly List< Note > _notes;
        private readonly List< Question > _questions;
        private readonly List< Section > _sections;
        private readonly List< Answer > _answers;

        public QuestionnaireBusinessContext ()
        {
            
        }
    }
}
