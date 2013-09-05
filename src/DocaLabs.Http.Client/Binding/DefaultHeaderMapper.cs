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
        readonly PropertyMaps _maps = new PropertyMaps();

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

            if (!checkImplicitConditions && PropertyMaps.IsDictionaryModel(model.GetType()))
                return new WebHeaderCollection();

            return new WebHeaderCollection { _maps.Convert(model, x => x.IsHeader(checkImplicitConditions)) };
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
