﻿using System;
using System.Collections.Concurrent;
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

        public PropertyMap GetOrAdd(object o)
        {
            if(o == null)
                throw new ArgumentNullException("o");

            var type = o.GetType();

            PropertyMap map;

            if (_maps.TryGetValue(type, out map))
                return map;

            map = new PropertyMap(this);

            _maps.TryAdd(type, map);

            map.Parse(o);

            return map;
        }
    }
}