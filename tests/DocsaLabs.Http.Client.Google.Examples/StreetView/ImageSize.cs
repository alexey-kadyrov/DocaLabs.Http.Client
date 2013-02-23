namespace DocsaLabs.Http.Client.Google.Examples.StreetView
{
    public class ImageSize
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

        public override string ToString()
        {
            return string.Format("{0}x{1}", Width, Height);
        }
    }
}
