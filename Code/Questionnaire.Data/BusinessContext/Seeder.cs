using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public static class Seeder
    {
        public static string FileNameRegions { get; set; } = @"SeedFiles/Regions.xlsx";
        public static string FileNameCities { get; set; } = @"SeedFiles/Cities.xlsx";
        public static string FileNameFirmTypes { get; set; } = @"SeedFiles/FirmTypes.xlsx";
        public static string FileNameFirms { get; set; } = @"SeedFiles/Firms.xlsx";

        public static string FileNameCategories { get; set; } = @"SeedFiles/Categories.xlsx";
        public static string FileNameSections { get; set; } = @"SeedFiles/Sections.xlsx";
        public static string FileNameQuestionMultipleChoiceList { get; set; } = @"SeedFiles/QuestionMultipleChoiceList.xlsx";
        public static string FileNameQuestionOpenList { get; set; } = @"SeedFiles/QuestionOpenList.xlsx";


        public static SheetTable RegionSheetTable { get; private set; }
        public static SheetTable CitySheetTable { get; private set; }
        public static SheetTable FirmTypeSheetTable { get; private set; }
        public static SheetTable FirmSheetTable { get; private set; }

        public static SheetTable CategoryTable { get; private set; }
        public static SheetTable SectionTable { get; private set; }
        public static SheetTable QuestionOpenTable { get; private set; }
        public static SheetTable QuestionMultipleChoiceTable { get; private set; }
        

        public static IEnumerable< Region > GetRegions ()
        {
            try {
                if ( RegionSheetTable == default( SheetTable ) ) {
                    RegionSheetTable = ExcelImporter.ImportData( FileNameRegions );
                }

                return ExcelImporter.GetEnumerable< Region >( RegionSheetTable );
            }
            catch {
                return new Region[0];
            }
        }

        public static IEnumerable< City > GetCities ()
        {
            try {
                if ( CitySheetTable == default( SheetTable ) ) {
                    CitySheetTable = ExcelImporter.ImportData( FileNameCities );
                }

                return ExcelImporter.GetEnumerable< City >( CitySheetTable );
            }
            catch {
                return new City[0];
            }
        }

        public static IEnumerable< FirmType > GetFirmTypes ()
        {
            try {
                if ( FirmTypeSheetTable == default( SheetTable ) ) {
                    FirmTypeSheetTable = ExcelImporter.ImportData( FileNameFirmTypes );
                }

                return ExcelImporter.GetEnumerable< FirmType >( FirmTypeSheetTable );
            }
            catch {
                return new FirmType[0];
            }
        }

        public static IEnumerable< Firm > GetFirms ()
        {
            try {
                if ( FirmSheetTable == default( SheetTable ) ) {
                    FirmSheetTable = ExcelImporter.ImportData( FileNameFirms );
                }

                return ExcelImporter.GetEnumerable< Firm >( FirmSheetTable );
            }
            catch {
                return new Firm[0];
            }
        }

        public static IEnumerable< Category > GetCategories ()
        {
            try {
                if ( CategoryTable == default( SheetTable ) ) {
                    CategoryTable = ExcelImporter.ImportData( FileNameCategories );
                }

                return ExcelImporter.GetEnumerable< Category >( CategoryTable );
            }
            catch {
                return new Category[0];
            }
        }

        public static IEnumerable< Section > GetSections ()
        {
            try {
                if ( SectionTable == default( SheetTable ) ) {
                    SectionTable = ExcelImporter.ImportData( FileNameSections );
                }

                return ExcelImporter.GetEnumerable< Section >( SectionTable );
            }
            catch {
                return new Section[0];
            }
        }

        public static IEnumerable< QuestionMultipleChoice > GetQuestionMultipleChoiceList ()
        {
            try {
                if ( QuestionMultipleChoiceTable == default( SheetTable ) ) {
                    QuestionMultipleChoiceTable = ExcelImporter.ImportData( FileNameQuestionMultipleChoiceList );
                }

                return ExcelImporter.GetEnumerable< QuestionMultipleChoice >( QuestionMultipleChoiceTable );
            }
            catch {
                return new QuestionMultipleChoice[0];
            }
        }

        public static IEnumerable< QuestionOpen > GetQuestionOpenList ()
        {
            try {
                if ( QuestionOpenTable == default( SheetTable ) ) {
                    QuestionOpenTable = ExcelImporter.ImportData( FileNameQuestionOpenList );
                }

                return ExcelImporter.GetEnumerable< QuestionOpen >( QuestionOpenTable );
            }
            catch {
                return new QuestionOpen[0];
            }
        }
    }
}
