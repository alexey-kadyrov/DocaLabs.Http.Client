using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DocsaLabs.Http.Client.Examples.Core;
using DocsaLabs.Http.Client.Examples.Core.GeoTypes;

namespace DocsaLabs.Http.Client.Examples.Phone
{
    public partial class MainPage
    {
        static readonly IStreetViewService StreetViewService = new StreetViewService();

        // Constructor
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

            bitmap.SetSource(stream);

            return bitmap;
        }
   }
}