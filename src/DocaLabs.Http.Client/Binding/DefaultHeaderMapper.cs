using System;
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
        static readonly ClientPropertyMaps Maps = new ClientPropertyMaps();

        /// <summary>
        /// Maps a model to the collection of key, values. The property must have the RequesUseAttribute with the header target bit set.
        /// </summary>
        public WebHeaderCollection Map(object client, object model)
        {
            if(client == null)
                throw new ArgumentNullException("client");

            if (Ignore(model))
                return new WebHeaderCollection();

            var checkImplicitConditions = !model.GetType().IsSerializableToRequestBody() &&
                                          !client.GetType().IsSerializableToRequestBody();

            return new WebHeaderCollection { Maps.Convert(client, model, x => x.IsHeader(checkImplicitConditions)) };
        }

        static bool Ignore(object model)
        {
            if (model == null || PropertyMaps.IsDictionaryModel(model.GetType()))
                return true;

            var useAttribute = model.GetType().GetCustomAttribute<RequestUseAttribute>(true);

            return useAttribute != null && useAttribute.Targets == RequestUseTargets.Ignore;
        }
    }
}
