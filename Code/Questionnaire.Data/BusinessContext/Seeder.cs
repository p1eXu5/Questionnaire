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

        public static SheetTable RegionSheetTable { get; private set; }
        public static SheetTable CitySheetTable { get; private set; }
        public static SheetTable FirmTypeSheetTable { get; private set; }
        public static SheetTable FirmSheetTable { get; private set; }

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
    }
}
