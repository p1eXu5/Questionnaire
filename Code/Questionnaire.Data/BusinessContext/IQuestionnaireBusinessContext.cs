using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public interface IQuestionnaireBusinessContext
    {
        Region[] GetRegions ();
        Firm[] GetFirms ();
        City[] GetCities ();
        Section[] GetSections ();
    }
}
