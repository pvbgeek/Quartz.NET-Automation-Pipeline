using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace JobScheduler.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadDatasetPreview();
        }

        private void LoadDatasetPreview()
        {
            try
            {
                // Simulate loading dataset dimensions and preview
                int totalRows = 1000000; // Replace with your actual data
                int totalColumns = 7;    // Replace with your actual data

                DatasetDimensions.Text = $"Dimensions: {totalRows} rows, {totalColumns} columns";

                // Generate mock data for preview (replace with real data logic)
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("TransactionID");
                dataTable.Columns.Add("CustomerID");
                dataTable.Columns.Add("ProductID");
                dataTable.Columns.Add("Date");
                dataTable.Columns.Add("Amount");
                dataTable.Columns.Add("Region");
                dataTable.Columns.Add("Category");

                for (int i = 0; i < 20; i++)
                {
                    dataTable.Rows.Add($"TID{i}", $"CID{i}", $"PID{i}", DateTime.Now, i * 100, "RegionX", "CategoryY");
                }

                DataPreviewGrid.ItemsSource = dataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dataset: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void QuartzAutomationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Display loading screen
                MessageBox.Show("Quartz Timely Automation started. Please wait...", "Processing", MessageBoxButton.OK, MessageBoxImage.Information);

                // Execute the Quartz automation logic
                await Task.Run(() => RunQuartzAutomation());

                // Show the next screen with plots
                DataWindow dataWindow = new DataWindow();
                dataWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running Quartz Automation: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RunQuartzAutomation()
        {
                try
                {
                    // Set up the process to open a new terminal
                    Process process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = "/K dotnet run",
                            WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "JobScheduler", "JobScheduler.Processing"),
                            UseShellExecute = true,
                            CreateNoWindow = false // Ensure it opens a visible terminal
                        }
                    };

                    // Start the process
                    process.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error running Quartz Automation: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

    }
}
