using System;
using NUnit.Framework;

namespace DocaLabs.Test.Utils
{
    public static class Catch
    {
        public static Exception Exception(Action action)
        {
            return Assert.Catch(() => action());
        }
    }
}
