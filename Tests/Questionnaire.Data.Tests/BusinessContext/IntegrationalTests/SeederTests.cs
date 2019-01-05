using System.Linq;
using Helpers;
using NUnit.Framework;
using Questionnaire.Data.BusinessContext;

namespace Questionnaire.Data.Tests.BusinessContext.IntegrationalTests
{
    [ TestFixture ]
    public class SeederTests
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


        [ Test ]
        public void GetCategories_FileExists_Returns3Categories ()
        {
            var oldFileName = Seeder.FileNameCategories;
            Seeder.FileNameCategories = Seeder.FileNameCategories.AppendAssemblyPath();

            var res = Seeder.GetCategories().ToList();

            Assert.That( 3 == res.Count, $"res.Count: { res.Count }"  );
            Seeder.FileNameCategories = oldFileName;
        }

        [ Test ]
        public void GetCategories_FileDoesNotExists_ReturnEmptyCategoryCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileNameCategories;
            Seeder.FileNameCategories = "";

            // Action:
            var res = Seeder.GetCategories();

            // Assert:
            Assert.That( !res.Any() );
            Seeder.FileNameCategories = originFile;
        }

        [ Test ]
        public void GetSections_FileExists_Returns8Sections ()
        {
            var oldFileName = Seeder.FileNameSections;
            Seeder.FileNameSections = Seeder.FileNameSections.AppendAssemblyPath();

            var res = Seeder.GetSections().ToList();

            Assert.That( 8 == res.Count, $"res.Count: { res.Count }"  );
            Seeder.FileNameSections = oldFileName;
        }

        [ Test ]
        public void GetSections_FileDoesNotExists_ReturnEmptySectionCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileNameSections;
            Seeder.FileNameSections = "";

            // Action:
            var res = Seeder.GetSections();

            // Assert:
            Assert.That( !res.Any() );
            Seeder.FileNameSections = originFile;
        }

        [ Test ]
        public void GetMultipleChoiceQuestions_FileExists_Returns48Questions ()
        {
            var oldFileName = Seeder.FileNameQuestionMultipleChoiceList;
            Seeder.FileNameQuestionMultipleChoiceList = Seeder.FileNameQuestionMultipleChoiceList.AppendAssemblyPath();

            var res = Seeder.GetMultipleChoiceQuestions().ToList();

            Assert.That( 48 == res.Count, $"res.Count: { res.Count }"  );
            Seeder.FileNameQuestionMultipleChoiceList = oldFileName;
        }

        [ Test ]
        public void GetMultipleChoiceQuestions_FileDoesNotExists_ReturnEmptyQuestionCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileNameQuestionMultipleChoiceList;
            Seeder.FileNameQuestionMultipleChoiceList = "";

            // Action:
            var res = Seeder.GetMultipleChoiceQuestions();

            // Assert:
            Assert.That( !res.Any() );
            Seeder.FileNameQuestionMultipleChoiceList = originFile;
        }

        [ Test ]
        public void GetOpenQuestions_FileExists_Returns9Questions ()
        {
            var oldFileName = Seeder.FileNameQuestionOpenList;
            Seeder.FileNameQuestionOpenList = Seeder.FileNameQuestionOpenList.AppendAssemblyPath();

            var res = Seeder.GetOpenQuestions().ToList();

            Assert.That( 9 == res.Count, $"res.Count: { res.Count }"  );
            Seeder.FileNameQuestionOpenList = oldFileName;
        }

        [ Test ]
        public void GetOpenQuestions_FileDoesNotExists_ReturnEmptyQuestionCollection ()
        {
            // Arrange:
            var originFile = Seeder.FileNameQuestionOpenList;
            Seeder.FileNameQuestionOpenList = "";

            // Action:
            var res = Seeder.GetOpenQuestions();

            // Assert:
            Assert.That( !res.Any() );
            Seeder.FileNameQuestionOpenList = originFile;
        }
    }
}
