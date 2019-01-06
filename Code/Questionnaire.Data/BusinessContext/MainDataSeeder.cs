using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Questionnaire.Data.DataContext;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public class MainDataSeeder : IDataSeeder
    {
        public void SeedData ( QuestionnaireDbContext context )
        {
            SeedEntitiesWithZeroId( context.Regions, Seeder.GetRegions(), context );
            SeedEntitiesWithZeroId( context.Cities, Seeder.GetCities(), context );
            SeedEntitiesWithZeroId( context.FirmTypes, Seeder.GetFirmTypes(), context );

            if ( !context.Firms.Any() ) {
                context.Firms.AddRange( Seeder.GetFirms() );
                context.SaveChanges();
            }

            SeedEntitiesWithZeroId( context.Categories, Seeder.GetCategories(), context );
            SeedEntitiesWithZeroId( context.Sections, Seeder.GetSections(), context );
            SeedEntitiesWithZeroId( context.MultipleChoiceQuestions, Seeder.GetMultipleChoiceQuestions(), context );
            SeedEntitiesWithZeroId( context.OpenQuestions, Seeder.GetOpenQuestions(), context );
        }

        private void SeedEntitiesWithZeroId< T > ( DbSet<T> entity, IEnumerable< T > seedColl, QuestionnaireDbContext context )
            where T : class
        {
            if ( !entity.Any() ) {
                entity.AddRange( seedColl.OrderBy( ft => (int)(ft.GetType().GetProperty("Id").GetValue( ft )) )
                                         .Select( ft => { ft.GetType().GetProperty( "Id" ).SetValue( ft, 0 ); return ft; } ) );
                context.SaveChanges();
            }
        }
    }
}
