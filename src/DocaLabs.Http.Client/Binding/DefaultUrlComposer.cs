using System;
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
        readonly PropertyMaps _implicitPathOrQueryMaps = new PropertyMaps(RequestUsageExtensions.IsImplicitUrlPathOrQuery);
        readonly PropertyMaps _explicitQueryMaps = new PropertyMaps(RequestUsageExtensions.IsExplicitUrlQuery);
        readonly PropertyMaps _explicitPathMaps = new PropertyMaps(RequestUsageExtensions.IsExplicitUrlPath);

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
                    ? baseUrl.OriginalString
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
            if (model == null)
                return true;

            var useAttribute = model.GetType().GetCustomAttribute<RequestUseAttribute>(true);

            return useAttribute != null && useAttribute.Targets == RequestUseTargets.Ignore;
        }

        Uri CreateUrlFrom(object model, Uri baseUrl)
        {
            var path = new NameValueCollection();
            var query = new NameValueCollection();

            var existingPath = GetExistingPath(baseUrl);
            var exstingQuery = GetExistingQuery(baseUrl);

            var instancConverter = PropertyMaps.TryGetModelValueConverter(model);
            if (instancConverter != null)
            {
                ProcessImplicitPathOrQuery(existingPath, instancConverter.Convert(model), path, query);
            }
            else
            {
                ProcessImplicitPathOrQuery(existingPath, _implicitPathOrQueryMaps.Convert(model), path, query);
                ProcessExplicitPath(model, path);
                ProcessExplicitQuery(model, query);
            }

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

        static string GetFragmentWithoutSharpMark(Uri baseUrl)
        {
            var fragment = baseUrl.Fragment;

            return fragment.StartsWith("#")
                ? fragment.Substring(1)
                : fragment;
        }

        static void ProcessImplicitPathOrQuery(string existingPath, NameValueCollection implicitValues, NameValueCollection path, NameValueCollection query)
        {
            foreach (var key in implicitValues.AllKeys)
            {
                if (existingPath.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
                    path.Add(key, implicitValues.GetValues(key));
                else
                    query.Add(key, implicitValues.GetValues(key));
            }
        }

        void ProcessExplicitQuery(object model, NameValueCollection query)
        {
            query.Add(_explicitQueryMaps.Convert(model));
        }

        void ProcessExplicitPath(object model, NameValueCollection path)
        {
            path.Add(_explicitPathMaps.Convert(model));
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
                    var value = string.Join("/", values.Where(x => !string.IsNullOrWhiteSpace(x)).Select(HttpUtility.UrlPathEncode));

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
    }
}