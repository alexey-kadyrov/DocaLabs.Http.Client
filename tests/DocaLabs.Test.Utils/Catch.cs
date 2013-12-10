using System;

namespace DocaLabs.Test.Utils
{
    public static class Catch
    {
        public static Exception Exception(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                return e;
            }

            return null;
        }
    }
}
