using System;
using System.Collections.Generic;
using System.Linq;
using Agbm.Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.DataContext;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.Tests.BusinessContext.UnitTests
{
    [ TestFixture ]
    public class QuestionnaireBusinessContextTests
    {
        [SetUp]
        public void OpenSqliteConnection()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
        }

        [TearDown]
        public void CloseSqliteConnection()
        {
            _connection.Close();
            _options = null;
        }

        [ Test ]
        public void GetOpenAnswers_DbNotExist_ReturnsEmptyCollection ()
        {
            // Arrange:
            var bc = GetQuestionnaireBusinessContext();

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
            var bc = GetQuestionnaireBusinessContext();

            // Action:
            var resColl = bc.GetSections().ToArray();

            // Assert:
            Assert.That( resColl.Any() );
            RestorePath();
        }

        [ Test ]
        public void GetSections_DbNotExist_ReturnsSectionWithQuestionsCollection ()
        {
            // Arrange:
            var bc = GetQuestionnaireBusinessContext();

            // Action:
            var resColl = bc.GetSections().ToArray();

            // Assert:
            Assert.That( resColl.Any( q => q.QuestionMultipleChoiceCollection.Any() ) );
            Assert.That( resColl.Any( q => q.QuestionOpenCollection.Any() ) );
            Assert.That( resColl.All( q => q.CategoryId > 0 ) );
        }

        [ Test ]
        public void AddAnswer__OpenAnswer_FirmIdNotValid__AddsAnswer ()
        {
            // Arrange:
            var bc = GetQuestionnaireBusinessContext();
            var answer = GetOpenAnswer( bc );
            answer.FirmId = 0;

            // Action:
            bc.AddAnswer( answer );

            // Assert:
            var answerRes = bc.GetOpenAnswers().ToArray()[0];
            Assert.That( answerRes.Answer.Equals( answer.Answer ) );
        }

        [ Test ]
        public void AddAnswer__OpenAnswer_FirmIsNull__Throws ()
        {
            // Arrange:
            var bc = GetQuestionnaireBusinessContext();
            var answer = GetOpenAnswer( bc );
            answer.Firm = null;

            // Action:
            // Assert:
            var ex = Assert.Catch( () => bc.AddAnswer( answer ) );
        }

        [ TestCase( 0 ) ]
        [ TestCase( -1 ) ]
        public void AddAnswer__OpenAnswer_NumEqualsOrLessThahZero__Throws ( int num )
        {
            // Arrange:
            var bc = GetQuestionnaireBusinessContext();
            var answer = GetOpenAnswer( bc );
            answer.Num = num;

            // Action:
            // Assert:
            var ex = Assert.Catch< ArgumentException >( () => bc.AddAnswer( answer ) );
        }


        [ Test ]
        public void AddAnswer__MultipleChoiceAnswer_FirmIdNotValid__AddsAnswer ()
        {
            // Arrange:
            var bc = GetQuestionnaireBusinessContext();
            var answer = GetMultipleChoiceAnswer( bc );
            answer.FirmId = 0;

            // Action:
            bc.AddAnswer( answer );

            // Assert:
            var answerRes = bc.GetGruppedMultipleChoiceAnswers().ToArray()[0];
            Assert.That( answerRes, Is.Not.Null );
        }

        [ Test ]
        public void AddAnswer__MultipleChoiceAnswer_FirmIsNull__Throws ()
        {
            // Arrange:
            var bc = GetQuestionnaireBusinessContext();
            var answer = GetMultipleChoiceAnswer( bc );
            answer.Firm = null;

            // Action:
            // Assert:
            var ex = Assert.Catch( () => bc.AddAnswer( answer ) );
        }

        [ TestCase( 0 ) ]
        [ TestCase( -1 ) ]
        public void AddAnswer__MultipleChoiceAnswer_NumEqualsOrLessThahZero__Throws ( int num )
        {
            // Arrange:
            using ( var bc = GetQuestionnaireBusinessContext() ) {
             
                var answer = GetMultipleChoiceAnswer( bc );
                answer.Num = num;

                // Action:
                // Assert:
                var ex = Assert.Catch< ArgumentException >( () => bc.AddAnswer( answer ) );
            }
        }



        [ Test ]
        public void DeleteAnswers__DbHasOpenAnswers__DeleteAnswers ()
        {
            // Arrange:
            var bc = GetQuestionnaireBusinessContext();

            var answer = GetOpenAnswer( bc );
            answer.Num = 1;
            bc.AddAnswer( answer );
            
            answer = GetOpenAnswer( bc );
            answer.Num = 2;
            bc.AddAnswer( answer );

            var answerMulti = GetMultipleChoiceAnswer( bc );
            answerMulti.Num = 1;
            bc.AddAnswer( answerMulti );
            
            answerMulti = GetMultipleChoiceAnswer( bc );
            answerMulti.Num = 2;
            bc.AddAnswer( answerMulti );

            var openAnswers = bc.GetOpenAnswers();
            var multiAnswers = bc.GetGruppedMultipleChoiceAnswers();
            Assert.That( openAnswers.Any() );
            Assert.That( multiAnswers.Any() );

            // Action:
            bc.DeleteAnswers();

            // Assert:
            openAnswers = bc.GetOpenAnswers();
            multiAnswers = bc.GetGruppedMultipleChoiceAnswers();
            Assert.That( !openAnswers.Any() );
            Assert.That( !multiAnswers.Any() );
        }


        [ Test ]
        public void AddRegions_RegionWithExistedNameDifferentId_DoesntAddRegionInDb ()
        {
            var regionName = "Region 1";

            using ( var context = GetQuestionnaireBusinessContext( true ) ) {

                var regions = new[] {
                    new Region { Id = 1, Name = regionName },
                    new Region { Id = 2, Name = regionName },
                    new Region { Id = 3, Name = regionName }
                };

                context.AddRegions( regions );
            }

            using ( var context = GetQuestionnaireBusinessContext( true ) ) {

                var regions = context.GetRegions().Where( r => r.Name.Equals( regionName ) ).ToArray();
                Assert.That( 1 == regions.Length, $"was { regions.Length }" );
                Assert.That( 1 == regions[0].Id, $"was { regions[0].Id }");
            }
        }

        [ Test ]
        public void AddRegions_RegionsIsNull_Throw ()
        {
            using ( var context = GetQuestionnaireBusinessContext( true ) ) {

                var regions = ( IEnumerable< Region > )null;
                Assert.Catch< ArgumentNullException >( () => context.AddRegions( regions ) );
            }
        }

        [Test]
        public void AddRegions_RegionNameIsNull_DoesNotAdd()
        {
            using ( var context = GetQuestionnaireBusinessContext( true ) ) {

                var regions = new[] { new Region(), };
                context.AddRegions(regions);
            }

            using ( var context = GetQuestionnaireBusinessContext( true ) ) {
                var regions = context.GetRegions();
                Assert.That( !regions.Any() );
            }
        }




        #region Factory

        private readonly string[] _paths = new string[8];
        private SqliteConnection _connection;
        private DbContextOptions< QuestionnaireDbContext > _options;

        private void SavePaths ()
        {
            _paths[ 0 ] = DataSeeder.FileNameRegions;
            DataSeeder.FileNameRegions = DataSeeder.FileNameRegions.AppendAssemblyPath();

            _paths[ 1 ] = DataSeeder.FileNameCities;
            DataSeeder.FileNameCities = DataSeeder.FileNameCities.AppendAssemblyPath();

            _paths[ 2 ] = DataSeeder.FileNameFirmTypes;
            DataSeeder.FileNameFirmTypes = DataSeeder.FileNameFirmTypes.AppendAssemblyPath();

            _paths[ 3 ] = DataSeeder.FileNameFirms;
            DataSeeder.FileNameFirms = DataSeeder.FileNameFirms.AppendAssemblyPath();


            _paths[ 4 ] = DataSeeder.FileNameCategories;
            DataSeeder.FileNameCategories = DataSeeder.FileNameCategories.AppendAssemblyPath();

            _paths[ 5 ] = DataSeeder.FileNameSections;
            DataSeeder.FileNameSections = DataSeeder.FileNameSections.AppendAssemblyPath();

            _paths[ 6 ] = DataSeeder.FileNameQuestionMultipleChoiceList;
            DataSeeder.FileNameQuestionMultipleChoiceList = DataSeeder.FileNameQuestionMultipleChoiceList.AppendAssemblyPath();

            _paths[ 7 ] = DataSeeder.FileNameQuestionOpenList;
            DataSeeder.FileNameQuestionOpenList = DataSeeder.FileNameQuestionOpenList.AppendAssemblyPath();
        }

        private void RestorePath ()
        {
            DataSeeder.FileNameQuestionOpenList = _paths[ 7 ];
            DataSeeder.FileNameQuestionMultipleChoiceList = _paths[ 6 ];
            DataSeeder.FileNameSections = _paths[ 5 ];
            DataSeeder.FileNameCategories = _paths[ 4 ];
            DataSeeder.FileNameFirms = _paths[ 3 ];
            DataSeeder.FileNameFirmTypes = _paths[ 2 ];
            DataSeeder.FileNameCities = _paths[ 1 ];
            DataSeeder.FileNameSections = _paths[ 0 ];
        }

        private QuestionnaireBusinessContext GetQuestionnaireBusinessContext ( bool createEmpty = false )
        {
            if ( _options == null ) {

                _options = new DbContextOptionsBuilder< QuestionnaireDbContext >()
                            .UseSqlite( _connection )
                            .Options;
            }

            return createEmpty
                        ? new QuestionnaireBusinessContext( _options, null )
                        : new QuestionnaireBusinessContext( _options, new TestDataSeeder() );
        }

        private AnswerOpen GetOpenAnswer ( IQuestionnaireBusinessContext context )
        {
            return new AnswerOpen { Firm = context.GetFirms().First(),
                                    Num = 1,
                                    Answer = "TestAnswer",
                                    Question = context.GetOpenQuestions().First()
                                  };
        }

        private AnswerMultipleChoice GetMultipleChoiceAnswer ( IQuestionnaireBusinessContext context )
        {
            return new AnswerMultipleChoice { Firm = context.GetFirms().First(),
                                              Num = 1,
                                              Answer = 2,
                                              Question = context.GetMultipleChoiceQuestions().First()
                                            };
        }

        #endregion
    }
}
