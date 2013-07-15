using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.PropertyConverting;

namespace DocaLabs.Http.Client.Binding
{
    /// <summary>
    /// Default request header mapper.
    /// </summary>
    public class DefaultHeaderMapper
    {
        readonly PropertyMaps _maps = new PropertyMaps(PropertyInfoExtensions.IsHeader);

        /// <summary>
        /// Maps a model to the collection of key, values. The property must have the RequesUseAttribute with the header target bit set.
        /// </summary>
        public WebHeaderCollection Map(object model)
        {
            return Ignore(model) 
                ? new WebHeaderCollection()
                : new WebHeaderCollection { _maps.Convert(model) };
        }

        static bool Ignore(object model)
        {
            if (model == null)
                return true;

            var useAttribute = model.GetType().GetCustomAttribute<RequestUseAttribute>(true);

            return useAttribute != null && useAttribute.Targets == RequestUseTargets.Ignore;
        }
    }
}
