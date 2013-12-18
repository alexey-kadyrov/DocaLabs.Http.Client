using System;

namespace DocaLabs.Http.Client.Utils
{
    /// <summary>
    /// Compares whenever a given value is null taking into consideration DBNull.Value. 
    /// </summary>
    public class NullComparerOverride : INullComparer
    {
        /// <summary>
        /// Returns true if the value is null or DBNull.Value.
        /// </summary>
        public bool IsNull(object value)
        {
            return value == null || value == DBNull.Value;
        }
    }
}
