using System.Collections.Specialized;
using DocaLabs.Http.Client.Binding.PropertyConverting;

namespace DocsaLabs.Http.Client.Google.Examples.StreetView
{
    public class ImageSize : ICustomValueConverter
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public ImageSize()
        {
            Width = 640;
            Height = 480;
        }

        public ImageSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public NameValueCollection ConvertProperties()
        {
            return new NameValueCollection { { "", string.Format("{0}x{1}", Width, Height) } };
        }
    }
}
