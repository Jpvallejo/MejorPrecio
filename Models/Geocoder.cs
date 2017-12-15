using System;
using System.Collections.Generic;
using System.Linq;
using Geocoding;
using Geocoding.Google;

namespace mejor_precio_3.Models
{
    public class Geocoder
    {

        public Tuple<double,double> GetLatLong(string adress)
        {
        IGeocoder geocoder = new GoogleGeocoder() { ApiKey = "AIzaSyCjP-q3fTsUk-z0pPGVlnmW_LilMGX4k6c" };
        IEnumerable<Address> addresses = geocoder.GeocodeAsync(adress + "Buenos Aires Argentina").Result;
        var tuple = new Tuple<double,double>(addresses.First().Coordinates.Latitude, addresses.First().Coordinates.Longitude);
        return tuple;
        }
    }
}