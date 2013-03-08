﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.UrlMapping
{
    class NamedPropertyMap
    {
        public IList<NamedPropertyConverter> Converters { get; private set; }

        public NamedPropertyMap(Type type)
        {
            Converters = Parse(type);
        }

        public string Compose(object model, string existingPath)
        {
            if (string.IsNullOrWhiteSpace(existingPath))
                return "";

            foreach (var converter in Converters)
            {
                var value = converter.GetValue(model);

                existingPath = existingPath.Replace(
                    "{" + converter.Name + "}", string.IsNullOrWhiteSpace(value) ? "" : HttpUtility.UrlPathEncode(value), StringComparison.OrdinalIgnoreCase);
            }

            return existingPath;
        }

        static IList<NamedPropertyConverter> Parse(Type type)
        {
            if (type.IsSimpleType())
                return new List<NamedPropertyConverter>();

            return type.GetAllInstancePublicProperties()
                       .Select(ParseProperty)
                       .Where(x => x != null)
                       .ToList();
        }

        static NamedPropertyConverter ParseProperty(PropertyInfo info)
        {
            if (info.IsUrlNamedPath() && info.PropertyType.IsSimpleType())
                return new NamedPropertyConverter(info);

            return null;
        }
    }
}