namespace DocaLabs.Http.Client.Binding
{
    public interface IUrlPathMapper
    {
        object[] Map(object model, object client);
    }
}
