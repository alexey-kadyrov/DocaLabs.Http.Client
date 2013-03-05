using System;
using System.Collections.Generic;

namespace DocaLabs.Http.Client.Utils
{
    public class InheritanceComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            return ReferenceEquals(x, y) ? 0 : (x.IsSubclassOf(y) ? 1 : -1);
        }
    }
}