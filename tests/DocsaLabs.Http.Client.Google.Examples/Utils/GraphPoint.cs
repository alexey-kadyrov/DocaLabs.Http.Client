using System;
using System.Globalization;

namespace DocsaLabs.Http.Client.Google.Examples.Utils
{
    [Serializable]
    public struct GraphPoint : IEquatable<GraphPoint>
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public GraphPoint(double x, double y)
            : this()
        {
            X = x;
            Y = y;
        }

        public double GetDistanceTo(GraphPoint other)
        {
            var d1 = X - other.X;
            var d2 = Y - other.Y;

            return Math.Sqrt(d1 * d1 + d2 * d2);
        }

        public static bool operator ==(GraphPoint left, GraphPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GraphPoint left, GraphPoint right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is GraphPoint && Equals((GraphPoint)obj);
        }

        public bool Equals(GraphPoint other)
        {
            return other.X.IsEqualTo(X) && other.Y.IsEqualTo(Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return CombineHashCodes(X.GetHashCode(), Y.GetHashCode());
            }
        }

        static int CombineHashCodes(int h1, int h2)
        {
            return (h1 << 5) + h1 ^ h2;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0},{1}", X, Y);
        }
    }
}
