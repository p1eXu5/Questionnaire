using System;
using System.Collections.Generic;
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
        public static void MakeReport ( string fileName,  IQuestionnaireBusinessContext context,  ReportOrientation orientation = ReportOrientation.Horizontal )
        {
            var firms = context.GetMultipleChoiceAnswers().Select( a => a.Firm ).Distinct().ToArray();
            var sections = context.GetSections().ToArray();

            var cells = new List< IExportingCell >();

            // firms
            foreach ( var firm in firms ) {

                var firmAnswers = context.GetMultipleChoiceAnswers().Where( f => f.FirmId == firm.Id ).ToArray();
                var employeeCount = firmAnswers.Select( a => a.Num ).Distinct().Count();

                var row = FillFirmHeader( cells, firm );


                // sections
                foreach ( var section in sections ) {

                    row = FillSectionHeader( row, cells, section );

                    var questions = context.GetMultipleChoiceQuestions().Where( q => q.SectionId == section.Id )
                                           .OrderBy( q => q.Id ).ToArray();

                    int questionNum = 1;

                    // questions
                    foreach ( var question in questions ) {

                        var questionAnswers = firmAnswers.Where( a => a.Question.Id == question.Id ).ToArray();

                        var yesAnswerCount = questionAnswers.Count( a => a.Answer == AnswerValueConverter.YES_UNSWER );
                        var yesAnswerPercent = (double)yesAnswerCount / employeeCount;

                        var noAnswerCount = questionAnswers.Count( a => a.Answer == AnswerValueConverter.NO_ANSWER );
                        var noAnswerPercent = (double)noAnswerCount / employeeCount;

                        var undefinedAnswerCount = questionAnswers.Count( a => a.Answer == AnswerValueConverter.UNDEFINED_ANSWER );
                        var undefinedAnswerPersent = (double)undefinedAnswerCount / employeeCount;

                        ++row;
                        cells.Add( new ExportingCell( $"{section.Id}.{questionNum++}", row, 0, new byte[]{ 0xa9, 0xd1, 0x8e } ) );
                        cells.Add( new ExportingCell( question.Text, row, 1, new byte[]{ 0xe2, 0xf0, 0xd9 } ) );

                        cells.Add( new ExportingCell( yesAnswerCount, row, 2, new byte[]{ 0xe2, 0xf0, 0xd9 } ) );
                        cells.Add( new ExportingCell( $"{yesAnswerPercent:N2}", row, 3, new byte[]{ 0xe2, 0xf0, 0xd9 } ) );

                        cells.Add( new ExportingCell( noAnswerCount, row, 4, new byte[]{ 0xe2, 0xf0, 0xd9 } ) );
                        cells.Add( new ExportingCell( $"{noAnswerPercent:N2}", row, 5, new byte[]{ 0xe2, 0xf0, 0xd9 } ) );

                        cells.Add( new ExportingCell( undefinedAnswerCount, row, 6, new byte[]{ 0xe2, 0xf0, 0xd9 } ) );
                        cells.Add( new ExportingCell( $"{undefinedAnswerPersent:N2}", row, 7, new byte[]{ 0xe2, 0xf0, 0xd9 } ) );
                    }
                }

                var answerGroups = context.GetGruppedMultipleChoiceAnswers().Where( a => (int)(a.GetType().GetProperty( "FirmId" ).GetValue( a )) == firm.Id )
                                          .OrderBy( a => (int)(a.GetType().GetProperty( "EmployeeId" ).GetValue( a )) ).ToArray();

                if ( answerGroups.Any() ) {

                    var answerType = answerGroups[ 0 ].GetType();
                    var categories = context.GetCategories();

                    row = 0;
                    int columnDiff = 9;

                    cells.Add( new ExportingCell( "Филиал", row, columnDiff + 0, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    cells.Add( new ExportingCell( "Количество\nсотрудников", row, columnDiff + 1, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    cells.Add( new ExportingCell( "%Новых\nсотрудников", row, columnDiff + 2, new byte[] { 0xb4, 0xc7, 0xe7 } ) );

                    ++row;
                    cells.Add( new ExportingCell( firm.Name, row, columnDiff + 0, null ) );
                    cells.Add( new ExportingCell( employeeCount, row, columnDiff + 1, null ) );
                    cells.Add( new ExportingCell( "?", row, columnDiff + 2, null ) );

                    ++row;
                    cells.Add( new ExportingCell( "№ гр.", row, columnDiff + 0, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    cells.Add( new ExportingCell( "Группа факторов", row, columnDiff + 1, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    cells.Add( new ExportingCell( "№ кат.", row, columnDiff + 2, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    cells.Add( new ExportingCell( "Категория", row, columnDiff + 3, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    cells.Add( new ExportingCell( "План", row, columnDiff + 4, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    cells.Add( new ExportingCell( "Факт", row, columnDiff + 5, new byte[] { 0xb4, 0xc7, 0xe7 } ) );

                    for ( int i = 0; i < answerGroups.Length; ++i ) {
                        cells.Add( new ExportingCell( answerType.GetProperty( "EmployeeId" ).GetValue( answerGroups[ i ] ), 
                                                     row, 
                                                     columnDiff + 6 + i, 
                                                     new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                    }

                    cells.Add( new ExportingCell( "Сумма", row, columnDiff + 6 + answerGroups.Length, new byte[] { 0xb4, 0xc7, 0xe7 } ) );

                    ++row;

                    foreach ( var category in categories ) {

                        cells.Add( new ExportingCell( category.Id, row, columnDiff + 0, new byte[] { 0xa9, 0xd1, 0x8e } ) );
                        cells.Add( new ExportingCell( category.Name, row, columnDiff + 1, new byte[] { 0xc5, 0xe0, 0xb4 } ) );



                        foreach ( var section in category.Sections.OrderBy( s => s.Id ) ) {
                            
                            cells.Add( new ExportingCell( section.Id, row, columnDiff + 2, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
                            cells.Add( new ExportingCell( section.Name, row, columnDiff + 3, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
                            cells.Add( new ExportingCell( section.Name, row, columnDiff + 3, new byte[] { 0xc5, 0xe0, 0xb4 } ) );
                            cells.Add( new ExportingCell( section.Name, row, columnDiff + 3, new byte[] { 0xc5, 0xe0, 0xb4 } ) );

                            int sectionSum = 0;

                            for ( int i = 0; i < answerGroups.Length; ++i ) {

                                int sum = 0;

                                IEnumerable< dynamic > answerCategories = answerType.GetProperty( "Categories" ).GetValue( answerGroups[ i ] );
                                if ( !answerCategories.Any() ) goto loadSum;

                                var answerCategory = answerCategories.FirstOrDefault( c => c.GetType().GetProperty( "CategoryId" ).GetValue( c ) == category.Id );
                                if ( answerCategory == null ) goto loadSum;

                                IEnumerable< dynamic > answerSections = answerCategory.GetType().GetProperty( "Sections" ).GetValue( answerCategory );
                                if ( !answerSections.Any() ) goto loadSum;

                                var answerSection = answerSections.FirstOrDefault( s => s.GetType().GetProperty( "SectionId" ).GetValue( s ) == section.Id);
                                if ( answerSection == null ) goto loadSum;

                                sum = answerSection.GetType().GetProperty( "AnswerSum" ).GetValue( answerSection );

                                loadSum:
                                cells.Add( new ExportingCell( sum, row, columnDiff + 6 + i, new byte[] { 0xb4, 0xc7, 0xe7 } ) );
                                sectionSum += sum;
                            }

                            cells.Add( new ExportingCell( sectionSum, row, columnDiff + 6 + answerGroups.Length, new byte[] { 0xb4, 0xc7, 0xe7 } ) );

                            ++row;
                        }
                    }
                }

                var location = Path.GetDirectoryName( fileName );
                var newFileName = $"{location}{Path.GetFileNameWithoutExtension( fileName )}_{firm.Name}{Path.GetExtension( fileName )}";

                ExcelExporter.ExportData( newFileName, cells );
            }
        }

        private static int FillFirmHeader ( List< IExportingCell > cells, Firm firm )
        {
            int row = 0;
            cells.Add( new ExportingCell( firm.Name, row, 0, new byte[] { 0xb4, 0xc7, 0xe7 } ) );

            ++row;
            cells.Add( new ExportingCell( "№", row, 0, new byte[] { 0xa9, 0xd1, 0x8e } ) );
            cells.Add( new ExportingCell( "Вопрос", row, 1, new byte[] { 0xa9, 0xd1, 0x8e } ) );
            cells.Add( new ExportingCell( "Варианты ответов", row, 2, new byte[] { 0xa9, 0xd1, 0x8e } ) );
            return row;
        }

        private static int FillSectionHeader ( int row, List< IExportingCell > cells, Section section )
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
