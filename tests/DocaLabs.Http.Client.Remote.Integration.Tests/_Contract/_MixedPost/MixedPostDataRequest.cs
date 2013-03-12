using DocaLabs.Http.Client.Binding.RequestSerialization;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._MixedPost
{
    public class MixedPostDataRequest
    {
        public int Id { get; set; }

        [SerializeAsJson]
        public InnerPostDataRequest Data { get; set; }
    }
}
