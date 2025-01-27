using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace JobScheduler.UI
{
    public partial class DataWindow : Window
    {
        public DataWindow()
        {
            InitializeComponent();
            LoadPlots();
        }

        private void LoadPlots()
        {
            try
            {
               string plotDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "JobScheduler", "JobScheduler", "plots");


                // Load images
                Plot1Image.Source = LoadImage(Path.Combine(plotDirectory, "1.png"));
                Plot2Image.Source = LoadImage(Path.Combine(plotDirectory, "2.png"));
                Plot3Image.Source = LoadImage(Path.Combine(plotDirectory, "3.png"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading plots: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private BitmapImage LoadImage(string path)
        {
            if (File.Exists(path))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
            throw new FileNotFoundException($"File not found: {path}");
        }
    }
}
