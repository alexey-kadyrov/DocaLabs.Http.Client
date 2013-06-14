using System.ComponentModel.Composition;
using DocaLabs.Http.Client.Utils.JsonSerialization;

namespace DocaLabs.Http.Client.Extension.Test.Example
{
    [Export(typeof(IJsonSerializer))]
    public class TestIJsonSerializer : IJsonSerializer
    {
        public string Serialize(object obj)
        {
            return "--success--";
        }
    }
}
