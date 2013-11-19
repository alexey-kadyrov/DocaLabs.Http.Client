using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using DocsaLabs.Http.Client.Examples.GeoTypes;

namespace DocsaLabs.Http.Client.Examples.Store
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        static readonly IStreetViewService StreetViewService = new StreetViewService(new Uri("http://maps.googleapis.com/maps/api/streetview"));

        public MainPage()
        {
            InitializeComponent();
        }

        private async void GetStreetView_Click(object sender, RoutedEventArgs e)
        {
            Location.IsEnabled = false;
            GetStreetView.IsEnabled = false;

            try
            {
                var imageStream = await StreetViewService.ExecuteAsync(new StreetViewRequest(new GeoLocation(Location.Text)) { heading = 0 });

                StreetViewImage.Source = ToImageSource(imageStream);
            }
            finally
            {
                Location.IsEnabled = true;
                GetStreetView.IsEnabled = true;
            }
        }

        static ImageSource ToImageSource(Stream stream)
        {
            if (stream == null)
                return null;

            var bitmap = new BitmapImage();

            using (stream)
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                bitmap.SetSource(memoryStream.AsRandomAccessStream());
            }

            return bitmap;
        }
    }
}
