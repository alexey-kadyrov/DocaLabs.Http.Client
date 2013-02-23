using System;
using System.Collections.Generic;

namespace DocsaLabs.Http.Client.Google.Examples.Utils
{
    [Serializable]
    public class SeparatedStringCollection : List<string>
    {
        public string Separator { get; set; }

        public SeparatedStringCollection()
        {
            Separator = "|";
        }

        public SeparatedStringCollection(IEnumerable<string> collection)
            : base(collection)
        {
            Separator = "|";
        }

        public SeparatedStringCollection(int capacity)
            : base(capacity)
        {
            Separator = "|";
        }

        public override string ToString()
        {
            return string.Join(Separator, this);
        }
    }
}
