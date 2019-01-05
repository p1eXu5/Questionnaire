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
        private readonly IDataSeeder _dataSeeder;

        private bool _disposed;

        #endregion


        #region Ctor

        public QuestionnaireBusinessContext ()
            : this ( new MainDataSeeder() )
        {
            _context = new QuestionnaireDbContext();
            _context.Database.Migrate();

            _dataSeeder.SeedData( _context );
        }

        private QuestionnaireBusinessContext ( IDataSeeder dataSeeder )
        {
            _dataSeeder = dataSeeder ?? throw new ArgumentNullException( nameof( dataSeeder ), "IDataSeeder cannot be null." );
        }

        public QuestionnaireBusinessContext ( DbContextOptions< QuestionnaireDbContext > options, IDataSeeder dataSeeder )
            : this( dataSeeder )
        {
            if ( options == null ) throw new ArgumentNullException( nameof( options ), "Options cannot be null." );

            _context = new QuestionnaireDbContext( options );
            _context.Database.EnsureCreated();

            _dataSeeder.SeedData( _context );

        }

        #endregion


        #region Properties

        public QuestionnaireDbContext DbContext => _context;

        #endregion

        #region Methods

        public IEnumerable< Region > GetRegions ()
        {
            return _context.Regions.AsNoTracking().OrderBy( s => s.Name ).ToArray();
        }

        public IEnumerable< Firm > GetFirms ()
        {
            return _context.Firms.AsNoTracking().OrderBy( f => f.Name ).ToArray();
        }

        public IEnumerable< City > GetCities ()
        {
            return _context.Cities.AsNoTracking().OrderBy( c => c.Name ).ToArray();
        }

        public IEnumerable< Section > GetSections ()
        {
            return _context.Sections
                           .Include( section => section.QuestionMultipleChoiceCollection )
                           .Include( section => section.QuestionOpenCollection )
                           .AsNoTracking()
                           .OrderBy( s => s.Id ).ToArray();
        }


        public IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions ()
        {
            return _context.MultipleChoiceQuestions.AsNoTracking().OrderBy( q => q.Id ).ToArray();
        }

        public IEnumerable< QuestionOpen > GetOpenQuestions ()
        {
            return _context.OpenQuestions.AsNoTracking().OrderBy( q => q.Id ).ToArray();
        }

        public IEnumerable< AnswerOpen > GetOpenAnswers ()
        {
            return _context.OpenAnswers.Include( answer => answer.Firm ).AsNoTracking().OrderBy( a => a.FirmId ).ToArray();
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


        public void AddAnswer ( AnswerMultipleChoice answer )
        {
            _context.MultipleChoiceAnswers.Add( Check.Checked( (dynamic)answer ) );
            _context.SaveChanges();
        }

        public void AddAnswer ( AnswerOpen answer )
        {
            _context.OpenAnswers.Add( Check.Checked( (dynamic)answer ) );
            _context.SaveChanges();
        }

        public void DeleteAnswers ()
        {
            _context.OpenAnswers.RemoveRange( _context.OpenAnswers );
            _context.MultipleChoiceAnswers.RemoveRange( _context.MultipleChoiceAnswers );
            _context.SaveChanges();
        }


        public void SaveChanges () => _context.SaveChanges();



        static class Check
        {
            public static dynamic Checked ( dynamic answer )
            {
                if ( answer.Num <= 0 ) throw new ArgumentException("EmployeeNum cannot be not greater than zero");

                return answer;
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
