using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.DataContext;

namespace Questionnaire.Data.Tests.BusinessContext.IntegrationalTests
{
    [ TestFixture ]
    public class QuestionnaireBusinessContextTests
    {
        private string[] _paths = new string[8];

        [ SetUp ]
        public void SavePaths ()
        {
            _paths[ 0 ] = Seeder.FileNameRegions;
            Seeder.FileNameRegions = Seeder.FileNameRegions.AppendAssemblyPath();

            _paths[ 1 ] = Seeder.FileNameCities;
            Seeder.FileNameCities = Seeder.FileNameCities.AppendAssemblyPath();

            _paths[ 2 ] = Seeder.FileNameFirmTypes;
            Seeder.FileNameFirmTypes = Seeder.FileNameFirmTypes.AppendAssemblyPath();

            _paths[ 3 ] = Seeder.FileNameFirms;
            Seeder.FileNameFirms = Seeder.FileNameFirms.AppendAssemblyPath();
        }

        [ TearDown ]
        public void RestorePath ()
        {
            Seeder.FileNameFirms = _paths[ 3 ];
            Seeder.FileNameFirmTypes = _paths[ 2 ];
            Seeder.FileNameCities = _paths[ 1 ];
            Seeder.FileNameSections = _paths[ 0 ];
        }

        [ Test ]
        public void Ctor_ByDefault_CreatesDb ()
        {
            using ( var context = new QuestionnaireBusinessContext() ) {

                Assert.That( context.DbContext.Database.CanConnect );

                context.DbContext.Database.EnsureDeleted();
            }
        }

        [ Test ]
        public void GetFirms_UnitByDefault_ReturnsFirms ()
        {
            using ( var context = GetContext() ) {

                var firms = context.GetFirms();

                Assert.That( firms.Any() );
            }
        }

        [ Test ]
        public void GetFirms_ByDefault_ReturnsRegions ()
        {
            QuestionnaireBusinessContext context = null;

            using ( context = new QuestionnaireBusinessContext() ) {

                var regions = context.GetRegions();

                Assert.That( regions.Any() );

            }

            context.DbContext.Database.EnsureDeleted();
        }

        private QuestionnaireBusinessContext GetContext ()
        {
            var options = new DbContextOptionsBuilder< QuestionnaireDbContext >()
                          .UseInMemoryDatabase( databaseName: "TestDb" )
                          .Options;

            return new QuestionnaireBusinessContext( options );
        }
    }
}
