using DocaLabs.Http.Client.RequestSerialization;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Post
{
    [SerializeAsJson]
    public interface IPostData
    {
        PostDataResponse Post(PostDataRequest data);
    }
}
