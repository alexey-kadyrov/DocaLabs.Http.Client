namespace DocaLabs.Http.Client.Utils
{
    public interface ITraceLog
    {
        void TraceError(string format, params object[] args);
    }
}
