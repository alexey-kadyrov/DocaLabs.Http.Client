using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DocaLabs.Http.Client;
using DocsaLabs.Http.Client.Examples.Core;
using DocsaLabs.Http.Client.Examples.Core.GeoTypes;

namespace DocsaLabs.Http.Client.Examples.DotNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        static readonly IStreetViewService StreetViewService = HttpClientFactory.CreateInstance<IStreetViewService>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetStreetView_Click(object sender, RoutedEventArgs e)
        {
            Location.IsEnabled = false;
            GetStreetView.IsEnabled = false;

            try
            {
                var imageStream = await StreetViewService.FetchAsync(new StreetViewRequest(new GeoLocation(Location.Text)) { heading = 0 });
                
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

            bitmap.BeginInit();

            bitmap.StreamSource = stream;

            bitmap.EndInit();

            return bitmap;
        }
    }
}
