using System;

namespace DocaLabs.Http.Client.Tests._Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute : Attribute
    {
        public string FromConstructorArg { get; private set; }
        public string Property { get; set; }
        public string Field;

        public ClassAttributeWithFieldsPropertiesAndConstructorArgsAttribute(string value)
        {
            FromConstructorArg = value;
        }
    }
}
