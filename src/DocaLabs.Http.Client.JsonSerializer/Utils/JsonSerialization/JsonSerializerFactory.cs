namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    public class JsonSerializerFactory : IJsonSerializerFactory
    {
        readonly IJsonSerializer _serializer = new JsonSerializer();
        readonly IJsonDeserializer _deserializer = new JsonDeserializer();

        public IJsonSerializer CreateSerializer()
        {
            return _serializer;
        }

        public IJsonDeserializer CreateDeserializer()
        {
            return _deserializer;
        }
    }
}
