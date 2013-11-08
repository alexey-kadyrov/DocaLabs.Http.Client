using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DocaLabs.Http.Client.Utils
{
    public class ReadOnlyList<TInterface, TConcrete> : IReadOnlyList<TInterface>
        where TConcrete : TInterface
    {
        readonly IList<TConcrete> _data;

        public TInterface this[int index]
        {
            get { return _data[index]; }
        }

        public int Count { get { return _data.Count; } }

        public ReadOnlyList(IList<TConcrete> data)
        {
            _data = data;
        }

        public IEnumerator<TInterface> GetEnumerator()
        {
            return _data.Cast<TInterface>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}