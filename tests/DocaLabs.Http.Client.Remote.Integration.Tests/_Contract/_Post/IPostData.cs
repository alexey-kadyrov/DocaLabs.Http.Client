using DocaLabs.Http.Client.Binding.Serialization;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Post
{
    [SerializeAsJson]
    public interface IPostData
    {
        PostDataResponse Post(PostDataRequest data);
    }
}
