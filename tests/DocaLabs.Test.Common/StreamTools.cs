using System.IO;
using System.Text;

namespace DocaLabs.Testing.Common
{
    public static class StreamTools
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static string ReadAsString(this Stream stream, Encoding encoding)
        {
            return encoding.GetString(stream.ToByteArray());
        }
    }
}
