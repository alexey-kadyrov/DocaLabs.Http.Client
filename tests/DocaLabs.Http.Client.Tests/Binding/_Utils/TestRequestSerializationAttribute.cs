using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Tests.Binding._Utils
{
    class TestRequestSerializationAttribute : RequestSerializationAttribute
    {
        public static string UsedMarker { get; set; }
        public static string UsedAsyncMarker { get; set; }
        public static CancellationToken UsedCancellationToken { get; set; }
        public string Marker { get; set; }

        public override void Serialize(object obj, WebRequest request)
        {
            UsedMarker = Marker;
        }

        public override Task SerializeAsync(object obj, WebRequest request, CancellationToken cancellationToken)
        {
            UsedAsyncMarker = Marker;
            UsedCancellationToken = cancellationToken;
            return TaskUtils.CompletedTask();
        }
    }
}