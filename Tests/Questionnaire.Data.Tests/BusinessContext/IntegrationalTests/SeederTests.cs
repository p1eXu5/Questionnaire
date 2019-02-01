using System.Linq;
using Agbm.Helpers.Extensions;
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
            var oldFileName = DataSeeder.FileNameRegions;
            DataSeeder.FileNameRegions = DataSeeder.FileNameRegions.AppendAssemblyPath();

            // Action:
            var res = DataSeeder.GetRegions().ToList();

            // Assert:
            Assert.That( 14 == res.Count, $"res.Count: { res.Count }" );

            // Clean:
            DataSeeder.FileNameRegions = oldFileName;
        }

        [Test]
        public void Getegions_FileDoesNotExists_ReturnEmptyRegionCollection ()
        {
            // Arrange:
            var originFile = DataSeeder.FileNameRegions;
            DataSeeder.FileNameRegions = "";

            // Action:
            var res = DataSeeder.GetRegions();

            // Assert:
            Assert.That( !res.Any() );
            DataSeeder.FileNameRegions = originFile;
        }


        [Test]
        public void GetCities_FileExists_Returns4Cities ()
        {
            var oldFileName = DataSeeder.FileNameCities;
            DataSeeder.FileNameCities = DataSeeder.FileNameCities.AppendAssemblyPath();

            var res = DataSeeder.GetCities().ToList();

            Assert.That( 5 == res.Count, $"res.Count: { res.Count }" );
            DataSeeder.FileNameCities = oldFileName;
        }

        [Test]
        public void GetCities_FileDoesNotExists_ReturnEmptyCityCollection ()
        {
            // Arrange:
            var originFile = DataSeeder.FileNameCities;
            DataSeeder.FileNameCities = "";

            // Action:
            var res = DataSeeder.GetCities();

            // Assert:
            Assert.That( !res.Any() );
            DataSeeder.FileNameCities = originFile;
        }


        [ Test ]
        public void GetFirmTypes_FileExists_Returns10FirmTypes ()
        {
            var oldFileName = DataSeeder.FileNameFirmTypes;
            DataSeeder.FileNameFirmTypes = DataSeeder.FileNameFirmTypes.AppendAssemblyPath();

            var res = DataSeeder.GetFirmTypes().ToList();

            Assert.That( 10 == res.Count, $"res.Count: { res.Count }"   );
            DataSeeder.FileNameFirmTypes = oldFileName;
        }

        [ Test ]
        public void GetFirmTypes_FileDoesNotExists_ReturnEmptyFirmTypeCollection ()
        {
            // Arrange:
            var originFile = DataSeeder.FileNameFirmTypes;
            DataSeeder.FileNameFirmTypes = "";

            // Action:
            var res = DataSeeder.GetFirmTypes();

            // Assert:
            Assert.That( !res.Any() );
            DataSeeder.FileNameFirmTypes = originFile;
        }


        [ Test ]
        public void GetFirms_FileExists_Returns4Firms ()
        {
            var oldFileName = DataSeeder.FileNameFirms;
            DataSeeder.FileNameFirms = DataSeeder.FileNameFirms.AppendAssemblyPath();

            var res = DataSeeder.GetFirms().ToList();

            Assert.That( 4 == res.Count, $"res.Count: { res.Count }"  );
            DataSeeder.FileNameFirms = oldFileName;
        }

        [ Test ]
        public void GetFirms_FileDoesNotExists_ReturnEmptyFirmCollection ()
        {
            // Arrange:
            var originFile = DataSeeder.FileNameFirms;
            DataSeeder.FileNameFirms = "";

            // Action:
            var res = DataSeeder.GetFirms();

            // Assert:
            Assert.That( !res.Any() );
            DataSeeder.FileNameFirms = originFile;
        }


        [ Test ]
        public void GetCategories_FileExists_Returns3Categories ()
        {
            var oldFileName = DataSeeder.FileNameCategories;
            DataSeeder.FileNameCategories = DataSeeder.FileNameCategories.AppendAssemblyPath();

            var res = DataSeeder.GetCategories().ToList();

            Assert.That( 3 == res.Count, $"res.Count: { res.Count }"  );
            DataSeeder.FileNameCategories = oldFileName;
        }

        [ Test ]
        public void GetCategories_FileDoesNotExists_ReturnEmptyCategoryCollection ()
        {
            // Arrange:
            var originFile = DataSeeder.FileNameCategories;
            DataSeeder.FileNameCategories = "";

            // Action:
            var res = DataSeeder.GetCategories();

            // Assert:
            Assert.That( !res.Any() );
            DataSeeder.FileNameCategories = originFile;
        }

        [ Test ]
        public void GetSections_FileExists_Returns8Sections ()
        {
            var oldFileName = DataSeeder.FileNameSections;
            DataSeeder.FileNameSections = DataSeeder.FileNameSections.AppendAssemblyPath();

            var res = DataSeeder.GetSections().ToList();

            Assert.That( 8 == res.Count, $"res.Count: { res.Count }"  );
            DataSeeder.FileNameSections = oldFileName;
        }

        [ Test ]
        public void GetSections_FileDoesNotExists_ReturnEmptySectionCollection ()
        {
            // Arrange:
            var originFile = DataSeeder.FileNameSections;
            DataSeeder.FileNameSections = "";

            // Action:
            var res = DataSeeder.GetSections();

            // Assert:
            Assert.That( !res.Any() );
            DataSeeder.FileNameSections = originFile;
        }

        [ Test ]
        public void GetMultipleChoiceQuestions_FileExists_Returns48Questions ()
        {
            var oldFileName = DataSeeder.FileNameQuestionMultipleChoiceList;
            DataSeeder.FileNameQuestionMultipleChoiceList = DataSeeder.FileNameQuestionMultipleChoiceList.AppendAssemblyPath();

            var res = DataSeeder.GetMultipleChoiceQuestions().ToList();

            Assert.That( 48 == res.Count, $"res.Count: { res.Count }"  );
            DataSeeder.FileNameQuestionMultipleChoiceList = oldFileName;
        }

        [ Test ]
        public void GetMultipleChoiceQuestions_FileDoesNotExists_ReturnEmptyQuestionCollection ()
        {
            // Arrange:
            var originFile = DataSeeder.FileNameQuestionMultipleChoiceList;
            DataSeeder.FileNameQuestionMultipleChoiceList = "";

            // Action:
            var res = DataSeeder.GetMultipleChoiceQuestions();

            // Assert:
            Assert.That( !res.Any() );
            DataSeeder.FileNameQuestionMultipleChoiceList = originFile;
        }

        [ Test ]
        public void GetOpenQuestions_FileExists_Returns9Questions ()
        {
            var oldFileName = DataSeeder.FileNameQuestionOpenList;
            DataSeeder.FileNameQuestionOpenList = DataSeeder.FileNameQuestionOpenList.AppendAssemblyPath();

            var res = DataSeeder.GetOpenQuestions().ToList();

            Assert.That( 9 == res.Count, $"res.Count: { res.Count }"  );
            DataSeeder.FileNameQuestionOpenList = oldFileName;
        }

        [ Test ]
        public void GetOpenQuestions_FileDoesNotExists_ReturnEmptyQuestionCollection ()
        {
            // Arrange:
            var originFile = DataSeeder.FileNameQuestionOpenList;
            DataSeeder.FileNameQuestionOpenList = "";

            // Action:
            var res = DataSeeder.GetOpenQuestions();

            // Assert:
            Assert.That( !res.Any() );
            DataSeeder.FileNameQuestionOpenList = originFile;
        }
    }
}
