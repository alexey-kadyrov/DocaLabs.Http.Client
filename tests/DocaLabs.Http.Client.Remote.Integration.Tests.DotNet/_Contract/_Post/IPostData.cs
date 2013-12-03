using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding.Serialization;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Post
{
    [SerializeAsJson]
    public interface IPostData
    {
        PostDataResponse Post(PostDataRequest data);
    }

    [SerializeAsJson]
    public interface IPostDataAsync
    {
        Task<PostDataResponse> Post(PostDataRequest data);
    }
}
