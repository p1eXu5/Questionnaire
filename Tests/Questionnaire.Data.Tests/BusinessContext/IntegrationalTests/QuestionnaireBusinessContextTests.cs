using System.Linq;
using Agbm.Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.DataContext;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.Tests.BusinessContext.IntegrationalTests
{
    [ TestFixture ]
    public class QuestionnaireBusinessContextTests
    {
        private readonly string[] _paths = new string[8];

        
        private void SavePaths ()
        {
            _paths[ 0 ] = DataSeeder.FileNameRegions;
            DataSeeder.FileNameRegions = DataSeeder.FileNameRegions.AppendAssemblyPath();

            _paths[ 1 ] = DataSeeder.FileNameCities;
            DataSeeder.FileNameCities = DataSeeder.FileNameCities.AppendAssemblyPath();

            _paths[ 2 ] = DataSeeder.FileNameFirmTypes;
            DataSeeder.FileNameFirmTypes = DataSeeder.FileNameFirmTypes.AppendAssemblyPath();

            _paths[ 3 ] = DataSeeder.FileNameFirms;
            DataSeeder.FileNameFirms = DataSeeder.FileNameFirms.AppendAssemblyPath();
        }


        private void RestorePath ()
        {
            DataSeeder.FileNameFirms = _paths[ 3 ];
            DataSeeder.FileNameFirmTypes = _paths[ 2 ];
            DataSeeder.FileNameCities = _paths[ 1 ];
            DataSeeder.FileNameSections = _paths[ 0 ];
        }



        [ Test ]
        public void Ctor_ByDefault_CreatesDb ()
        {
            SavePaths();

            using ( var context = new QuestionnaireBusinessContext() ) {

                Assert.That( context.DbContext.Database.CanConnect );

                context.DbContext.Database.EnsureDeleted();
            }

            RestorePath();
        }


        [ Test ]
        public void GetFirms_DataSeederNotNull_SeedFirms ()
        {
            SavePaths();

            using ( var context = GetContext() ) {

                var firms = context.GetFirms();

                Assert.That( firms.Any() );
            }

            RestorePath();
        }

        [ Test ]
        public void GetRegions_ByDefault_ReturnsRegions ()
        {
            SavePaths();

            QuestionnaireBusinessContext context = null;

            try {
                context = new QuestionnaireBusinessContext( new DataSeeder() );

                var regions = context.GetRegions();

                Assert.That( regions.Any() );

            }
            finally {
                context?.DbContext.Database.EnsureDeleted();
            }

            RestorePath();
        }

        [ Test ]
        public void GetCities_ByDefault_ReturnsCities ()
        {
            SavePaths();

            QuestionnaireBusinessContext context = null;

            try {
                context = new QuestionnaireBusinessContext( new DataSeeder() );

                var cities = context.GetCities();

                Assert.That( cities.Any() );

            }
            finally {
                context?.DbContext.Database.EnsureDeleted();
            }

            RestorePath();
        }


        [ Test ]
        public void AddRegions_RegionsWithFilledId_AddRegions ()
        {
            using ( var context = new QuestionnaireBusinessContext() ) {

                var region = new Region { Id = 9999, Name = "asda" };

                var regions = new[] {
                    region
                };

                context.AddRegions( regions );
                context.DeleteRegion( region );
            }
        }



        private QuestionnaireBusinessContext GetContext ( bool seed = true )
        {
            var options = new DbContextOptionsBuilder< QuestionnaireDbContext >()
                          .UseInMemoryDatabase( databaseName: "TestDb" )
                          .Options;

            return seed ? new QuestionnaireBusinessContext( options, new TestDataSeeder() )
                        : new QuestionnaireBusinessContext( options, null );
        }
    }
}
