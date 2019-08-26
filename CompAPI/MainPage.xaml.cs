using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CompAPI.Model;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CompAPI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Movie> Movies;
        public MainPage()
        {
            this.InitializeComponent();

            Movies = new ObservableCollection<Movie>();
            xamlMovies.ItemsSource = Movies;

            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPage(1);
            await LoadPage(2);
            await LoadPage(3);
            await LoadPage(4);
        }

        private async Task LoadPage(int page)
        {
            var movies = await GetPage(page);
            foreach (var movie in movies)
            {
                Movies.Add(movie);
            }
        }

        private async Task<List<Movie>> GetPage(int page)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/data/page{page}.json"));
            var str = await FileIO.ReadTextAsync(file);
            var obj = JsonConvert.DeserializeObject<MovieData>(str);

            return obj.content.Select(Movie.From).ToList();
        }

    }
}
