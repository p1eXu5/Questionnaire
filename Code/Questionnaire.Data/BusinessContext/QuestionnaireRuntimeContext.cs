using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionnaire.Data.Models;

namespace Questionnaire.Data.BusinessContext
{
    public class QuestionnaireRuntimeContext : IQuestionnaireBusinessContext
    {
        private City[] _cities;
        private Firm[] _firms;
        private readonly List< QuestionBase > _questions;
        private readonly List< Section > _sections;
        private readonly List< AnswerMultipleChoice > _answers;

        public QuestionnaireRuntimeContext ()
        {
            Seed();
        }

        public Region[] GetRegions ()
        {
            throw new NotImplementedException();
        }

        public Firm[] GetFirms ()
        {
            return _firms.OrderBy( f => f.Name ).ToArray();
        }

        public City[] GetCities ()
        {
            return _cities.OrderBy( c => c.Name ).ToArray();
        }

        public Section[] GetSections ()
        {
            throw new NotImplementedException();
        }

        private void Seed ()
        {
            _cities = GetCities( Seeder.GetRegions().ToArray() );
            _firms = GetFirms( _cities, Seeder.GetFirmTypes().ToArray() );
        }

        private City[] GetCities ( Region[] regions )
        {
            var unknownRegion = new Region { Id = 1, Name = "-" };
            var unknownCity = new City { Id = 1, Name = "-", Region = unknownRegion, RegionId = unknownRegion.Id };
            unknownRegion.CityCollection.Add( unknownCity );

            var cities = Seeder.GetCities().ToList();

            if ( cities.Count > 0 ) {

                foreach ( var city in cities ) {

                    if ( city.Id <= 0 ) city.Id = cities.Max( c => c.Id ) + 1;

                    var region = regions.FirstOrDefault( r => r.Id == city.RegionId );

                    if ( region == null || region.Id <= 1 ) { 
                        region = unknownRegion;
                    }

                    city.Region = region;
                    city.RegionId = region.Id;
                    region.CityCollection.Add( city );
                }

                var city1 = cities.FirstOrDefault( c => c.Id == 1 );

                if ( city1 == null ) {
                    cities.Add( unknownCity );
                }

                return cities.OrderBy( c => c.Id ).ToArray();
            }

            return new[] { unknownCity };
        }

        private Firm[] GetFirms ( City[] cities, FirmType[] firmTypes )
        {
            var unknownFirmType = new FirmType { Id = 1, Name = "-" };
            var unknownFirm = new Firm { Id = 1, Name = "-", City = cities[0], CityId = cities[0].Id, FirmType = unknownFirmType, FirmTypeId = unknownFirmType.Id };
            cities[0].FirmCollection.Add( unknownFirm );
            unknownFirmType.FirmCollection.Add( unknownFirm );

            var firms = Seeder.GetFirms().ToList();

            if ( firms.Count > 0 ) {

                foreach ( var firm in firms ) {

                    if ( firm.Id <= 0 ) firm.Id = firms.Max( f => f.Id ) + 1;

                    var city = _cities.FirstOrDefault( c => firm.CityId == c.Id );

                    if ( city == null || firm.CityId <= 1 ) {
                        city = _cities[0];
                    }

                    var firmType = firmTypes.FirstOrDefault( f => firm.FirmTypeId == f.Id );

                    if ( firmType == null || firmType.Id <= 1 ) {
                        firmType = unknownFirmType;
                    }

                    firm.City = city;
                    firm.CityId = city.Id;
                    city.FirmCollection.Add( firm );

                    firm.FirmType = firmType;
                    firm.FirmTypeId = firmType.Id;
                    firmType.FirmCollection.Add( firm );
                }

                var firm1 = firms.FirstOrDefault( f => f.Id == 1 );

                if ( firm1 == null ) {
                    firms.Add( unknownFirm );
                }

                return firms.OrderBy( f => f.Id ).ToArray();
            }

            return new[] { unknownFirm };
        }
    }
}
