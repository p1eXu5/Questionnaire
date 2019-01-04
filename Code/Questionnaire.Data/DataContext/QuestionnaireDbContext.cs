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
                optionsBuilder.UseSqlServer( ConfigurationManager
                                             .ConnectionStrings[ "QuestionnaireDb" ].ConnectionString );
            }
        }

        public DbSet< Region > Regions { get; set; }
        public DbSet< City > Cities { get; set; }
        public DbSet< FirmType > FirmTypes { get; set; }
        public DbSet< Firm > Firms { get; set; }

        public DbSet< Category > Categories { get; set; }
        public DbSet< Section > Sections { get; set; }
        public DbSet< QuestionOpen > OpenQuestions { get; set; }
        public DbSet< QuestionMultipleChoice > MultipleChoiceQuestions { get; set; }

        public DbSet< AnswerOpen > AnswerOpenCollection { get; set; }
        public DbSet< AnswerMultipleChoice > AnswerMultipleChoiceCollection { get; set; }
    }
}
