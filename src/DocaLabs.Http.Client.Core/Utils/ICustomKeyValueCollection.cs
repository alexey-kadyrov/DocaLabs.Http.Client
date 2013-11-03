namespace DocaLabs.Http.Client.Utils
{
    public interface ICustomKeyValueCollection
    {
        string[] AllKeys { get; }
        string[] GetValues(string key);
        void Add(string key, string value);
    }
}
