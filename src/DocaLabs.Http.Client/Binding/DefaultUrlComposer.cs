using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Default URL composer.
    /// </summary>
    public class DefaultUrlComposer : IUrlComposer
    {
        readonly ConcurrentDictionary<Type, ExplicitQueryPropertyMap> _explicitQueryPropertyMaps = new ConcurrentDictionary<Type, ExplicitQueryPropertyMap>();
        readonly ConcurrentDictionary<Type, ExplicitPathPropertyMap> _explicitPathPropertyMaps = new ConcurrentDictionary<Type, ExplicitPathPropertyMap>();
        readonly ConcurrentDictionary<Type, ImplicitPathOrQueryPropertyMap> _implicitPathOrQueryPropertyMaps = new ConcurrentDictionary<Type, ImplicitPathOrQueryPropertyMap>();

        /// <summary>
        /// Composes a new URL using the model's properties and the base URL.
        /// </summary>
        public string Compose(object model, Uri baseUrl)
        {
            if (baseUrl == null)
                throw new ArgumentNullException("baseUrl");

            try
            {
                return Ignore(model)
                    ? baseUrl.AbsoluteUri
                    : CreateUrlFrom(model, baseUrl).AbsoluteUri;
            }
            catch (UnrecoverableHttpClientException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new UnrecoverableHttpClientException(string.Format(Resources.Text.failed_create_url, baseUrl), e);
            }
        }

        static bool Ignore(object model)
        {
            return  model == null || model.GetType().GetCustomAttribute<IgnoreInRequestAttribute>(true) != null;
        }

        Uri CreateUrlFrom(object model, Uri baseUrl)
        {
            var path = new CustomNameValueCollection();
            var query = new CustomNameValueCollection();

            var existingPath = GetExistingPath(baseUrl);
            var exstingQuery = GetExistingQuery(baseUrl);

            ProcessImplicitPathOrQuery(existingPath, model, path, query);
            ProcessExplicitPath(model, path);
            ProcessExplicitQuery(model, query);

            return new UriBuilder(GetBaseUrl(baseUrl))
            {
                Path = ComposePath(path, existingPath),
                Query = ComposeQuery(query, exstingQuery),
                Fragment = GetFragmentWithoutSharpMark(baseUrl)
            }.Uri;
        }

        static string GetBaseUrl(Uri baseUrl)
        {
            return baseUrl.IsFile || baseUrl.IsUnc
                ? baseUrl.AbsoluteUri
                : baseUrl.GetLeftPart(UriPartial.Authority);
        }

        static string GetExistingPath(Uri baseUrl)
        {
            return baseUrl == null
                ? ""
                : baseUrl.GetComponents(UriComponents.Path, baseUrl.UserEscaped ? UriFormat.UriEscaped : UriFormat.Unescaped);
        }

        static string GetExistingQuery(Uri baseUrl)
        {
            return baseUrl == null
                ? ""
                : baseUrl.GetComponents(UriComponents.Query, baseUrl.UserEscaped ? UriFormat.UriEscaped : UriFormat.Unescaped);
        }

        static string GetFragmentWithoutSharpMark(Uri url)
        {
            var fragment = url.Fragment;

            return fragment.StartsWith("#")
                ? fragment.Substring(1)
                : fragment;
        }

        void ProcessImplicitPathOrQuery(string existingPath, object model, IDictionaryList<string, string> path, IDictionaryList<string, string> query)
        {
            var values = _implicitPathOrQueryPropertyMaps.GetOrAdd(model.GetType(), x => new ImplicitPathOrQueryPropertyMap(x)).ConvertModel(model);

            foreach (var pair in values)
            {
                if(existingPath.Contains(pair.Key, StringComparison.OrdinalIgnoreCase))
                    path.Add(pair.Key, pair.Value);
                else
                    query.Add(pair.Key, pair.Value);
            }
        }

        void ProcessExplicitQuery(object model, IDictionaryList<string, string> query)
        {
            query.AddRange(_explicitQueryPropertyMaps.GetOrAdd(model.GetType(), x => new ExplicitQueryPropertyMap(x)).ConvertModel(model));
        }

        void ProcessExplicitPath(object model, IDictionaryList<string, string> path)
        {
            path.AddRange(_explicitPathPropertyMaps.GetOrAdd(model.GetType(), x => new ExplicitPathPropertyMap(x)).ConvertModel(model));
        }

        static string ComposePath(IEnumerable<KeyValuePair<string, IList<string>>> path, string existingPath)
        {
            if (string.IsNullOrWhiteSpace(existingPath))
                return "";

            foreach (var pair in path)
            {
                var value = string.Join("/", pair.Value.Select(HttpUtility.UrlPathEncode));

                existingPath = existingPath.Replace(
                    "{" + pair.Key + "}", string.IsNullOrWhiteSpace(value) ? "" : value, StringComparison.OrdinalIgnoreCase);
            }

            return existingPath;
        }

        static string ComposeQuery(CustomNameValueCollection query, string existingQuery)
        {
            var modelQuery = new QueryStringBuilder().Add(query).ToString();

            if (string.IsNullOrWhiteSpace(existingQuery))
                return modelQuery;

            return string.IsNullOrWhiteSpace(modelQuery)
                ? existingQuery
                : ConcatenateQueryParts(existingQuery, modelQuery);
        }

        static string ConcatenateQueryParts(string leftPart, string rightPart)
        {
            return leftPart.EndsWith("&")
                    ? leftPart + rightPart
                    : leftPart + "&" + rightPart;
        }

        class ExplicitPathPropertyMap : PropertyMap
        {
            public ExplicitPathPropertyMap(Type type)
                : base(type)
            {
            }

            protected override bool AcceptProperty(PropertyInfo info)
            {
                return info.IsExplicitUrlPath();
            }

            protected override IPropertyConverterOverrides GetPropertyConverterOverrides(PropertyInfo property)
            {
                return property.GetCustomAttribute<InRequestPathAttribute>();
            }
        }

        class ExplicitQueryPropertyMap : PropertyMap
        {
            public ExplicitQueryPropertyMap(Type type)
                : base(type)
            {
            }

            protected override bool AcceptProperty(PropertyInfo info)
            {
                return info.IsExplicitUrlQuery();
            }

            protected override IPropertyConverterOverrides GetPropertyConverterOverrides(PropertyInfo property)
            {
                return property.GetCustomAttribute<InRequestQueryAttribute>();
            }
        }

        class ImplicitPathOrQueryPropertyMap : PropertyMap
        {
            public ImplicitPathOrQueryPropertyMap(Type type)
                : base(type)
            {
            }

            protected override bool AcceptProperty(PropertyInfo info)
            {
                return info.IsImplicitUrlPathOrQuery();
            }
        }
    }
}