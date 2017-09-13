using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Configuration;

using System.IO;

using Geocoding.Google;

namespace CarFixed.Core
{
    public class GeoLocationService
    {
        public static PostcodeLatitudeLongitude GetLatitudeAndLongitude(string postcode)
        {
            //TODO - Remove this before go live - Override so not to overload GoogleApi request limit during seeding.
            if (string.IsNullOrWhiteSpace(postcode)) return new PostcodeLatitudeLongitude();
            //if (postcode.Replace(" ", "").Equals("BS361PQ", StringComparison.InvariantCultureIgnoreCase))
            //    return new PostcodeLatitudeLongitude {Latitude = 51.5293306, Longitude = -2.5011335};
            //if (postcode.Replace(" ", "").Equals("test", StringComparison.InvariantCultureIgnoreCase))
            //    return new PostcodeLatitudeLongitude {Latitude = 41.2925714, Longitude = -73.6794265};
            try
            {
                var geocoder = new GoogleGeocoder { ApiKey = ConfigurationManager.AppSettings["GoogleApiKey"], RegionBias = "UK" };
                var addresses = geocoder.Geocode(postcode).ToList();
                if (addresses.Any())
                {
                    return new PostcodeLatitudeLongitude
                    {
                        Latitude = addresses.First().Coordinates.Latitude,
                        Longitude = addresses.First().Coordinates.Longitude,
                    };
                }
            }
            catch (Exception ex)
            {
                //failed to get postcode
            }
            return new PostcodeLatitudeLongitude();
        }

        //public double GetDistanceFromLatLonInMiles(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
        //{
        //    var sCoord = new GeoCoordinate(originLatitude, originLongitude);
        //    var eCoord = new GeoCoordinate(destinationLatitude, destinationLongitude);
        //    return sCoord.GetDistanceTo(eCoord) * 0.621371192; //KM -> miles
        //}

        public double GetDrivingDistanceInMiles(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
        {
            var url =
                string.Format(
                    "http://maps.googleapis.com/maps/api/distancematrix/xml?origins={0},{1}&destinations={2},{3}&mode=driving&sensor=false&language=en-EN&units=imperial",
                    originLatitude, originLongitude, destinationLatitude, destinationLongitude);

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = request.GetResponse();
            var dataStream = response.GetResponseStream();
            var sreader = new StreamReader(dataStream);
            var responsereader = sreader.ReadToEnd();
            response.Close();
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);

            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText != "OK") return 0;
            var distance = xmldoc.GetElementsByTagName("distance");
            return Convert.ToDouble(distance[0].ChildNodes[1].InnerText.Replace(" mi", ""));
        }
    }

    public class PostcodeLatitudeLongitude
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public bool HasValue
        {
            get { return Latitude.HasValue && Longitude.HasValue; }
        }

    }
}
