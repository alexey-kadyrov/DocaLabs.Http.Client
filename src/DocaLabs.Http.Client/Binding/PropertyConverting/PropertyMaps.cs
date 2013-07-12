using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace DocaLabs.Http.Client.Binding.PropertyConverting
{
    public class PropertyMaps
    {
        readonly ConcurrentDictionary<Type, PropertyMap> _maps = new ConcurrentDictionary<Type, PropertyMap>();

        public Func<PropertyInfo, bool> AcceptPropertyCheck { get; private set; }

        public PropertyMaps(Func<PropertyInfo, bool> acceptPropertyCheck)
        {
            AcceptPropertyCheck = acceptPropertyCheck;
        }

        public NameValueCollection Convert(object instance)
        {
            return Convert(instance, new HashSet<object>());
        }

        internal NameValueCollection Convert(object instance, ISet<object> processed)
        {
            if (instance == null)
                return new NameValueCollection();

            var type = instance.GetType();

            PropertyMap map;

            if (_maps.TryGetValue(type, out map))
                return map.Convert(instance, processed);

            map = new PropertyMap(this);

            _maps.TryAdd(type, map);

            map.Parse(instance);

            return map.Convert(instance, processed);
        }
    }
}