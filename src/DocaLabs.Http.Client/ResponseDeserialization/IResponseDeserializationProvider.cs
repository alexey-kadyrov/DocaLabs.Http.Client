using System;

namespace DocaLabs.Http.Client.ResponseDeserialization
{
    /// <summary>
    /// Defines methods for deserializing the response.
    /// </summary>
    public interface IResponseDeserializationProvider : IResponseDeserialization
    {
        /// <summary>
        /// Checks whenever the response can be deserialized for TReult type by the instance of that class.
        /// </summary>
        bool CanDeserialize(HttpResponse response, Type resultType);
    }
}
