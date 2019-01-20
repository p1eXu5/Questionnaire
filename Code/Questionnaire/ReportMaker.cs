using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel;
using Questionnaire.Data.BusinessContext;
using Questionnaire.Data.Models;

namespace Questionnaire
{
    public static class ReportMaker
    {
        public const int SECTION_COEFFICIENT = 6;

        public static Color H1Color { get; set; }
        public static Color H2Color { get; set; }
        public static Color H3Color { get; set; }
        public static Color H4Color { get; set; }

        public static void MakeReport ( string fileName,  IQuestionnaireBusinessContext context,  ReportOrientation orientation = ReportOrientation.Horizontal )
        {
            var firms = context.GetMultipleChoiceAnswers().Select( a => a.Firm ).Distinct().ToArray();
            var sections = context.GetSections().ToArray();

            // firms
            foreach ( var firm in firms ) {

                var sheetData = MakeMultipleChoiceAnswersReport( context, firm, sections );
                CreateMultipleChoiceAnswersFile( firm, fileName, sheetData );

                sheetData.Clear();

                sheetData = MakeOpenAnswersReport( context, firm, sections );
                CreateOpenAnswersFile( firm, fileName, sheetData );
            }
        }

        /// <summary>
        /// Creates file with firm name suffix contained answers of multiple choice questions report.
        /// </summary>
        /// <param name="firm"></param>
        /// <param name="fileName"></param>
        /// <param name="sheetData"></param>
        private static void CreateMultipleChoiceAnswersFile ( Firm firm, string fileName, IEnumerable<IExportingCell> sheetData )
        {
            var location = Path.GetDirectoryName( fileName );
            var newFileName = $"{location}{Path.GetFileNameWithoutExtension( fileName )}_{firm.Name}_A1{Path.GetExtension( fileName )}";

            ExcelExporter.ExportData( newFileName, sheetData );
        }

        /// <summary>
        /// Creates file with firm name suffix contained answers of open questions report.
        /// </summary>
        /// <param name="firm"></param>
        /// <param name="fileName"></param>
        /// <param name="sheetData"></param>
        private static void CreateOpenAnswersFile ( Firm firm, string fileName, IEnumerable< IExportingCell > sheetData )
        {
            var location = Path.GetDirectoryName( fileName );
            var newFileName = $"{location}{Path.GetFileNameWithoutExtension( fileName )}_{firm.Name}_A2{Path.GetExtension( fileName )}";

            ExcelExporter.ExportData( newFileName, sheetData );
        }

        private static List< IExportingCell > MakeOpenAnswersReport ( IQuestionnaireBusinessContext context, Firm firm, Section[] sections )
        {
            var cells = new List< IExportingCell >();

            FillFirstRow();

            return cells;


            #region Functions

            void FillFirstRow()
            {
                cells[ 0 ] = new ExportingCell( firm.Name, 0, 0, H1Color );
            }

            #endregion
        }

