using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.PropertyConverting;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultHeaderMapper
    {
        readonly PropertyMaps _maps = new PropertyMaps(PropertyInfoExtensions.IsHeader);

        public WebHeaderCollection Map(object model)
        {
            return Ignore(model) 
                ? new WebHeaderCollection()
                : GetHeaders(_maps.Convert(model));
        }

        static bool Ignore(object model)
        {
            if (model == null)
                return false;

            var useAttribute = model.GetType().GetCustomAttribute<RequestUseAttribute>(true);

            return useAttribute != null && useAttribute.Targets == RequestUseTargets.Ignore;
        }

        static WebHeaderCollection GetHeaders(NameValueCollection headers)
        {
            return new WebHeaderCollection
            {
                headers
            };
        }
    }
}
