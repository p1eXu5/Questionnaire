using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.DataContext;

namespace Questionnaire.Data.Tests.BusinessContext.UnitTests
{
    [ TestFixture ]
    public class QuestionnaireBusinessContextTests
    {
        [ SetUp ]
        public void OpenSqliteConnection ()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }

        [ TearDown ]
        public void CloseSqliteConnection ()
        {
            _connection.Close();
        }

        [ Test ]
        public void GetOpenAnswers_DbNotExist_ReturnsEmptyCollection ()
        {
            // Arrange:
            var bc = GetQuestionnaireBusinessContext( "GetOpenAnswers" );

            // Action:
            var resColl = bc.GetOpenAnswers();

            // Assert:
            Assert.That( !resColl.Any() );
        }

        [ Test ]
        public void GetSections_DbNotExist_ReturnsNotEmptySectionCollection ()
        {
            // Arrange:
            SavePaths();
            var bc = GetQuestionnaireBusinessContext( "GetSections" );

            // Action:
            var resColl = bc.GetSections().ToArray();

            // Assert:
            Assert.That( resColl.Any() );
            RestorePath();
        }


        #region Factory

        private readonly string[] _paths = new string[8];
        private SqliteConnection _connection;

        private void SavePaths ()
        {
            _paths[ 0 ] = Seeder.FileNameRegions;
            Seeder.FileNameRegions = Seeder.FileNameRegions.AppendAssemblyPath();

            _paths[ 1 ] = Seeder.FileNameCities;
            Seeder.FileNameCities = Seeder.FileNameCities.AppendAssemblyPath();

            _paths[ 2 ] = Seeder.FileNameFirmTypes;
            Seeder.FileNameFirmTypes = Seeder.FileNameFirmTypes.AppendAssemblyPath();

            _paths[ 3 ] = Seeder.FileNameFirms;
            Seeder.FileNameFirms = Seeder.FileNameFirms.AppendAssemblyPath();


            _paths[ 4 ] = Seeder.FileNameCategories;
            Seeder.FileNameCategories = Seeder.FileNameCategories.AppendAssemblyPath();

            _paths[ 5 ] = Seeder.FileNameSections;
            Seeder.FileNameSections = Seeder.FileNameSections.AppendAssemblyPath();

            _paths[ 6 ] = Seeder.FileNameQuestionMultipleChoiceList;
            Seeder.FileNameQuestionMultipleChoiceList = Seeder.FileNameQuestionMultipleChoiceList.AppendAssemblyPath();

            _paths[ 7 ] = Seeder.FileNameQuestionOpenList;
            Seeder.FileNameQuestionOpenList = Seeder.FileNameQuestionOpenList.AppendAssemblyPath();
        }

        public void RestorePath ()
        {
            Seeder.FileNameQuestionOpenList = _paths[ 7 ];
            Seeder.FileNameQuestionMultipleChoiceList = _paths[ 6 ];
            Seeder.FileNameSections = _paths[ 5 ];
            Seeder.FileNameCategories = _paths[ 4 ];
            Seeder.FileNameFirms = _paths[ 3 ];
            Seeder.FileNameFirmTypes = _paths[ 2 ];
            Seeder.FileNameCities = _paths[ 1 ];
            Seeder.FileNameSections = _paths[ 0 ];
        }

        private QuestionnaireBusinessContext GetQuestionnaireBusinessContext ( string dbName )
        {
            if ( String.IsNullOrWhiteSpace( dbName ) ) throw new ArgumentException();

            var options = new DbContextOptionsBuilder< QuestionnaireDbContext >()
                            .UseSqlite( _connection )
                            .Options;

            return new QuestionnaireBusinessContext( options );
        }

        #endregion
    }
}
