using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.DataContext
{
    public class QuestionnaireDbContext : DbContext
    {
        public QuestionnaireDbContext ( DbContextOptions< QuestionnaireDbContext > options )
            : base( options )
        { }

        public QuestionnaireDbContext ()
            : base()
        { }

        protected override void OnConfiguring ( DbContextOptionsBuilder optionsBuilder )
        {
            if ( !optionsBuilder.IsConfigured ) {

                try {
                    var connectionStr = ConfigurationManager
                                        .ConnectionStrings[ "Questionnaire" ].ConnectionString;

                    optionsBuilder.UseSqlServer( connectionStr );
                }
                catch ( Exception ) {
                    throw new ConfigurationErrorsException("Какая-то беда с connectionString.");
                }

            }
        }

        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            modelBuilder.Entity< AnswerOpen >().HasKey( a => new { a.FirmId, a.Num } );
            modelBuilder.Entity< AnswerMultipleChoice >().HasKey( a => new { a.FirmId, a.Num } );
        }

        public DbSet< Region > Regions { get; set; }
        public DbSet< City > Cities { get; set; }
        public DbSet< FirmType > FirmTypes { get; set; }
        public DbSet< Firm > Firms { get; set; }

        public DbSet< Category > Categories { get; set; }
        public DbSet< Section > Sections { get; set; }
        public DbSet< QuestionOpen > OpenQuestions { get; set; }
        public DbSet< QuestionMultipleChoice > MultipleChoiceQuestions { get; set; }

        public DbSet< AnswerOpen > OpenAnswers { get; set; }
        public DbSet< AnswerMultipleChoice > MultipleChoiceAnswers { get; set; }
    }
}
