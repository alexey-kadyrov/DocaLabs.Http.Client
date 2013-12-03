using System.Threading.Tasks;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Get
{
    public interface IGetData
    {
        GetDataResponse Get(GetDataRequest data);
    }

    public interface IGetDataAsync
    {
        Task<GetDataResponse> Get(GetDataRequest data);
    }
}
