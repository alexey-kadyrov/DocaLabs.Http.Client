using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils
{
    public interface ICustomKeyValueCollection : IEnumerable<string>
    {
        int Count { get; }
        IReadOnlyCollection<string> AllKeys { get; }
        IReadOnlyList<string> GetValues(string key);
        void Add(string key, string value);
        void Add(ICustomKeyValueCollection collection);
    }
}
