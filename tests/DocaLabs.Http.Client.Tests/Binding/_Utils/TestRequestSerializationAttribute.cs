using System.Net;
using DocaLabs.Http.Client.Binding.Serialization;

namespace DocaLabs.Http.Client.Tests.Binding._Utils
{
    class TestRequestSerializationAttribute : RequestSerializationAttribute
    {
        public static string UsedMarker { get; set; }
        public string Marker { get; set; }

        public override void Serialize(object obj, WebRequest request)
        {
            UsedMarker = Marker;
        }
    }
}