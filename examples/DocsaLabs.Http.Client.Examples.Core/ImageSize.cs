using DocaLabs.Http.Client.Binding.PropertyConverting;
using DocaLabs.Http.Client.Utils;

namespace DocsaLabs.Http.Client.Examples.Core
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

        public ICustomKeyValueCollection ConvertProperties()
        {
            return new CustomKeyValueCollection { { "", string.Format("{0}x{1}", Width, Height) } };
        }
    }
}
