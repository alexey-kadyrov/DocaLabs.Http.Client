using System;

namespace DocaLabs.Http.Client.Utils
{
    public class NullComparerOverride : INullComparer
    {
        public bool IsNull(object value)
        {
            return value == null || value == DBNull.Value;
        }
    }
}
