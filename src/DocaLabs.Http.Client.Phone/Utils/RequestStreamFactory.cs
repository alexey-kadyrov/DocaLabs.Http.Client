﻿using System.IO;
using System.Net;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding;

namespace DocaLabs.Http.Client.Utils
{
    public class RequestStreamFactory : IRequestStreamFactory
    {
        public Stream Get(BindingContext context, WebRequest request)
        {
            var stream = Task<Stream>.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, null).Result;

            if (stream != null && stream.CanTimeout)
                stream.WriteTimeout = context.Configuration.WriteTimeout;

            return stream;
        }

        public async Task<Stream> GetAsync(AsyncBindingContext context, WebRequest request)
        {
            var stream = await Task<Stream>.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, null);

            if (stream != null && stream.CanTimeout)
                stream.WriteTimeout = context.Configuration.WriteTimeout;

            return stream;
        }
    }
}
