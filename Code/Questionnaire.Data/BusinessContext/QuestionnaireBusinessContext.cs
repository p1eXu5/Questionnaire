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

            if ( options == null ) {

                _context = new QuestionnaireDbContext();
                SeedData();
            }
            else {
                _context = new QuestionnaireDbContext( options );
                _context.Database.EnsureCreated();
                SeedDataForTests();
            }

        }

        #endregion


        #region Properties

        public QuestionnaireDbContext DbContext => _context;

        #endregion

        #region Methods

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
            return _context.Sections
                           //.Include( section => section.QuestionMultipleChoiceCollection )
                           //.Include( section => section.QuestionOpenCollection )
                           .AsNoTracking()
                           .OrderBy( s => s.Id ).AsEnumerable();
        }


        public IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions ()
        {
            return _context.MultipleChoiceQuestions.AsNoTracking().OrderBy( q => q.Id );
        }

        public IEnumerable< QuestionOpen > GetOpenQuestions ()
        {
            return _context.OpenQuestions.AsNoTracking().OrderBy( q => q.Id );
        }


        public void AddAnswer ( AnswerMultipleChoice answer )
        {
            _context.MultipleChoiceAnswers.Add( Check.Checked( (dynamic)answer ) );
        }

        public void AddAnswer ( AnswerOpen answer )
        {
            _context.OpenAnswers.Add( Check.Checked( (dynamic)answer ) );
        }

        public IEnumerable< dynamic > GetMultipleChoiceAnswers ()
        {
            return ( from a in _context.MultipleChoiceAnswers
                     group a by new { FirmId = a.FirmId, EmployeeId = a.Num } into firms
                     select new {
                         FirmId = firms.Key.FirmId,
                         EmployeeId = firms.Key.EmployeeId,
                         Categories = from c in firms
                                      group c by c.Question.Section.CategoryId into cat
                                      select new {
                                          CategoryId = cat.Key,
                                          Sections = from s in cat
                                                     group s by s.Question.SectionId into sec
                                                     select new {
                                                         SectionId = sec.Key,
                                                         Answer = sec.Sum( s => s.Answer )
                                                     }
                                      }				  
                     }).ToArray();
        }

        public IEnumerable< AnswerOpen > GetOpenAnswers ()
        {
            return _context.OpenAnswers.AsNoTracking().OrderBy( a => a.FirmId );
        }

        public void SaveChanges () => _context.SaveChanges();

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

            foreach ( var questionMultiple in Seeder.GetMultipleChoiceQuestions() ) {

                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.MultipleChoiceQuestions ON; " +
                                                     $"INSERT INTO dbo.MultipleChoiceQuestions ( [Id], [Name], [CategoryId] ) " +
                                                     $"VALUES ( { questionMultiple.Id }, '{ questionMultiple.Text }', { questionMultiple.SectionId } )" );
            }

            foreach ( var questionOpen in Seeder.GetOpenQuestions() ) {

                _context.Database.ExecuteSqlCommand( $"SET IDENTITY_INSERT dbo.OpenQuestions ON; " +
                                                     $"INSERT INTO dbo.OpenQuestions ( [Id], [Name], [CategoryId] ) " +
                                                     $"VALUES ( { questionOpen.Id }, '{ questionOpen.Text }', { questionOpen.SectionId } )" );
            }

            _context.SaveChanges();
        }

        private void SeedDataForTests ()
        {
            _context.Regions.AddRange( Seeder.GetRegions() );
            _context.Cities.AddRange( Seeder.GetCities() );
            _context.FirmTypes.AddRange( Seeder.GetFirmTypes() );
            _context.Firms.AddRange( Seeder.GetFirms() );

            _context.Categories.AddRange( Seeder.GetCategories() );
            _context.Sections.AddRange( Seeder.GetSections() );
            _context.MultipleChoiceQuestions.AddRange( Seeder.GetMultipleChoiceQuestions() );
            _context.OpenQuestions.AddRange( Seeder.GetOpenQuestions() );

            _context.SaveChanges();
        }

        static class Check
        {
            public static bool Checked ( dynamic answer )
            {
                if ( answer.Num <= 0 ) throw new ArgumentException("EmployeeNum cannot be not greater than zero");
                if ( answer.Question == null ) throw new ArgumentNullException( nameof(answer.Question), "Question cannot be null.");

                return true;
            }
        }

        #endregion

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
