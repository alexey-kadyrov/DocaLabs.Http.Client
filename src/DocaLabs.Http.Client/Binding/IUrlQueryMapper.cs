using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public interface IUrlQueryMapper
    {
        CustomNameValueCollection Map(object model, object client);
    }
}
