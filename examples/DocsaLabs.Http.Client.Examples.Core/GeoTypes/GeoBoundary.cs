using System;

namespace DocsaLabs.Http.Client.Examples.Core.GeoTypes
{
    public struct GeoBoundary : IEquatable<GeoBoundary>
    {
        public double North { get; private set; }

        public double South { get; private set; }

        public double West { get; private set; }

        public double East { get; private set; }

        public GeoBoundary(GeoLocation southWest, GeoLocation northEast) : this()
        {
            Initialize(southWest, northEast);
        }

        public GeoBoundary(string value) : this()
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("value");

            var corners = value.Split('|');
            if(corners.Length != 2)
                throw new FormatException("Wrong format: " + value);

            Initialize(new GeoLocation(corners[0]), new GeoLocation(corners[1]));
        }

        public bool Contains(GeoLocation location)
        {
            return location.Latitude <= North && location.Latitude >= South && 
                     location.Longitude >= West && location.Longitude <= East;
        }

        /// <summary>
        /// Determines if this boundary intersects with <paramref name="boundary"/>.
        /// </summary>
        /// <param name="boundary">The boundary to test.</param>
        /// <returns>This method returns true if there is any intersection, otherwise false.</returns>
        public bool IntersectsWith(GeoBoundary boundary)
        {
            return boundary.West < East && West < boundary.East && boundary.South < North && South < boundary.North;
        }

        public GeoLocation GetSouthEast()
        {
            return new GeoLocation(South, East);
        }

        public GeoLocation GetNorthEast()
        {
            return new GeoLocation(North, East);
        }

        public GeoLocation GetNorthWest()
        {
            return new GeoLocation(North, West);
        }

        public GeoLocation GetSouthWest()
        {
            return new GeoLocation(South, West);
        }

        public double GetLatitudeSize()
        {
            return GetNorthWest().GetDistanceTo(GetSouthWest());
        }

        public double GetLongitudeSize()
        {
            return GetNorthWest().GetDistanceTo(GetNorthEast());
        }

        public double GetInnerRadius()
        {
            return Math.Min(GetLongitudeSize(), GetLatitudeSize()) / 2d;
        }

        public double GetOuterRadius()
        {
            var lat = GetLatitudeSize() / 2d;
            var lon = GetLongitudeSize() / 2d;

            return Math.Sqrt(lat * lat + lon * lon);
        }

        public GeoLocation GetCentre()
        {
            return new GeoLocation(
                South + (North - South) / 2d, West + (East - West) / 2d);
        }

        public double GetLatitudeDegrees()
        {
            return North - South;
        }

        public double GetLongitudeDegrees()
        {
            return East - West;
        }

        public static bool operator == (GeoBoundary left, GeoBoundary right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GeoBoundary left, GeoBoundary right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is GeoBoundary && Equals((GeoBoundary) obj);
        }

        public bool Equals(GeoBoundary other)
        {
            return other.West.IsEqualTo(West) && other.North.IsEqualTo(North) && other.East.IsEqualTo(East) && other.South.IsEqualTo(South);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return CombineHashCodes(West.GetHashCode(), North.GetHashCode(), East.GetHashCode(), South.GetHashCode());
            }
        }

        static int CombineHashCodes(int h1, int h2)
        {
            return (h1 << 5) + h1 ^ h2;
        }

        static int CombineHashCodes(int h1, int h2, int h3, int h4)
        {
            return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}", GetSouthWest(), GetNorthEast());
        }

        void Initialize(GeoLocation southWest, GeoLocation northEast)
        {
            West = southWest.Longitude;
            North = northEast.Latitude;
            East = northEast.Longitude;
            South = southWest.Latitude;
        }
    }
}
