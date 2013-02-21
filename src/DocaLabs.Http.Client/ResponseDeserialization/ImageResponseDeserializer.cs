using System;
using System.Drawing;
using System.IO;
using DocaLabs.Http.Client.Utils;

namespace DocaLabs.Http.Client.ResponseDeserialization
{
    /// <summary>
    /// Deserializes an image from the response stream.
    /// </summary>
    public class ImageResponseDeserializer : IResponseDeserializationProvider
    {
        /// <summary>
        /// Deserializes an image from the response stream using intermediate MemoryStream as the Image/Bitmap classes 
        /// require that the stream was kept open for the lifetime of the image object.
        /// </summary>
        public object Deserialize(HttpResponse response, Type resultType)
        {
            if(response == null)
                throw new ArgumentNullException("response");

            if(resultType == null)
                throw new ArgumentNullException("resultType");

            // the Image/Bitmap classes require that the stream was kept open for the lifetime of the image object.
            var imageStream = new MemoryStream(response.ContentLength <= 0 ? 8192 : (int)response.ContentLength);

            using (var sourceStream = response.GetDataStream())
            {
                sourceStream.CopyTo(imageStream);
            }

            imageStream.Seek(0, SeekOrigin.Begin);

            if (typeof(Bitmap).IsAssignableFrom(resultType))
                    return new Bitmap(imageStream);

            if (typeof(Image).IsAssignableFrom(resultType))
                return Image.FromStream(imageStream);

            throw new UnrecoverableHttpClientException(string.Format(Resources.Text.expected_retsult_to_be_image_or_bitmap_classes, resultType));
        }

        /// <summary>
        /// Returns true if the content type is 'image/gif'/'image/jpeg'/'image/tiff'/'image/png' and the TResult is not "simple type", like int, string, Guid, double, etc.
        /// </summary>
        public bool CanDeserialize(HttpResponse response, Type resultType)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            if (resultType == null)
                throw new ArgumentNullException("resultType");

            return  (
                        response.ContentType.Is("image/gif") || response.ContentType.Is("image/jpeg") ||
                        response.ContentType.Is("image/tiff") || response.ContentType.Is("image/png")
                    ) &&
                    (
                        resultType == typeof(Image) || resultType == typeof(Bitmap)
                    );
        }
    }
}
