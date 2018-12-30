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
        static Seeder ()
        {
            RegionSheetTable = ExcelImporter.ImportData( @"SeedFiles/Regions.xlsx" );
            CitySheetTable = ExcelImporter.ImportData( @"SeedFiles/Cities.xlsx" );
            FirmTypeSheetTable = ExcelImporter.ImportData( @"SeedFiles/FirmTypes.xlsx" );
            FirmSheetTable = ExcelImporter.ImportData( @"SeedFiles/FirmTypes.xlsx" );
        }

        public static SheetTable RegionSheetTable { get; private set; }
        public static SheetTable CitySheetTable { get; private set; }
        public static SheetTable FirmTypeSheetTable { get; private set; }
        public static SheetTable FirmSheetTable { get; private set; }

        public static IEnumerable< Firm > GetRegions () => ExcelImporter.GetEnumerable< Firm >( RegionSheetTable );
        public static IEnumerable< City > GetCities () => ExcelImporter.GetEnumerable< City >( CitySheetTable );
        public static IEnumerable< FirmType > GetFirmTypes () => ExcelImporter.GetEnumerable< FirmType >( FirmTypeSheetTable );
        public static IEnumerable< Firm > GetFirms () => ExcelImporter.GetEnumerable< Firm >( FirmSheetTable );
    }
}
