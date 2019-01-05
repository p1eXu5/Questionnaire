using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Questionnaire.Data.DataContext;

namespace Questionnaire.Data.BusinessContext
{
    public class MainDataSeeder : IDataSeeder
    {
        public void SeedData ( QuestionnaireDbContext context )
        {
            foreach ( var region in Seeder.GetRegions() ) {
                context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Regions ON; INSERT INTO dbo.Regions ( [Id], [Name] ) VALUES ( { region.Id }, '{ region.Name }' )" );
            }

            foreach ( var city in Seeder.GetCities() ) {
                context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Cities ON; INSERT INTO dbo.Cities ( [Id], [Name], [RegionId] ) VALUES ( { city.Id }, '{ city.Name }', { city.RegionId } )" );
            }
            
            foreach ( var firmType in Seeder.GetFirmTypes() ) {
                context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.FirmTypes ON; INSERT INTO dbo.FirmTypes ( [Id], [Name] ) VALUES ( { firmType.Id }, '{ firmType.Name }' )" );
            }

            context.Firms.AddRange( Seeder.GetFirms() );
            context.SaveChanges();

            foreach ( var category in Seeder.GetCategories() ) {
                context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Categories ON; INSERT INTO dbo.Categories ( [Id], [Name] ) VALUES ( { category.Id }, '{ category.Name }' )" );
            }

            foreach ( var section in Seeder.GetSections() ) {
                context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Sections ON; INSERT INTO dbo.Sections ( [Id], [Name], [CategoryId] ) VALUES ( { section.Id }, '{ section.Name }', { section.CategoryId } )" );
            }

            foreach ( var questionMultiple in Seeder.GetMultipleChoiceQuestions() ) {

                context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.MultipleChoiceQuestions ON; " +
                                                     $"INSERT INTO dbo.MultipleChoiceQuestions ( [Id], [Name], [CategoryId] ) " +
                                                     $"VALUES ( { questionMultiple.Id }, '{ questionMultiple.Text }', { questionMultiple.SectionId } )" );
            }

            foreach ( var questionOpen in Seeder.GetOpenQuestions() ) {

                context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.OpenQuestions ON; " +
                                                    $"INSERT INTO dbo.OpenQuestions ( [Id], [Name], [CategoryId] ) " +
                                                     $"VALUES ( { questionOpen.Id }, '{ questionOpen.Text }', { questionOpen.SectionId } )" );
            }

        }
    }
}
