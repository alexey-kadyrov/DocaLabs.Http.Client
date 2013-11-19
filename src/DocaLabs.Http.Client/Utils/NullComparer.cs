namespace DocaLabs.Http.Client.Utils
{
    public class NullComparer : INullComparer
    {
        public bool IsNull(object value)
        {
            return value == null;
        }
    }
}
