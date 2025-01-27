using Quartz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobScheduler.Processing
{
   public class MonthlyTrendsJob : IJob
   {
       private readonly AppDbContext _dbContext;
       private readonly ILogger<MonthlyTrendsJob> _logger;

       public MonthlyTrendsJob(AppDbContext dbContext, ILogger<MonthlyTrendsJob> logger)
       {
           _dbContext = dbContext;
           _logger = logger;
       }

       public async Task Execute(IJobExecutionContext context)
       {
           try
           {
               _logger.LogInformation("Starting MonthlyTrendsJob...");

               await _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"MonthlyTrends\"");
               _logger.LogInformation("Processing monthly trends...");

               var trendsData = await _dbContext.StagingData
                   .GroupBy(s => new 
                   { 
                       Month = new DateTime(DateTime.UtcNow.Year, s.Date.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                       s.Category 
                   })
                   .Select(g => new MonthlyTrends
                   {
                       Month = g.Key.Month,
                       Category = g.Key.Category ?? "Unknown",
                       SalesGrowth = g.Sum(x => x.Amount)
                   })
                   .ToListAsync();

               await _dbContext.MonthlyTrends.AddRangeAsync(trendsData);
               await _dbContext.SaveChangesAsync();

               _logger.LogInformation("MonthlyTrendsJob completed successfully!");
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error occurred while executing MonthlyTrendsJob");
               throw;
           }
       }
   }
}