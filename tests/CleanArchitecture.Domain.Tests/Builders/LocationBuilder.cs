using CleanArchitecture.Domain.Locations.Entities;
using CleanArchitecture.Domain.Locations.ValueObjects;

namespace CleanArchitecture.Domain.Tests.Builders
{
    public class LocationBuilder
    {
        private string _country = "United Kingdom";
        private string _city = "London";
        private decimal _latitude = 51.51m;
        private decimal _longitude = -0.13m;

        public Location Build()
        {
            return Location.Create(_country, _city, Coordinates.Create(_latitude, _longitude));
        }

        public LocationBuilder WithCity(string city)
        {
            _city = city;
            return this;
        }

        public LocationBuilder WithLatitude(decimal latitude)
        {
            _latitude = latitude;
            return this;
        }
    }
}
