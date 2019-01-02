﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public interface IQuestionnaireBusinessContext
    {
        IEnumerable< Firm > GetFirms ();
        IEnumerable< City > GetCities ();
    }
}
