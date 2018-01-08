using System;
using System.Collections.Generic;
using System.Linq;
using Geocoding;
using Geocoding.Google;

namespace MejorPrecio3.Services
{
    public class Geocoder
    {

        private IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyCjP-q3fTsUk-z0pPGVlnmW_LilMGX4k6c" };
        public Tuple<double, double> GetLatLong(string adress)
        {
            IEnumerable<Address> addresses = geocoder.GeocodeAsync(adress + "Buenos Aires Argentina").Result;
            var tuple = new Tuple<double, double>(addresses.First().Coordinates.Latitude, addresses.First().Coordinates.Longitude);
            return tuple;
        }

        public string GetAdress(double latitude, double longitude)
        {
            IEnumerable<Address> addresses = geocoder.ReverseGeocodeAsync(latitude,longitude).Result;
            return addresses.First().FormattedAddress;
        }
    }
}