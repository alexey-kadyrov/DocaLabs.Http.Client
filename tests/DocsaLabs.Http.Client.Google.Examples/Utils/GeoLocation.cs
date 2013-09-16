using System;
using System.Collections.Specialized;
using System.Globalization;
using DocaLabs.Http.Client.Binding.PropertyConverting;

namespace DocsaLabs.Http.Client.Google.Examples.Utils
{
    [Serializable]
    public struct GeoLocation : IEquatable<GeoLocation>, ICustomValueConverter
    {
        /// <summary>
        /// The average radius for a spherical approximation of the figure of the Earth is approximately 6371,010 meters.
        /// </summary>
        public const double EarthRadius = 6371010;

        static readonly double MinLatRad = DegreeToRadian(-90d);  // -PI/2
        static readonly double MaxLatRad = DegreeToRadian(90d);   //  PI/2
        static readonly double MinLonRad = DegreeToRadian(-180d); // -PI
        static readonly double MaxLonRad = DegreeToRadian(180d);  //  PI

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public GeoLocation(double latitude, double longitude)
            : this()
        {
            Initialize(latitude, longitude);
        }

        public GeoLocation(string value)
            : this()
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("value");

            var cc = value.Split(',');
            if (cc.Length != 2)
                throw new FormatException("Wrong format " + value);

            Initialize(double.Parse(cc[0]), double.Parse(cc[1]));
        }

        public static GeoLocation FromRadians(double latitude, double longitude)
        {
            return new GeoLocation(RadianToDegree(latitude), RadianToDegree(longitude));
        }

        /// <summary>
        /// Computes the great circle distance between this GeoLocation instance and the location argument.
        /// </summary>
        /// <seealso cref="http://JanMatuschek.de/LatitudeLongitudeBoundingCoordinates"/>
        /// <returns>The distance, measured in the same unit as the Earth's radius.</returns>
        public double GetDistanceTo(GeoLocation other)
        {
            if (Equals(other))
                return 0d;

            var radLat = DegreeToRadian(Latitude);
            var radLon = DegreeToRadian(Longitude);

            var otherRadLat = DegreeToRadian(other.Latitude);
            var otherRadLon = DegreeToRadian(other.Longitude);

            var distance = Math.Acos(
                            Math.Sin(radLat) * Math.Sin(otherRadLat) +
                            Math.Cos(radLat) * Math.Cos(otherRadLat) *
                            Math.Cos(radLon - otherRadLon)) * EarthRadius;

            return double.IsNaN(distance) ? 0d : Math.Abs(distance);
        }

        /// <summary>
        /// <p>Computes the bounding coordinates of all points on the surface
        /// of a sphere that have a great circle distance to the point represented
        /// by this GeoLocation instance that is less or equal to the distance
        /// argument.
        /// For more information about the formulae used in this method visit
        /// <a href="http://JanMatuschek.de/LatitudeLongitudeBoundingCoordinates">http://JanMatuschek.de/LatitudeLongitudeBoundingCoordinates</a>.</p>
        /// </summary>
        /// <param name="distance">Distance the distance from the point represented by this GeoLocation instance. Must me measured in the same unit as the Earth's radius.</param>
        /// <returns>
        /// An array of two GeoLocation objects such that:<ul>
        /// <li>The latitude of any point within the specified distance is greater
        /// or equal to the latitude of the first array element and smaller or
        /// equal to the latitude of the second array element.</li>
        /// <li>If the longitude of the first array element is smaller or equal to
        /// the longitude of the second element, then
        /// the longitude of any point within the specified distance is greater
        /// or equal to the longitude of the first array element and smaller or
        /// equal to the longitude of the second array element.</li>
        /// <li>If the longitude of the first array element is greater than the
        /// longitude of the second element (this is the case if the 180th
        /// meridian is within the distance), then
        /// the longitude of any point within the specified distance is greater
        /// or equal to the longitude of the first array element
        /// <strong>or</strong> smaller or equal to the longitude of the second
        /// array element.</li>
        /// </ul>
        /// </returns>
        public GeoBoundary GetBoundingCoordinates(double distance)
        {
            if (distance <= 0d)
                throw new ArgumentOutOfRangeException("distance", @"Distance must be greater than zero.");

            var radLat = DegreeToRadian(Latitude);
            var radLon = DegreeToRadian(Longitude);

            // angular distance in radians on a great circle
            var radDist = distance / EarthRadius;

            var minLat = radLat - radDist;
            var maxLat = radLat + radDist;

            double minLon, maxLon;
            if (minLat > MinLatRad && maxLat < MaxLatRad)
            {
                var deltaLon = Math.Asin(Math.Sin(radDist) / Math.Cos(radLat));
                minLon = radLon - deltaLon;

                if (minLon < MinLonRad)
                    minLon += 2d * Math.PI;

                maxLon = radLon + deltaLon;
                if (maxLon > MaxLonRad)
                    maxLon -= 2d * Math.PI;
            }
            else
            {
                // a pole is within the distance
                minLat = Math.Max(minLat, MinLatRad);
                maxLat = Math.Min(maxLat, MaxLatRad);
                minLon = MinLonRad;
                maxLon = MaxLonRad;
            }

            // left,bottom - right, top
            return new GeoBoundary(FromRadians(minLat, minLon), FromRadians(maxLat, maxLon));
        }

        void Initialize(double latitude, double longitude)
        {
            if (latitude < -90d || latitude > 90d)
                throw new ArgumentOutOfRangeException("latitude", latitude, @"Latitude must be between -90 and 90.");

            if (longitude < -180d || longitude > 180d)
                throw new ArgumentOutOfRangeException("longitude", longitude, @"Longitude must be between -180 and 180.");

            Latitude = latitude;
            Longitude = longitude;
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180d;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180d / Math.PI);
        }

        public static bool operator ==(GeoLocation left, GeoLocation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeoLocation left, GeoLocation right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is GeoLocation && Equals((GeoLocation)obj);
        }

        public bool Equals(GeoLocation other)
        {
            return other.Latitude.IsEqualTo(Latitude) && other.Longitude.IsEqualTo(Longitude);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return CombineHashCodes(Latitude.GetHashCode(), Longitude.GetHashCode());
            }
        }

        static int CombineHashCodes(int h1, int h2)
        {
            return (h1 << 5) + h1 ^ h2;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0},{1}", Latitude, Longitude);
        }

        public NameValueCollection ConvertProperties()
        {
            return new NameValueCollection { { "", string.Format(CultureInfo.InvariantCulture, "{0},{1}", Latitude, Longitude) } };
        }
    }
}
