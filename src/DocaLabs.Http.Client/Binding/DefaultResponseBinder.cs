using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Binding.Serialization;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Defines helper methods to deserialize a web response. All public methods are thread safe.
    /// </summary>
    public class DefaultResponseBinder : IResponseBinder, IAsyncResponseBinder
    {
        readonly ConcurrentDictionary<Type, Type> _responseTypes;
        IList<IResponseDeserializationProvider> _providers;

        /// <summary>
        /// Gets or sets the list of deserialization providers.
        /// The setter and getter are thread safe.
        /// </summary>
        public IList<IResponseDeserializationProvider> Providers
        {
            // reference assignment is thread safe

            get
            {
                var providers = _providers;

                return providers.ToList();
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                var providers = value.ToList();

                _providers = providers;
            }
        }

        /// <summary>
        /// Creates a new instance of the DefaultResponseBinder class with Json, Xml, and plain text deserializers.
        /// </summary>
        public DefaultResponseBinder()
        {
            _responseTypes = new ConcurrentDictionary<Type, Type>();
            _providers = new List<IResponseDeserializationProvider>
            {
                new DeserializeFromJsonAttribute(),
                new DeserializeFromXmlAttribute(),
                new DeserializeFromPlainTextAttribute()
            };
        }

        /// <summary>
        /// Reads the response stream and returns an object if there is anything there.
        /// For deserialization it:
        ///     * Checks for ResponseDeserializationAttribute on the output model type
        ///     * Checks for ResponseDeserializationAttribute on the http client type
        ///     * Checks whenever it can use the registered deserializers
        ///     * If the output model is string return it as the string
        ///     * If the output model is byte array return it as byte array
        ///     * If the output model is Stream or HttpResponseStream returns directly the stream - it'll be the caller responsibility to dispose it.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="request">The WebRequest object.</param>
        /// <param name="resultType">Expected type for the return value.</param>
        /// <returns>Return value from the stream or null.</returns>
        public virtual object Read(BindingContext context, WebRequest request, Type resultType)
        {
            var responseType = GetResponseType(context, resultType);

            HttpResponseStream stream = null;

            try
            {
                stream = HttpResponseStream.CreateResponseStream(request);

                var value = ReadStream(context, stream, responseType);

                object richResponse = null;

                if (responseType != resultType)
                    richResponse = Activator.CreateInstance(resultType, stream.Response, value);

                if (value.Equals(stream))
                    stream = null;

                return richResponse ?? value;
            }
            catch (WebException e)
            {
                if (Is3XX(e) && responseType != resultType)
                    return Activator.CreateInstance(resultType, e.Response, null);

                throw;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        /// <summary>
        /// Asynchronously reads the response stream and returns an object if there is anything there.
        /// </summary>
        /// <param name="context">The binding context.</param>
        /// <param name="request">The WebRequest object.</param>
        /// <returns>Return value from the stream or default value of T.</returns>
        public async Task<T> ReadAsync<T>(AsyncBindingContext context, WebRequest request)
        {
            var resultType = typeof(T);

            var responseType = GetResponseType(context, resultType);

            HttpResponseStream stream = null;

            try
            {
                stream = await HttpResponseStream.CreateAsyncResponseStream(request, context.CancellationToken);

                var deserializer = GetUserSpecifiedDeserializer(context, responseType);
                if (deserializer == null)
                {
                    if (responseType == typeof(Stream) || responseType == typeof(HttpResponseStream))
                    {
                        var retVal = stream;
                        stream = null;
                        return (T)MakeReturnValue(retVal, retVal, responseType, resultType);
                    }

                    if (responseType == typeof(VoidType))
                        return (T)MakeReturnValue(stream, VoidType.Value, responseType, resultType);

                    deserializer = FindProvider(stream, responseType);

                    if (deserializer == null)
                    {
                        if (responseType == typeof(string))
                            return await (Task<T>)(object)stream.AsStringAsync();

                        if (responseType == typeof(byte[]))
                            return await (Task<T>)(object)stream.AsByteArrayAsync(context.CancellationToken);

                        throw new HttpClientException(Resources.Text.cannot_figure_out_how_to_deserialize);
                    }
                }

                var asyncDeserializer = deserializer as IAsyncResponseDeserialization;

                var value = await (asyncDeserializer != null
                    ? asyncDeserializer.DeserializeAsync(stream, responseType, context.CancellationToken)
                    : Task.FromResult(deserializer.Deserialize(stream, responseType)));

                return (T)MakeReturnValue(stream, value, responseType, resultType);
            }
            catch (WebException e)
            {
                if (Is3XX(e) && responseType != resultType)
                    return (T)Activator.CreateInstance(resultType, e.Response, null);

                throw;
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        static object MakeReturnValue(HttpResponseStream stream, object value, Type responseType, Type resultType)
        {
            object richResponse = null;

            if (responseType != resultType)
                richResponse = Activator.CreateInstance(resultType, stream.Response, value);

            return richResponse ?? value;
        }

        Type GetResponseType(BindingContext context, Type resultType)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            return _responseTypes.GetOrAdd(resultType, t => t.TryGetWrappedResponseModelType() ?? t);
        }

        static bool Is3XX(WebException e)
        {
            var httpResponse = e.Response as HttpWebResponse;
            return (httpResponse != null && ((int)httpResponse.StatusCode) >= 300 && ((int)httpResponse.StatusCode) < 400);
        }

        object ReadStream(BindingContext context, HttpResponseStream responseStream, Type resultType)
        {
            var deserializer = resultType.GetCustomAttribute<ResponseDeserializationAttribute>(true);
            if (deserializer != null)
                return deserializer.Deserialize(responseStream, resultType);

            if (context.HttpClient != null)
            {
                deserializer = context.HttpClient.GetType().GetCustomAttribute<ResponseDeserializationAttribute>(true);
                if (deserializer != null)
                    return deserializer.Deserialize(responseStream, resultType);
            }

            if (resultType == typeof(Stream) || resultType == typeof(HttpResponseStream))
                return responseStream;

            if (resultType == typeof(VoidType))
                return VoidType.Value;

            var provider = FindProvider(responseStream, resultType);
            if (provider != null)
                return provider.Deserialize(responseStream, resultType);

            if (resultType == typeof(string))
                return responseStream.AsString();

            if (resultType == typeof(byte[]))
                return responseStream.AsByteArray();

            throw new HttpClientException(Resources.Text.cannot_figure_out_how_to_deserialize);
        }

        static IResponseDeserialization GetUserSpecifiedDeserializer(BindingContext context, MemberInfo resultType)
        {
            var deserializer = resultType.GetCustomAttribute<ResponseDeserializationAttribute>(true);
            if (deserializer != null)
                return deserializer;

            return context.HttpClient != null 
                ? context.HttpClient.GetType().GetCustomAttribute<ResponseDeserializationAttribute>(true) 
                : null;
        }

        IResponseDeserialization FindProvider(HttpResponseStream responseStream, Type resultType)
        {
            // reference assignment is thread safe

            var providers = _providers;

            return providers.FirstOrDefault(x => x.CanDeserialize(responseStream, resultType));
        }
    }
}
