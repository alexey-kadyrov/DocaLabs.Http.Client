using System.Diagnostics;

namespace DocaLabs.Http.Client.Utils
{
    public class TraceLog : ITraceLog
    {
        public void TraceError(string format, params object[] args)
        {
            // ReSharper disable EmptyGeneralCatchClause
            try
            {
                Trace.TraceError(string.Format(format, args));
            }
            catch
            {
            }
            // ReSharper restore EmptyGeneralCatchClause
        }
    }
}
