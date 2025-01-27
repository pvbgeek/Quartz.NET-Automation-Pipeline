using Quartz;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JobScheduler.Processing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<AppDbContext>();

                    // Register FlaskApiService with HttpClient
                    services.AddHttpClient<FlaskApiService>();

                    // Register FlaskApiServiceJob
                    services.AddTransient<FlaskApiServiceJob>();

                    // Add Quartz and configure jobs
                    services.AddQuartz(q =>
                    {
                        // Register AggregatedSalesJob
                        var aggregatedJobKey = new JobKey("AggregatedSalesJob");
                        q.AddJob<AggregatedSalesJob>(opts => opts.WithIdentity(aggregatedJobKey));
                        q.AddTrigger(opts => opts
                            .ForJob(aggregatedJobKey)
                            .WithIdentity("AggregatedSalesTrigger")
                            .StartNow()); // Run immediately

                        // Register MonthlyTrendsJob
                        var trendsJobKey = new JobKey("MonthlyTrendsJob");
                        q.AddJob<MonthlyTrendsJob>(opts => opts.WithIdentity(trendsJobKey));
                        q.AddTrigger(opts => opts
                            .ForJob(trendsJobKey)
                            .WithIdentity("MonthlyTrendsTrigger")
                            .StartAt(DateBuilder.FutureDate(30, IntervalUnit.Second))); // 30-second delay

                        // Register FlaskApiServiceJob
                        var flaskJobKey = new JobKey("FlaskApiServiceJob");
                        q.AddJob<FlaskApiServiceJob>(opts => opts.WithIdentity(flaskJobKey));
                        q.AddTrigger(opts => opts
                            .ForJob(flaskJobKey)
                            .WithIdentity("FlaskApiServiceTrigger")
                            .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Minute))); // 1-minute delay
                    });

                    // Add Quartz hosted service
                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });

            var host = builder.Build();
            Console.WriteLine("Starting jobs. The application will exit after jobs complete.");
            await host.RunAsync();
        }
    }
}
