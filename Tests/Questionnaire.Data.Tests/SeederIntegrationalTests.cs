using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using NUnit.Framework;
using Questionnaire.Data.BusinessContext;

namespace Questionnaire.Data.Tests
{
    [ TestFixture ]
    public class SeederIntegrationalTests
    {
        [Test]
        public void GetRegions_FileExists_Returns14Regions ()
        {
            // Arrange:
            var oldFileName = Seeder.FileNameRegions;
            Seeder.FileNameRegions = Seeder.FileNameRegions.AppendAssemblyPath();

            // Action:
            var res = Seeder.GetRegions().ToList();

            // Assert:
            Assert.That( 14 == res.Count, $"res.Count: { res.Count }" );

            // Clean:
            Seeder.FileNameRegions = oldFileName;
        }

        [Test]
        public void Getegions_FileDoesNotExists_ReturnEmptyRegionCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileNameRegions;
            Seeder.FileNameRegions = "";

            // Action:
            var res = Seeder.GetRegions();

            // Assert:
            Assert.That( !res.Any() );
            Seeder.FileNameRegions = originFile;
        }

        [Test]
        public void GetCities_FileExists_Returns4Cities ()
        {
            var oldFileName = Seeder.FileNameCities;
            Seeder.FileNameCities = Seeder.FileNameCities.AppendAssemblyPath();

            var res = Seeder.GetCities().ToList();

            Assert.That( 5 == res.Count, $"res.Count: { res.Count }" );
            Seeder.FileNameCities = oldFileName;
        }

        [Test]
        public void GetCities_FileDoesNotExists_ReturnEmptyCityCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileNameCities;
            Seeder.FileNameCities = "";

            // Action:
            var res = Seeder.GetCities();

            // Assert:
            Assert.That( !res.Any() );
            Seeder.FileNameCities = originFile;
        }

        [ Test ]
        public void GetFirmTypes_FileExists_Returns10FirmTypes ()
        {
            var oldFileName = Seeder.FileNameFirmTypes;
            Seeder.FileNameFirmTypes = Seeder.FileNameFirmTypes.AppendAssemblyPath();

            var res = Seeder.GetFirmTypes().ToList();

            Assert.That( 10 == res.Count, $"res.Count: { res.Count }"   );
            Seeder.FileNameFirmTypes = oldFileName;
        }

        [ Test ]
        public void GetFirmTypes_FileDoesNotExists_ReturnEmptyFirmTypeCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileNameFirmTypes;
            Seeder.FileNameFirmTypes = "";

            // Action:
            var res = Seeder.GetFirmTypes();

            // Assert:
            Assert.That( !res.Any() );
            Seeder.FileNameFirmTypes = originFile;
        }

        [ Test ]
        public void GetFirms_FileExists_Returns4Firms ()
        {
            var oldFileName = Seeder.FileNameFirms;
            Seeder.FileNameFirms = Seeder.FileNameFirms.AppendAssemblyPath();

            var res = Seeder.GetFirms().ToList();

            Assert.That( 4 == res.Count, $"res.Count: { res.Count }"  );
            Seeder.FileNameFirms = oldFileName;
        }

        [ Test ]
        public void GetFirms_FileDoesNotExists_ReturnEmptyFirmCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileNameFirms;
            Seeder.FileNameFirms = "";

            // Action:
            var res = Seeder.GetFirms();

            // Assert:
            Assert.That( !res.Any() );
            Seeder.FileNameFirms = originFile;
        }

    }
}
