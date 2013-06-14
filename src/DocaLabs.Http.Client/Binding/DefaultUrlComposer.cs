using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Default URL composer.
    /// </summary>
    public class DefaultUrlComposer
    {
        readonly ConcurrentDictionary<Type, PropertyMap> _explicitQueryPropertyMaps = new ConcurrentDictionary<Type, PropertyMap>();
        readonly ConcurrentDictionary<Type, PropertyMap> _explicitPathPropertyMaps = new ConcurrentDictionary<Type, PropertyMap>();
        readonly ConcurrentDictionary<Type, PropertyMap> _implicitPathOrQueryPropertyMaps = new ConcurrentDictionary<Type, PropertyMap>();

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
            catch (HttpClientException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new HttpClientException(string.Format(Resources.Text.failed_create_url, baseUrl), e);
            }
        }

        static bool Ignore(object model)
        {
            return  model == null || model.GetType().GetCustomAttribute<IgnoreInRequestAttribute>(true) != null;
        }

        Uri CreateUrlFrom(object model, Uri baseUrl)
        {
            var path = new NameValueCollection();
            var query = new NameValueCollection();

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

        void ProcessImplicitPathOrQuery(string existingPath, object model, NameValueCollection path, NameValueCollection query)
        {
            var values = ImplicitPathOrQueryPropertyMapGetOrAddType(model).ConvertModel(model);

            foreach (var key in values.AllKeys)
            {
                if (existingPath.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
                    path.Add(key, values.GetValues(key));
                else
                    query.Add(key, values.GetValues(key));
            }
        }

        void ProcessExplicitQuery(object model, NameValueCollection query)
        {
            query.Add(ExplicitQueryPropertyMapGetOrAddType(model).ConvertModel(model));
        }

        void ProcessExplicitPath(object model, NameValueCollection path)
        {
            path.Add(ExplicitPathPropertyMapGetOrAddType(model).ConvertModel(model));
        }

        PropertyMap ImplicitPathOrQueryPropertyMapGetOrAddType(object model)
        {
            return _implicitPathOrQueryPropertyMaps.GetOrAdd(model.GetType(), x => new ImplicitPathOrQueryPropertyMap(x, ImplicitPathOrQueryPropertyMapGetOrAddType));
        }

        PropertyMap ExplicitPathPropertyMapGetOrAddType(object model)
        {
            return _explicitPathPropertyMaps.GetOrAdd(model.GetType(), x => new ExplicitPathPropertyMap(x, ExplicitPathPropertyMapGetOrAddType));
        }

        PropertyMap ExplicitQueryPropertyMapGetOrAddType(object model)
        {
            return _explicitQueryPropertyMaps.GetOrAdd(model.GetType(), x => new ExplicitQueryPropertyMap(x, ExplicitQueryPropertyMapGetOrAddType));
        }

        static string ComposePath(NameValueCollection path, string existingPath)
        {
            if (string.IsNullOrWhiteSpace(existingPath))
                return "";

            foreach (var key in path.AllKeys)
            {
                var values = path.GetValues(key);
                if (values != null)
                {
                    var value = string.Join("/", values.Select(HttpUtility.UrlPathEncode));

                    existingPath = existingPath.Replace(
                        "{" + key + "}", string.IsNullOrWhiteSpace(value) ? "" : value, StringComparison.OrdinalIgnoreCase);
                }
            }

            return existingPath;
        }

        static string ComposeQuery(NameValueCollection query, string existingQuery)
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
            public ExplicitPathPropertyMap(Type type, Func<object, PropertyMap> propertyMapGetOrAddType)
                : base(type, propertyMapGetOrAddType)
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
            public ExplicitQueryPropertyMap(Type type, Func<object, PropertyMap> propertyMapGetOrAddType)
                : base(type, propertyMapGetOrAddType)
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
            public ImplicitPathOrQueryPropertyMap(Type type, Func<object, PropertyMap> propertyMapGetOrAddType)
                : base(type, propertyMapGetOrAddType)
            {
            }

            protected override bool AcceptProperty(PropertyInfo info)
            {
                return info.IsImplicitUrlPathOrQuery();
            }
        }
    }
}