using DocaLabs.Http.Client.Binding.Serialization;

namespace DocaLabs.Http.Client.Remote.Integration.Tests._Contract._Put
{
    [SerializeAsJson]
    public interface IPutData
    {
        PutDataResponse Post(PutDataRequest data);
    }
}
