namespace DocaLabs.Http.Client.Utils.JsonSerialization
{
    public class JsonSerializerFactory : IJsonSerializerFactory
    {
        public IJsonSerializer CreateSerializer()
        {
            return new JsonSerializer();
        }

        public IJsonDeserializer CreateDeserializer()
        {
            return new JsonDeserializer();
        }
    }
}
