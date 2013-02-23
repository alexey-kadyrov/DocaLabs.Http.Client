using System.Collections.Generic;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Get
{
    public class GetDataResponse
    {
        public string Url { get; set; }
        public Dictionary<string, string> Args { get; set; }
        public string Origin { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}
