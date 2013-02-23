using System.Collections.Generic;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Post
{
    public class PostDataResponse
    {
        public string Url { get; set; }
        public Dictionary<string, string> Args { get; set; }
        public string Origin { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public PostDataRequest Json { get; set; }
    }
}
