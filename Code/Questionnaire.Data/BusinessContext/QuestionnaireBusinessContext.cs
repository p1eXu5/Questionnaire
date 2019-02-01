using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Questionnaire.Data.BusinessContext.Comparers;
using Questionnaire.Data.BusinessContext.Visitors;
using Questionnaire.Data.DataContext;
using Questionnaire.Data.Models;
using Questionnaire.Data.BusinessContext.Extensions;

namespace Questionnaire.Data.BusinessContext
{
    public class QuestionnaireBusinessContext : IQuestionnaireBusinessContext, IDisposable
    {
        #region Fields

        private readonly QuestionnaireDbContext _context;

        private bool _disposed;

        #endregion



        #region Ctor

        public QuestionnaireBusinessContext (IDataSeeder dataSeeder = null )
        {
            _context = new QuestionnaireDbContext();
            _context.Database.Migrate();

            dataSeeder?.SeedData( this );
        }

        public QuestionnaireBusinessContext ( DbContextOptions< QuestionnaireDbContext > options, IDataSeeder dataSeeder )
        {
            if ( options == null ) throw new ArgumentNullException( nameof( options ), "Options cannot be null." );

            _context = new QuestionnaireDbContext( options );
            _context.Database.EnsureCreated();

            dataSeeder?.SeedData( this );
        }

        #endregion



        #region Properties

        public QuestionnaireDbContext DbContext => _context;

        private EntityChecker EntityChecker { get; } = new EntityChecker();

        #endregion



        #region Methods

        public int GetNextNumOfTested(int firmId)
        {
            var openAnsw = GetOpenAnswers().Where(a => a.FirmId == firmId).ToArray();
            var multiAnsw = GetMultipleChoiceAnswers().Where(a => a.FirmId == firmId).ToArray();

            if (openAnsw.Length == 0 && multiAnsw.Length == 0) return 1;

            if (openAnsw.Length == 0) return multiAnsw.Max(a => a.Num) + 1;
            if (multiAnsw.Length == 0) return openAnsw.Max(a => a.Num) + 1;

            var multiNum = multiAnsw.Max(a => a.Num);
            var openNum = openAnsw.Max(a => a.Num);

            return multiNum >= openNum ? multiNum + 1 : openNum + 1;
        }

        public bool HasMultipleChoiceAnswers() => GetMultipleChoiceAnswers().Any();


        public IEnumerable< Region > GetRegions ()
        {
            return _context.Regions.AsNoTracking().OrderBy( s => s.Name ).ToArray();
        }

        public IEnumerable< Firm > GetFirms ()
        {
            return _context.Firms.OrderBy( f => f.Name ).ToArray();
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
                           .OrderBy( s => s.Id ).ToArray();
        }

        public IEnumerable< Category > GetCategories ()
        {
            return _context.Categories.Include( c => c.Sections ).AsNoTracking().OrderBy( c => c.Id ).ToArray();
        }

        public IEnumerable< QuestionMultipleChoice > GetMultipleChoiceQuestions ()
        {
            return _context.MultipleChoiceQuestions.AsNoTracking().OrderBy( q => q.Id ).ToArray();
        }

        public IEnumerable< QuestionOpen > GetOpenQuestions ()
        {
            return _context.OpenQuestions.AsNoTracking().OrderBy( q => q.Id ).ToArray();
        }

        public IEnumerable< AnswerMultipleChoice > GetMultipleChoiceAnswers ()
        {
            return _context.MultipleChoiceAnswers.ToArray();
        }

        public IEnumerable< AnswerOpen > GetOpenAnswers ()
        {
            return _context.OpenAnswers.OrderBy( a => a.FirmId ).ToArray();
        }


        public IEnumerable< dynamic > GetGruppedMultipleChoiceAnswers ()
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
                                                         AnswerSum = sec.Sum( s => s.Answer )
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

        public void DeleteAnswers ( int firmId, int employeeNum )
        {
             _context.OpenAnswers.RemoveRange( _context.OpenAnswers.Where( a => a.FirmId == firmId && a.Num == employeeNum ) );
            _context.MultipleChoiceAnswers.RemoveRange( _context.MultipleChoiceAnswers.Where( a => a.FirmId == firmId && a.Num == employeeNum ) );
            _context.SaveChanges();
        }

        public void AddRegions ( IEnumerable< Region > regions )
        {
            if (regions == null) throw new ArgumentNullException( nameof( regions ), "Region collection cannot be null.");

            var dbNames = new HashSet< string >( _context.Regions.Select( r => r.Name ) );
            var coll = regions.Distinct( new NameEntityComparer() ).ToArray();
            _context.Regions.AddRange( coll.Where( r => !String.IsNullOrWhiteSpace( r.Name ) && !dbNames.Contains( r.Name ) ).Select( r => new Region { Name = r.Name } ) );
            _context.SaveChanges();
        }

        public void DeleteRegion ( Region region )
        {
            var reg = _context.Regions.FirstOrDefault( r => r.Name.Equals( r.Name ) );
            if ( null == reg ) return;

            _context.Regions.Remove( reg );
            _context.SaveChanges();
        }

