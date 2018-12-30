using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Questionnaire.Data.BusinessContext;

namespace Questionnaire.Data.Tests
{
    [ TestFixture ]
    public class SeederIntegrationalTests
    {
        [ Test ]
        public void GetFirms_FileExists_Returns243Firms ()
        {
            var res = Seeder.GetFirms();

            Assert.That( 243 == res.Count() );
        }

        [ Test ]
        public void GetFirms_FileDoesNotExists_Throws ()
        {
            bool ok = true;

            try {
                File.Move( @"/../../../../Code/Questionnaire.Data/SeedFiles/Firms.xlsx",
                          @"/../../../../Code/Questionnaire.Data/SeedFiles/Firms.xlsx1" );
            }
            catch ( DirectoryNotFoundException ) {
                Debug.WriteLine( "Path invalid" );
                throw;
            }
            catch ( IOException ) {
                ok = false;
            }

            var res = Seeder.GetFirms();

            if ( ok ) {
                File.Move( @"/../../../../Code/Questionnaire.Data/SeedFiles/Firms.xlsx1",
                          @"/../../../../Code/Questionnaire.Data/SeedFiles/Firms.xlsx" );
            }

        }

        [ Test ]
        public void GetFirms_FileAccessDenied_Throws ()
        {
            FileStream fs = null;

            try {
                fs = new FileStream( @"/../../../../Code/Questionnaire.Data/SeedFiles/Firms.xlsx",
                                               FileMode.Open );
            }
            finally {
                fs?.Close();
            }

        }

    }
}
