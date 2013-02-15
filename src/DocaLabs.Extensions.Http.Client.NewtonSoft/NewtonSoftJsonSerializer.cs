using System.ComponentModel.Composition;
using DocaLabs.Http.Client.JsonSerialization;
using Newtonsoft.Json;

namespace DocaLabs.Extensions.Http.Client.NewtonSoft
{
    /// <summary>
    /// Implements IJsonSerializer using Newtonsoft JsonConvert.
    /// </summary>
    [Export(typeof(IJsonSerializer))]
    public class NewtonSoftDefaultJsonSerializer : IJsonSerializer
    {
        /// <summary>
        /// Serializes an object into string using JSON notation.
        /// </summary>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}