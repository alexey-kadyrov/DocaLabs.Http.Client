using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._MixedPost
{
    public interface IMixedPostData
    {
        MixedPostDataResponse Post(MixedPostDataRequest data);
    }

    public interface IMixedPostDataAsync
    {
        Task<MixedPostDataResponse> Post(MixedPostDataRequest data);
    }
}
