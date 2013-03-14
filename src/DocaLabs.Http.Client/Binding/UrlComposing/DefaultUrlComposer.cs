using System;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;

namespace DocaLabs.Http.Client.Binding.UrlComposing
{
    public class DefaultUrlComposer : IUrlComposer
    {
        public string Compose(object model, object client, Uri baseUrl)
        {
            if (baseUrl == null)
                throw new ArgumentNullException("baseUrl");

            try
            {
                return Ignore(model, client)
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

        static bool Ignore(object model, object client)
        {
            return  model == null ||
                    model.GetType().GetCustomAttribute<IgnoreInRequestAttribute>(true) != null ||
                    (client != null && client.GetType().GetCustomAttribute<IgnoreInRequestAttribute>(true) != null);
        }

        static Uri CreateUrlFrom(object model, Uri baseUrl)
        {
            var modelType = model.GetType();

            var builder = new UriBuilder(GetBaseUrl(baseUrl))
            {
                //Path = ClientModelBinders.GetUrlPathComposer(modelType).Compose(model, baseUrl),
                //Query = ClientModelBinders.GetUrlQueryComposer(modelType).Compose(model, baseUrl),
                //Fragment = GetFragmentWithoutSharpMark(baseUrl)
            };

            return builder.Uri;
        }

        static string GetBaseUrl(Uri baseUrl)
        {
            return baseUrl.IsFile || baseUrl.IsUnc
                ? baseUrl.ToString()
                : baseUrl.GetLeftPart(UriPartial.Authority);
        }

        static string GetExistingPath(Uri baseUrl)
        {
            return baseUrl == null
                ? ""
                : baseUrl.GetComponents(UriComponents.Path, baseUrl.UserEscaped ? UriFormat.UriEscaped : UriFormat.Unescaped);
        }

        static string GetFragmentWithoutSharpMark(Uri url)
        {
            var fragment = url.Fragment;

            return fragment.StartsWith("#")
                ? fragment.Substring(1)
                : fragment;
        }
    }
}