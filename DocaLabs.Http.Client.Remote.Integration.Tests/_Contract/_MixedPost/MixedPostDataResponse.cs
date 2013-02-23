using System.Collections.Generic;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._MixedPost
{
    public class MixedPostDataResponse
    {
        public string Url { get; set; }
        public Dictionary<string, string> Args { get; set; }
        public string Origin { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public InnerPostDataRequest Json { get; set; }
    }
}
