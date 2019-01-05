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
            if ( !context.Regions.Any() ) { 
                context.Regions.AddRange( Seeder.GetRegions().OrderBy( r => r.Id ).Select( r =>{ r.Id = 0; return r; } ) );
                context.SaveChanges();
            }

            if ( !context.Cities.Any() ) {
                context.Cities.AddRange( Seeder.GetCities().OrderBy( c => c.Id ).Select( c => { c.Id = 0; return c; } ) );
                context.SaveChanges();
            }

            if ( !context.FirmTypes.Any() ) {
                context.FirmTypes.AddRange( Seeder.GetFirmTypes().OrderBy( ft => ft.Id ).Select( ft => { ft.Id = 0; return ft; } ) );
                context.SaveChanges();
            }

            if ( !context.Firms.Any() ) {
                context.Firms.AddRange( Seeder.GetFirms() );
                context.SaveChanges();
            }

            SeedEntities( context.Categories, Seeder.GetCategories(), context );

            SeedEntities( context.Sections, Seeder.GetSections(), context );

            SeedEntities( context.MultipleChoiceQuestions, Seeder.GetMultipleChoiceQuestions(), context );

            SeedEntities( context.OpenQuestions, Seeder.GetOpenQuestions(), context );

        }

        private void SeedEntities< T > ( DbSet<T> entity, IEnumerable< T > seedColl, QuestionnaireDbContext context )
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
