using System;

namespace DocaLabs.Http.Client.Utils
{
    public static class UriExtensions
    {
        public static string GetAuthorityPart(this Uri uri)
        {
            return uri.GetComponents(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.UriEscaped);
        }
    }
}
