using Quartz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobScheduler.Processing
{
    public class AggregatedSalesJob : IJob
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<AggregatedSalesJob> _logger;

        public AggregatedSalesJob(AppDbContext dbContext, ILogger<AggregatedSalesJob> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("Starting AggregatedSalesJob...");

                await _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"AggregatedSales\"");
                _logger.LogInformation("Processing sales data...");

                var aggregatedData = await _dbContext.StagingData
                    .GroupBy(s => new { s.Region, s.Category })
                    .Select(g => new AggregatedSales
                    {
                        Region = g.Key.Region ?? "Unknown",
                        Category = g.Key.Category ?? "Unknown",
                        TotalSales = g.Sum(x => x.Amount),
                        AverageSales = g.Average(x => x.Amount),
                        TopProduct = g.GroupBy(x => x.ProductID)
                                     .OrderByDescending(p => p.Sum(x => x.Amount))
                                     .Select(p => p.Key.ToString())
                                     .FirstOrDefault() ?? "Unknown"
                    })
                    .ToListAsync();

                await _dbContext.AggregatedSales.AddRangeAsync(aggregatedData);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("AggregatedSalesJob completed successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing AggregatedSalesJob");
                throw;
            }
        }
    }
}