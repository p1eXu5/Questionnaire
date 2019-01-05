using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.DataContext;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.Tests.BusinessContext
{
    public class TestDataSeeder : IDataSeeder
    {
        public void SeedData ( QuestionnaireDbContext context )
        {
            context.Regions.AddRange( new [] {
                new Region { Name = "Test Region # 1" },
                new Region { Name = "Test Region # 2" },
            });
            context.SaveChanges();

            context.Cities.AddRange(new [] {
                new City { Name = "Test City # 1", RegionId = 1 },
                new City { Name = "Test City # 2", RegionId = 1 },
                new City { Name = "Test City # 3", RegionId = 2 },
                new City { Name = "Test City # 4", RegionId = 2 },
            });
            context.SaveChanges();

            context.FirmTypes.AddRange(new [] {
                new FirmType { Name = "Test FirmType" },
            });
            context.SaveChanges();

            context.Firms.AddRange(new [] {
                new Firm { Id=12568, Name = "Test Firm # 1", CityId = 1, FirmTypeId = 1 },
                new Firm { Id=56910, Name = "Test Firm # 2", CityId = 2, FirmTypeId = 1 },
            });
            context.SaveChanges();

            context.Categories.AddRange(new [] {
                new Category { Name = "Test Category # 1" },
                new Category { Name = "Test Category # 2" },
                new Category { Name = "Test Category # 3" },
            });
            context.SaveChanges();

            context.Sections.AddRange(new [] {
                new Section { Name = "Test Section # 1", CategoryId = 1 },
                new Section { Name = "Test Section # 2", CategoryId = 1 },
                new Section { Name = "Test Section # 3", CategoryId = 2 },
                new Section { Name = "Test Section # 4", CategoryId = 3 },
                new Section { Name = "Test Section # 5", CategoryId = 3 },
            });
            context.SaveChanges();

            context.MultipleChoiceQuestions.AddRange(new [] {
                new QuestionMultipleChoice { Text = "Test Question # 1", SectionId = 1 },
                new QuestionMultipleChoice { Text = "Test Question # 2", SectionId = 2 },
                new QuestionMultipleChoice { Text = "Test Question # 3", SectionId = 3 },
                new QuestionMultipleChoice { Text = "Test Question # 4", SectionId = 4 },
            });
            context.SaveChanges();

            context.OpenQuestions.AddRange( new [] {
                new QuestionOpen { Text = "Test OpenQuestion # 1", SectionId = 1 }, 
                new QuestionOpen { Text = "Test OpenQuestion # 5", SectionId = 5 }, 
            } );
            context.SaveChanges();
        }
    }
}
