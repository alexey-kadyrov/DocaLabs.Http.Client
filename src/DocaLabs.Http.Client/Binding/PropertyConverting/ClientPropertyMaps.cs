using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    /// <summary>
    /// Manages property maps of parsed types separately per each client type.
    /// The actual converting is delegated to PropertyMap.:
    /// </summary>
    public class ClientPropertyMaps
    {
        readonly ConcurrentDictionary<Type, PropertyMaps> _maps = new ConcurrentDictionary<Type, PropertyMaps>();

        /// <summary>
        /// Converts instance into NameValueCollection where keys/values correspond to property names/values.
        /// The maps are build per client type.
        /// </summary>
        /// <param name="client">Http client.</param>
        /// <param name="model">Value to be converted.</param>
        /// <param name="acceptPropertyCheck">Delegate which is used to check whenever the passed property should be parsed.</param>
        public NameValueCollection Convert(object client, object model, Func<PropertyInfo, bool> acceptPropertyCheck)
        {
            if(client == null)
                throw new ArgumentNullException("client");

            if (acceptPropertyCheck == null)
                throw new ArgumentNullException("acceptPropertyCheck");

            return _maps.GetOrAdd(client.GetType(), k => new PropertyMaps()).Convert(model, new HashSet<object>(), acceptPropertyCheck);
        }
    }
}