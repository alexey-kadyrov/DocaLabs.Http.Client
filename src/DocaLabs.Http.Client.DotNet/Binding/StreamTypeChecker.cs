using System;
using System.IO;

namespace DocaLabs.Http.Client.Binding
{
    public class StreamTypeChecker : IStreamTypeChecker
    {
        public bool IsStream(Type type)
        {
            return type == typeof(Stream) || type == typeof(HttpResponseStream) || type == typeof(HttpResponseStreamCore);
        }
    }
}
