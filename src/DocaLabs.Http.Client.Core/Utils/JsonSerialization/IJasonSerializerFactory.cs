namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    public interface IJasonSerializerFactory
    {
        IJsonSerializer CreateSerializer();
        IJsonDeserializer CreateDeserializer();
    }
}
