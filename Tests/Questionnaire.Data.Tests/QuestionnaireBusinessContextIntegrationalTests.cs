using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Questionnaire.Data.BusinessContext;

namespace Questionnaire.Data.Tests
{
    [TestFixture]
    public class QuestionnaireBusinessContextIntegrationalTests
    {
        [ Test ]
        public void Ctor_ByDefault_LoadsCities ()
        {
            var context = GetQuestionnaireBusinessContext();

            var cities = context.GetCities();

            Assert.That( cities.Any() );
        }

        [ Test ]
        public void Ctor_ByDefault_LoadsCitiesWithRegions ()
        {
            var context = GetQuestionnaireBusinessContext();

            var cities = context.GetCities();

            Assert.That( cities.First().Region != null );
        }

        [ Test ]
        public void Ctor_ByDefault_LoadsFirms ()
        {
            var context = GetQuestionnaireBusinessContext();

            var firms = context.GetFirms();

            Assert.That( firms.Any() );
        }

        [ Test ]
        public void Ctor_ByDefault_LoadsFirmsWithCities ()
        {
            var context = GetQuestionnaireBusinessContext();

            var firms = context.GetFirms();

            Assert.That( firms.First().City != null );
        }

        [ Test ]
        public void Ctor_ByDefault_LoadsFirmsWithFirmTypes ()
        {
            var context = GetQuestionnaireBusinessContext();

            var firms = context.GetFirms();

            Assert.That( firms.First().FirmType != null );
        }


        #region Factory

        private IQuestionnaireBusinessContext GetQuestionnaireBusinessContext ()
        {
            return new QuestionnaireBusinessContext();
        }
        #endregion
    }
}