        /// <summary>
        /// Makes multiple choice answers report.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="firm"></param>
        /// <param name="sections"></param>
        /// <returns></returns>
        private static List< IExportingCell > MakeMultipleChoiceAnswersReport ( IQuestionnaireBusinessContext context, Firm firm,  Section[] sections )
        {
            var cells = new List< IExportingCell >();

            var firmAnswers = context.GetMultipleChoiceAnswers().Where( f => f.FirmId == firm.Id ).ToArray();
            var employeeCount = firmAnswers.Select( a => a.Num ).Distinct().Count();

            if ( 0 == employeeCount ) return cells;

            var row = FillLeftTableHeader( cells, firm );


            // sections
            foreach ( var section in sections ) {
                row = FillLeftTableSectionHeader( row, cells, section );

                var questions = context.GetMultipleChoiceQuestions().Where( q => q.SectionId == section.Id )
                                       .OrderBy( q => q.Id ).ToArray();

                int questionNum = 1;

                // questions
                foreach ( var question in questions ) {
                    var questionAnswers = firmAnswers.Where( a => a.Question.Id == question.Id ).ToArray();

                    var yesAnswerCount = questionAnswers.Count( a => a.Answer == AnswerValueConverter.YES_UNSWER );
                    var yesAnswerPercent = ( double )yesAnswerCount / employeeCount;

                    var noAnswerCount = questionAnswers.Count( a => a.Answer == AnswerValueConverter.NO_ANSWER );
                    var noAnswerPercent = ( double )noAnswerCount / employeeCount;

                    var undefinedAnswerCount = questionAnswers.Count( a => a.Answer == AnswerValueConverter.UNDEFINED_ANSWER );
                    var undefinedAnswerPersent = ( double )undefinedAnswerCount / employeeCount;

                    ++row;
                    cells.Add( new ExportingCell( $"{section.Id}.{questionNum++}", row, 0, new byte[] { 0xa9, 0xd1, 0x8e } ) );
                    cells.Add( new ExportingCell( question.Text, row, 1, new byte[] { 0xe2, 0xf0, 0xd9 } ) );

                    cells.Add( new ExportingCell( yesAnswerCount, row, 2, new byte[] { 0xe2, 0xf0, 0xd9 } ) );
                    cells.Add( new ExportingCell( $"{yesAnswerPercent:N2}", row, 3, new byte[] { 0xe2, 0xf0, 0xd9 } ) );

                    cells.Add( new ExportingCell( noAnswerCount, row, 4, new byte[] { 0xe2, 0xf0, 0xd9 } ) );
                    cells.Add( new ExportingCell( $"{noAnswerPercent:N2}", row, 5, new byte[] { 0xe2, 0xf0, 0xd9 } ) );

                    cells.Add( new ExportingCell( undefinedAnswerCount, row, 6, new byte[] { 0xe2, 0xf0, 0xd9 } ) );
                    cells.Add( new ExportingCell( $"{undefinedAnswerPersent:N2}", row, 7, new byte[] { 0xe2, 0xf0, 0xd9 } ) );
                }
            }

            var answerGroups = context.GetGruppedMultipleChoiceAnswers()
                                      .Where( a => ( int )(a.GetType().GetProperty( "FirmId" ).GetValue( a )) == firm.Id )
                                      .OrderBy( a => ( int )(a.GetType().GetProperty( "EmployeeId" ).GetValue( a )) )
                                      .ToArray();

            if ( answerGroups.Any() ) {
                var answerType = answerGroups[ 0 ].GetType();
                var categories = context.GetCategories().ToArray();

                int columnDiff = 9;
                row = 0;

                FillRightTableTopHeader( cells, row, columnDiff );

                ++row;
                cells.Add( new ExportingCell( firm.Name, row, columnDiff + 0, null ) );
                cells.Add( new ExportingCell( employeeCount, row, columnDiff + 1, null ) );
                cells.Add( new ExportingCell( "?", row, columnDiff + 2, null ) );

                ++row;
                cells.Add( new ExportingCell( "№ кат.", row, columnDiff + 0, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                cells.Add( new ExportingCell( "Категория", row, columnDiff + 1, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                cells.Add( new ExportingCell( "План", row, columnDiff + 2, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                cells.Add( new ExportingCell( "Факт", row, columnDiff + 3, new byte[] { 0xb4, 0xc7, 0xe7 } ) );

                // Fill employee num headers
                for ( int i = 0; i < answerGroups.Length; ++i ) {
                    cells.Add( new ExportingCell( answerType.GetProperty( "EmployeeId" ).GetValue( answerGroups[ i ] ),
                                                 row,
                                                 columnDiff + 4 + i,
                                                 new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                }

                cells.Add( new ExportingCell( "Сумма", row, columnDiff + 4 + answerGroups.Length,
                                             new byte[] { 0xb4, 0xc7, 0xe7 } ) );

                ++row;

                var categorySums = new int[categories.Length];
                var categorySumRows = new int[categories.Length];

                // categories
                for ( var categoryIndex = 0; categoryIndex < categories.Length; ++categoryIndex ) {
                    categorySumRows[ categoryIndex ] = row;

                    // sections
                    foreach ( var section in categories[ categoryIndex ].Sections.OrderBy( s => s.Id ) ) {
                        // #Section | SectionName | План
                        cells.Add( new ExportingCell( section.Id, row, columnDiff + 0, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
                        cells.Add( new ExportingCell( section.Name, row, columnDiff + 1, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
                        cells.Add( new ExportingCell( 3.0, row, columnDiff + 2, new byte[] { 0xc5, 0xe0, 0xb4 } ) );

                        int sum = 0;

                        // employee answers
                        for ( int i = 0; i < answerGroups.Length; ++i ) {
                            IEnumerable< dynamic > answerCategories =
                                answerType.GetProperty( "Categories" ).GetValue( answerGroups[ i ] );
                            if ( !answerCategories.Any() ) goto loadSum;

                            var answerCategory =
                                answerCategories.FirstOrDefault( c => c.GetType().GetProperty( "CategoryId" ).GetValue( c ) ==
                                                                      categories[ categoryIndex ].Id );
                            if ( answerCategory == null ) goto loadSum;

                            IEnumerable< dynamic > answerSections =
                                answerCategory.GetType().GetProperty( "Sections" ).GetValue( answerCategory );
                            if ( !answerSections.Any() ) goto loadSum;

                            var answerSection =
                                answerSections.FirstOrDefault( s => s.GetType().GetProperty( "SectionId" ).GetValue( s ) ==
                                                                    section.Id );
                            if ( answerSection == null ) goto loadSum;

                            sum = answerSection.GetType().GetProperty( "AnswerSum" ).GetValue( answerSection );

                            loadSum:
                            cells.Add( new ExportingCell( sum, row, columnDiff + 4 + i, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                            categorySums[ categoryIndex ] += sum;
                        }

                        // employee answer sum
                        cells.Add( new ExportingCell( categorySums[ categoryIndex ], row, columnDiff + 4 + answerGroups.Length,
                                                     new byte[] { 0xb4, 0xc7, 0xe7 } ) );

                        var average = sum * 1.0 / (employeeCount * SECTION_COEFFICIENT);

                        // факт
                        cells.Add( new ExportingCell( $"{average:N2}", row, columnDiff + 3, null ) );

                        ++row;
                    }
                }

                cells.Add( new ExportingCell( "Факторный анализ", row, columnDiff + 0, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                ++row;
                cells.Add( new ExportingCell( "№ гр.", row, columnDiff + 0, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                cells.Add( new ExportingCell( "Группа факторов", row, columnDiff + 1, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                cells.Add( new ExportingCell( "План", row, columnDiff + 2, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                cells.Add( new ExportingCell( "Факт", row, columnDiff + 3, new byte[] { 0xb4, 0xc7, 0xe7 } ) );

                for ( var categoryIndex = 0; categoryIndex < categories.Length; ++categoryIndex ) {
                    cells.Add( new ExportingCell( categorySums[ categoryIndex ],
                                                 categorySumRows[ categoryIndex ],
                                                 columnDiff + 4 + answerGroups.Length + 1,
                                                 null ) );

                    ++row;

                    cells.Add( new ExportingCell( categories[ categoryIndex ].Id, row, columnDiff + 0,
                                                 new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    cells.Add( new ExportingCell( categories[ categoryIndex ].Name, row, columnDiff + 1,
                                                 new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    cells.Add( new ExportingCell( 3.0, row, columnDiff + 2, new byte[] { 0xb4, 0xc7, 0xe7 } ) );

                    var k = categories[ categoryIndex ].Sections.Count;
                    var averageCategory = categorySums[ categoryIndex ] * 1.0 / (employeeCount * SECTION_COEFFICIENT * k);

                    // факт
                    cells.Add( new ExportingCell( $"{averageCategory:N2}", row, columnDiff + 3, null ) );
                }
            }

            return cells;
        }

        /// <summary>
        /// Adds header into right table.
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="row"></param>
        /// <param name="columnDiff"></param>
        private static void FillRightTableTopHeader ( List< IExportingCell > cells, int row, int columnDiff )
        {
            cells.Add( new ExportingCell( "Филиал", row, columnDiff + 0, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
            cells.Add( new ExportingCell( "Количество \nсотрудников", row, columnDiff + 1, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
            cells.Add( new ExportingCell( "%Новых \nсотрудников", row, columnDiff + 2, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
        }

        /// <summary>
        /// Adds header into left table.
        /// </summary>
        /// <param name="cells"></param>
        /// <param name="firm"></param>
        /// <returns></returns>
        private static int FillLeftTableHeader ( List< IExportingCell > cells, Firm firm )
        {
            int row = 0;
            cells.Add( new ExportingCell( firm.Name, row, 0, new byte[] { 0xb4, 0xc7, 0xe7 } ) );

            ++row;
            cells.Add( new ExportingCell( "№", row, 0, new byte[] { 0xa9, 0xd1, 0x8e } ) );
            cells.Add( new ExportingCell( "Вопрос", row, 1, new byte[] { 0xa9, 0xd1, 0x8e } ) );
            cells.Add( new ExportingCell( "Варианты ответов", row, 2, new byte[] { 0xa9, 0xd1, 0x8e } ) );
            return row;
        }

        /// <summary>
        /// Adds section header into left table.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cells"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        private static int FillLeftTableSectionHeader ( int row, List< IExportingCell > cells, Section section )
        {
            ++row;
            cells.Add( new ExportingCell( section.Id, row, 0, new byte[] { 0xa9, 0xd1, 0x8e } ) );
            cells.Add( new ExportingCell( section.Name, row, 1, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
            cells.Add( new ExportingCell( "Да", row, 2, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
            cells.Add( new ExportingCell( "%", row, 3, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
            cells.Add( new ExportingCell( "Не знаю", row, 4, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
            cells.Add( new ExportingCell( "%", row, 5, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
            cells.Add( new ExportingCell( "Нет", row, 6, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
            cells.Add( new ExportingCell( "%", row, 7, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
            return row;
        }

    }
}
