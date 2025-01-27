using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using Quartz;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JobScheduler.Processing
{
    // FlaskApiService class
    public class FlaskApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FlaskApiService> _logger;
        private readonly AppDbContext _dbContext;

        public FlaskApiService(HttpClient httpClient, ILogger<FlaskApiService> logger, AppDbContext dbContext)
        {
            _httpClient = httpClient;
            _logger = logger;
            _dbContext = dbContext;
        }

        // Send Aggregated Sales Data to Flask
        public async Task SendAggregatedSalesData()
        {
            var flaskApiUrl = "http://localhost:5000/plot/aggregated-sales";
            _logger.LogInformation("Preparing to send Aggregated Sales data to Flask API...");

            var aggregatedSalesData = await _dbContext.AggregatedSales
                .Select(a => new
                {
                    a.Region,
                    a.TotalSales
                }).ToListAsync();

            var jsonPayload = JsonSerializer.Serialize(aggregatedSalesData);
            await PostToFlaskApi(flaskApiUrl, jsonPayload);
        }

        // Send Monthly Trends Data to Flask
        public async Task SendMonthlyTrendsData()
        {
            var flaskApiUrl = "http://localhost:5000/plot/monthly-trends";
            _logger.LogInformation("Preparing to send Monthly Trends data to Flask API...");

            var monthlyTrendsData = await _dbContext.MonthlyTrends
                .Select(m => new
                {
                    Month = m.Month.ToString("yyyy-MM"),
                    m.Category,
                    m.SalesGrowth
                }).ToListAsync();

            var jsonPayload = JsonSerializer.Serialize(monthlyTrendsData);
            await PostToFlaskApi(flaskApiUrl, jsonPayload);
        }

        // Helper method to post data to Flask API
        private async Task PostToFlaskApi(string flaskApiUrl, string jsonPayload)
        {
            try
            {
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                _logger.LogInformation($"Sending data to {flaskApiUrl}...");
                var response = await _httpClient.PostAsync(flaskApiUrl, content);

                response.EnsureSuccessStatusCode();
                _logger.LogInformation("Data sent successfully to Flask API. Response: {StatusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send data to Flask API at {flaskApiUrl}");
                throw;
            }
        }
    }

    // FlaskApiServiceJob class
    public class FlaskApiServiceJob : IJob
    {
        private readonly FlaskApiService _flaskApiService;
        private readonly ILogger<FlaskApiServiceJob> _logger;

        public FlaskApiServiceJob(FlaskApiService flaskApiService, ILogger<FlaskApiServiceJob> logger)
        {
            _flaskApiService = flaskApiService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("FlaskApiServiceJob is executing.");

            // Send Aggregated Sales data
            _logger.LogInformation("Sending Aggregated Sales data...");
            await _flaskApiService.SendAggregatedSalesData();

            // Send Monthly Trends data
            _logger.LogInformation("Sending Monthly Trends data...");
            await _flaskApiService.SendMonthlyTrendsData();

            _logger.LogInformation("FlaskApiServiceJob execution completed.");
        }
    }
}
