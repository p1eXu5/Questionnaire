using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Questionnaire.Data.DataContext;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public class QuestionnaireBusinessContext : IQuestionnaireBusinessContext, IDisposable
    {
        private readonly QuestionnaireDbContext _context;

        private bool _disposed;

        public QuestionnaireBusinessContext ()
        {
            _context = new QuestionnaireDbContext();
            _context.Database.Migrate();
            SeedData();
        }

        public QuestionnaireBusinessContext ( DbContextOptions< QuestionnaireDbContext > options )
        {
            _context = options == null ? new QuestionnaireDbContext() : new QuestionnaireDbContext( options );

            SeedData();
        }


        public QuestionnaireDbContext DbContext => _context;

        public Region[] GetRegions ()
        {
            return _context.Regions.OrderBy( s => s.Name ).ToArray();
        }

        public Firm[] GetFirms ()
        {
            return _context.Firms.OrderBy( f => f.Name ).ToArray();
        }

        public City[] GetCities ()
        {
            return _context.Cities.OrderBy( c => c.Name ).ToArray();
        }

        public Section[] GetSections ()
        {
            return _context.Sections.OrderBy( s => s.Id ).ToArray();
        }

        public void AddFirm ( Firm firm )
        {

        }

        private void SeedData ()
        {

            foreach ( var region in Seeder.GetRegions() ) {
                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Regions ON; INSERT INTO dbo.Regions ( [Id], [Name] ) VALUES ( { region.Id }, '{ region.Name }' )" );
            }

            foreach ( var city in Seeder.GetCities() ) {
                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Cities ON; INSERT INTO dbo.Cities ( [Id], [Name], [RegionId] ) VALUES ( { city.Id }, '{ city.Name }', { city.RegionId } )" );
            }
            


            //_context.FirmTypes.AddRange( Seeder.GetFirmTypes() );
            //_context.Firms.AddRange( Seeder.GetFirms() );
            //_context.Categories.AddRange( Seeder.GetCategories() );
            //_context.Sections.AddRange( Seeder.GetSections() );
            //_context.QuestionMultipleChoiceCollection.AddRange( Seeder.GetQuestionMultipleChoiceList() );
            //_context.QuestionOpenCollection.AddRange( Seeder.GetQuestionOpenList() );

            //_context.SaveChanges();
        }

        static class Check
        {

        }

        #region IDisposable

        public void Dispose ()
        {
            Dispose( true );
        }

        private void Dispose ( bool disposing )
        {
            if ( !disposing || _disposed ) return;

            _context?.Dispose();
            _disposed = true;
        }

        #endregion
    }
}
