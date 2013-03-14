using System;
using System.Reflection;

namespace DocaLabs.Http.Client.Binding.UrlComposing
{
    class ExplicitQueryPropertyMap : QueryPropertyMapBase
    {
        public ExplicitQueryPropertyMap(Type type) 
            : base(type)
        {
        }

        protected override bool IsSuitableForUrlQuery(PropertyInfo info)
        {
            return info.IsExplicitUrlQuery();
        }
    }
}