using System;
using System.Collections.Concurrent;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.Binding.UrlComposing
{
    static class ExplicitUrlQueryComposer
    {
        readonly static ConcurrentDictionary<Type, ExplicitQueryPropertyMap> ConverterMaps = new ConcurrentDictionary<Type, ExplicitQueryPropertyMap>();

        public static CustomNameValueCollection Compose(object model, Uri baseUrl)
        {
            return ConvertModel(model, ConverterMaps.GetOrAdd(model.GetType(), x => new ExplicitQueryPropertyMap(x)));
        }

        static CustomNameValueCollection ConvertModel(object obj, ExplicitQueryPropertyMap map)
        {
            var values = new CustomNameValueCollection();

            foreach (var converter in map.Converters)
                values.AddRange(converter.GetValue(obj));

            return values;
        }
    }
}
