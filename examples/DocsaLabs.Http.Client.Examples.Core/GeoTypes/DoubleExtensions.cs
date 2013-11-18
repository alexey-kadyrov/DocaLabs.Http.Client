using System;

namespace DocsaLabs.Http.Client.Examples.Core.GeoTypes
{
    public static class DoubleExtensions
    {
        public static bool IsEqualTo(this double left, double right, double error = double.Epsilon)
        {
            return Math.Abs(left - right) <= error;
        }

        public static bool IsZero(this double left)
        {
            return left.IsEqualTo(0d);
        }

        public static bool IsLessOrEqualThan(this double left, double right)
        {
            return left <= right + double.Epsilon;
        }
    }
}
