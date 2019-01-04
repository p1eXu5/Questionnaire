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
        #region Fields

        private readonly QuestionnaireDbContext _context;

        private bool _disposed;

        #endregion


        #region Ctor

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

        #endregion


        #region Properties

        public QuestionnaireDbContext DbContext => _context;

        #endregion



        public IEnumerable< Region > GetRegions ()
        {
            return _context.Regions.AsNoTracking().OrderBy( s => s.Name );
        }

        public IEnumerable< Firm > GetFirms ()
        {
            return _context.Firms.AsNoTracking().OrderBy( f => f.Name );
        }

        public IEnumerable< City > GetCities ()
        {
            return _context.Cities.AsNoTracking().OrderBy( c => c.Name );
        }

        public IEnumerable< Section > GetSections ()
        {
            return _context.Sections.AsNoTracking().OrderBy( s => s.Id );
        }

        public IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< QuestionOpen > GetOpenQuestions ()
        {
            throw new NotImplementedException();
        }


        public void AddAnswers ( IEnumerable< AnswerMultipleChoice > answers )
        {
            throw new NotImplementedException();
        }

        public void AddAnswers ( IEnumerable< AnswerOpen > answers )
        {
            throw new NotImplementedException();
        }

        public IEnumerable< AnswerMultipleChoice > GetMultipleChoiceAnswers ()
        {
            throw new NotImplementedException();
        }

        public IEnumerable< AnswerOpen > GetOpenAnswers ()
        {
            throw new NotImplementedException();
        }


        private void SeedData ()
        {

            foreach ( var region in Seeder.GetRegions() ) {
                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Regions ON; INSERT INTO dbo.Regions ( [Id], [Name] ) VALUES ( { region.Id }, '{ region.Name }' )" );
            }

            foreach ( var city in Seeder.GetCities() ) {
                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Cities ON; INSERT INTO dbo.Cities ( [Id], [Name], [RegionId] ) VALUES ( { city.Id }, '{ city.Name }', { city.RegionId } )" );
            }
            
            foreach ( var firmType in Seeder.GetFirmTypes() ) {
                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.FirmTypes ON; INSERT INTO dbo.FirmTypes ( [Id], [Name] ) VALUES ( { firmType.Id }, '{ firmType.Name }' )" );
            }

            _context.Firms.AddRange( Seeder.GetFirms() );

            foreach ( var category in Seeder.GetCategories() ) {
                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Categories ON; INSERT INTO dbo.Categories ( [Id], [Name] ) VALUES ( { category.Id }, '{ category.Name }' )" );
            }

            foreach ( var section in Seeder.GetSections() ) {
                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.Sections ON; INSERT INTO dbo.Sections ( [Id], [Name], [CategoryId] ) VALUES ( { section.Id }, '{ section.Name }', { section.CategoryId } )" );
            }

            foreach ( var questionMultiple in Seeder.GetQuestionMultipleChoiceList() ) {

                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.MultipleChoiceQuestions ON; " +
                                                     $"INSERT INTO dbo.MultipleChoiceQuestions ( [Id], [Name], [CategoryId] ) " +
                                                     $"VALUES ( { questionMultiple.Id }, '{ questionMultiple.Text }', { questionMultiple.SectionId } )" );
            }

            foreach ( var questionOpen in Seeder.GetQuestionOpenList() ) {

                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.OpenQuestions ON; " +
                                                     $"INSERT INTO dbo.OpenQuestions ( [Id], [Name], [CategoryId] ) " +
                                                     $"VALUES ( { questionOpen.Id }, '{ questionOpen.Text }', { questionOpen.SectionId } )" );
            }

            _context.SaveChanges();
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
