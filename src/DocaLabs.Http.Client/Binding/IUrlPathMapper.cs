namespace DocaLabs.Http.Client.Binding
{
    public interface IUrlPathMapper
    {
        string[] Map(object model, object client);
    }
}
