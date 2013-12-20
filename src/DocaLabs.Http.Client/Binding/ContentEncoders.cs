using DocaLabs.Http.Client.Utils;
using DocaLabs.Http.Client.Utils.ContentEncoding;

namespace DocaLabs.Http.Client.Binding
{
    public static class ContentEncoders
    {
        public static IContentDecoderFactory ContentDecoderFactory { get; private set; }
        public static IContentEncoderFactory ContentEncoderFactory { get; private set; }

        static ContentEncoders()
        {
            ContentEncoderFactory = PlatformAdapter.Resolve<IContentEncoderFactory>(false);
            ContentDecoderFactory = PlatformAdapter.Resolve<IContentDecoderFactory>(false);
        }
    }
}