        public void AddCities ( IEnumerable< City > cities )
        {
            if ( cities == null ) throw new ArgumentNullException(nameof(cities), "City collection cannot be null.");

            var dbNames = new HashSet<string>( _context.Cities.Select( r => r.Name ) );
            var coll = cities.Distinct( new NameEntityComparer() ).ToArray();
            _context.Cities.AddRange( coll.Where( r => !String.IsNullOrWhiteSpace(r.Name) && !dbNames.Contains(r.Name) ).Select(r => new City { Name = r.Name, RegionId = ((City)r).RegionId }));
            _context.SaveChanges();
        }

        public void AddFirmTypes ( IEnumerable< FirmType > firmTypes )
        {
            if ( firmTypes == null ) throw new ArgumentNullException(nameof(firmTypes), "FirmType collection cannot be null.");

            var dbNames = new HashSet<string>( _context.FirmTypes.Select( r => r.Name ) );
            var coll = firmTypes.Distinct( new NameEntityComparer() ).ToArray();
            _context.FirmTypes.AddRange( coll.Where( r => !String.IsNullOrWhiteSpace(r.Name) && !dbNames.Contains(r.Name) ).Select(r => new FirmType { Name = r.Name }));
            _context.SaveChanges();
        }

        public void AddFirms ( IEnumerable< Firm > firms )
        {
            if ( firms == null ) throw new ArgumentNullException(nameof(firms), "Firm collection cannot be null.");

            var dbIds = new HashSet< int >( _context.Firms.Select( r => r.Id ) );

            var coll = firms.Distinct( new FirmEqualityComparer() ).ToArray();

            var c = _context.Cities.ToArray();
            var ft = _context.FirmTypes.ToArray();

            _context.Firms.AddRange( coll.Where( r => !String.IsNullOrWhiteSpace(r.Name) && !dbIds.Contains(r.Id) ).Cast< Firm >() );
            _context.SaveChanges();
        }

        public void AddCategories ( IEnumerable< Category > categories )
        {
            if ( categories == null ) throw new ArgumentNullException(nameof(categories), "Category collection cannot be null.");

            var dbNames = new HashSet<string>( _context.Categories.Select( r => r.Name ) );
            var coll = categories.Distinct( new NameEntityComparer() ).ToArray();
            _context.Categories.AddRange( coll.Where( r => !String.IsNullOrWhiteSpace(r.Name) && !dbNames.Contains( r.Name ) ).Select(r => new Category { Name = r.Name }));
            _context.SaveChanges();
        }

        public void AddSections ( IEnumerable< Section > sections )
        {
            if ( sections == null ) throw new ArgumentNullException(nameof(sections), "Section collection cannot be null.");

            var dbNames = new HashSet<string>( _context.Sections.Select( r => r.Name ) );
            var coll = sections.Distinct( new NameEntityComparer() ).ToArray();
            _context.Sections.AddRange( coll.Where( r => !String.IsNullOrWhiteSpace(r.Name) && !dbNames.Contains( r.Name ) )
                                            .Select( r => new Section { Name = r.Name, CategoryId = ((Section)r).CategoryId }));
            _context.SaveChanges();
        }

        public void AddMultipleChoiceQuestions ( IEnumerable< QuestionMultipleChoice > questions )
        {
            if ( questions == null ) throw new ArgumentNullException(nameof(questions), "Section collection cannot be null.");

            var dbNames = new HashSet<string>( _context.MultipleChoiceQuestions.Select( r => r.Text ) );
            var coll = questions.Distinct( new QuestionEqualityComparer() ).ToArray();
            _context.MultipleChoiceQuestions.AddRange( coll.Where( r => !String.IsNullOrWhiteSpace(r.Text) && !dbNames.Contains( r.Text ) )
                                            .Select( r => new QuestionMultipleChoice { Text = r.Text, SectionId = (( QuestionMultipleChoice )r).SectionId } ));
            _context.SaveChanges();
        }

        public void AddOpenQuestions ( IEnumerable< QuestionOpen > questions )
        {
            if ( questions == null ) throw new ArgumentNullException(nameof(questions), "Section collection cannot be null.");

            var dbNames = new HashSet<string>( _context.OpenQuestions.Select( r => r.Text ) );
            var coll = questions.Distinct( new QuestionEqualityComparer() ).ToArray();
            _context.OpenQuestions.AddRange( coll.Where( r => !String.IsNullOrWhiteSpace(r.Text) && !dbNames.Contains( r.Text ) )
                                            .Select( r => new QuestionOpen { Text = r.Text, SectionId = (( QuestionOpen )r).SectionId } ));
            _context.SaveChanges();
        }


        public void SaveChanges () => _context.SaveChanges();

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



        static class Check
        {
            public static dynamic Checked ( dynamic answer )
            {
                if ( (int)answer.Num <= 0 ) throw new ArgumentException("EmployeeNum cannot be not greater than zero");
                if ( answer.Firm == null )  throw new ArgumentException("Firm cannot be null");

                return answer;
            }
        }
    }
}
