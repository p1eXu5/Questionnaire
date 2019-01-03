using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Questionnaire.Data.BusinessContext;

namespace Questionnaire.Data.Tests.BusinessContext.IntegrationalTests
{
    [TestFixture]
    public class QuestionnaireBusinessContextTests
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

            Assert.That( firms.All( f => f.City != null && f.CityId > 0 ) );
        }

        [ Test ]
        public void Ctor_ByDefault_LoadsFirmsWithFirmTypes ()
        {
            var context = GetQuestionnaireBusinessContext();

            var firms = context.GetFirms();

            Assert.That( firms.All( f => f.FirmType != null && f.FirmTypeId > 0 ) );
        }


        #region Factory

        private IQuestionnaireBusinessContext GetQuestionnaireBusinessContext ()
        {
            return new QuestionnaireBusinessContext();
        }
        #endregion
    }
}
