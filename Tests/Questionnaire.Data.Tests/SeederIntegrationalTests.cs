using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Questionnaire.Data.BusinessContext;

namespace Questionnaire.Data.Tests
{
    [ TestFixture ]
    public class SeederIntegrationalTests
    {
        [ Test ]
        public void GetRegions_FileExists_Returns14Regions ()
        {
            var res = Seeder.GetRegions();

            Assert.That( 14 == res.Count() );
        }

        [ Test ]
        public void Getegions_FileDoesNotExists_ReturnEmptyRegionCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileWithRegionsName;
            Seeder.FileWithRegionsName = "";

            // Action:
            var res = Seeder.GetRegions();

            // Assert:
            Assert.That( !res.Any() );
            FileWithRegionsName = originFile;
        }

        [ Test ]
        public void GetCities_FileExists_Returns67Cities ()
        {
            var res = Seeder.GetCities();

            Assert.That( 67 == res.Count() );
        }

        [ Test ]
        public void GetCities_FileDoesNotExists_ReturnEmptyCityCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileWithCitiesName;
            Seeder.FileWithCitiesName = "";

            // Action:
            var res = Seeder.GetCities();

            // Assert:
            Assert.That( !res.Any() );
            FileWithCitiesName = originFile;
        }

        [ Test ]
        public void GetFirmTypes_FileExists_Returns10FirmTypes ()
        {
            var res = Seeder.GetFirmTypes();

            Assert.That( 10 == res.Count() );
        }

        [ Test ]
        public void GetFirmTypes_FileDoesNotExists_ReturnEmptyFirmTypeCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileWithFirmTypesName;
            Seeder.FileWithFirmTypesName = "";

            // Action:
            var res = Seeder.GetCities();

            // Assert:
            Assert.That( !res.Any() );
            FileWithFirmTypesName = originFile;
        }

        [ Test ]
        public void GetFirms_FileExists_Returns243Firms ()
        {
            var res = Seeder.GetFirms();

            Assert.That( 243 == res.Count() );
        }

        [ Test ]
        public void GetFirms_FileDoesNotExists_ReturnEmptyFirmCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileWithFirmsName;
            Seeder.FileWithFirmsName = "";

            // Action:
            var res = Seeder.GetFirms();

            // Assert:
            Assert.That( !res.Any() );
            FileWithFirmsName = originFile;
        }

    }
}
