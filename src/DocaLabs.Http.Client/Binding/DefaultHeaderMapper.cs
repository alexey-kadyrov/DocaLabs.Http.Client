﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using DocaLabs.Http.Client.Binding.Attributes;
using DocaLabs.Http.Client.Binding.PropertyConverters;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding
{
    public class DefaultHeaderMapper : IHeaderMapper
    {
        readonly ConcurrentDictionary<Type, PropertyMap> _parsedMaps = new ConcurrentDictionary<Type, PropertyMap>();

        public WebHeaderCollection Map(object model)
        {
            return model == null ? 
                new WebHeaderCollection() 
                : GetHeaders(model, _parsedMaps.GetOrAdd(model.GetType(), x => new PropertyMap(x)));
        }

        static WebHeaderCollection GetHeaders(object model, PropertyMap map)
        {
            var collection = new WebHeaderCollection();

            foreach (var property in map.Properties)
                ProcessProperty(model, property, collection);

            return collection;
        }

        static void ProcessProperty(object model, object property, NameValueCollection collection)
        {
            if(TryAsHeaderCollection(model, property, collection))
                return;

            TryConverter(model, property, collection);
        }

        static bool TryAsHeaderCollection(object model, object property, NameValueCollection collection)
        {
            var headerProperty = property as PropertyInfo;
            if (headerProperty == null)
                return false;

            var headers = headerProperty.GetValue(model) as WebHeaderCollection;
            if (headers == null)
                return false;

            collection.Add(headers);

            return true;
        }

        static void TryConverter(object model, object property, NameValueCollection collection)
        {
            var converter = property as IPropertyConverter;
            if (converter == null)
                return;

            var values = converter.GetValue(model);
            if (values == null)
                return;

            foreach (var key in values.Keys)
            {
                foreach (var value in values[key])
                {
                    collection.Add(key, value);
                }
            }
        }

        class PropertyMap
        {
            public IList<object> Properties { get; private set; }

            public PropertyMap(Type type)
            {
                Properties = Parse(type);
            }

            static IList<object> Parse(Type type)
            {
                if(type.IsSimpleType())
                    return new List<object>();

                var collection = new List<object>();

                foreach (var property in type.GetAllInstancePublicProperties())
                    ParseProperty(property, collection);

                return collection;
            }

            static void ParseProperty(PropertyInfo property, List<object> collection)
            {
                if (!property.IsHeader())
                    return;

                if (typeof (WebHeaderCollection).IsAssignableFrom(property.PropertyType))
                {
                    collection.Add(property);
                    return;
                }

                var converter = SimplePropertyConverter<RequestHeaderAttribute>.TryCreate(property);
                if (converter == null)
                    throw new UnrecoverableHttpClientException(string.Format(Resources.Text.property_must_be_simple, property.Name, property.DeclaringType));

                collection.Add(converter);
            }
        }
    }
}