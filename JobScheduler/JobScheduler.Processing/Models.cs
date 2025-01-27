using System.ComponentModel.DataAnnotations;

namespace JobScheduler.Processing
{
    public class StagingData
    {
        [Key]
        public long TransactionID { get; set; }
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Region { get; set; } = "Unknown";
        public string Category { get; set; } = "Unknown";
    }

    public class AggregatedSales
    {
        public string Region { get; set; } = "Unknown";
        public string Category { get; set; } = "Unknown";
        public decimal TotalSales { get; set; }
        public decimal AverageSales { get; set; }
        public string TopProduct { get; set; } = "Unknown";
    }

    public class MonthlyTrends
    {
        public DateTime Month { get; set; }
        public string Category { get; set; } = "Unknown";
        public decimal SalesGrowth { get; set; }
    }
}