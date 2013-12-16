namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    public interface IJsonSerializerFactory
    {
        IJsonSerializer CreateSerializer();
        IJsonDeserializer CreateDeserializer();
    }
}
