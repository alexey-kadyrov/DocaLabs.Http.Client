using System.Collections.Generic;

namespace DocaLabs.Http.Client.Mapping.PropertyConverters
{
    public interface IConvertProperty
    {
        IEnumerable<KeyValuePair<string, IList<string>>> GetValue(object obj);
    }
}
