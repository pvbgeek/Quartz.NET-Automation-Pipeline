using Microsoft.EntityFrameworkCore;

namespace JobScheduler.Processing
{
    public class AppDbContext : DbContext
    {
        public DbSet<StagingData> StagingData { get; set; }
        public DbSet<AggregatedSales> AggregatedSales { get; set; }
        public DbSet<MonthlyTrends> MonthlyTrends { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost;Port=5433;Database=postgres;Username=postgres;Password=newpassword",
                    options => 
                    {
                        options.CommandTimeout(300);
                        options.EnableRetryOnFailure(3);
                    });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StagingData>(entity =>
            {
                entity.HasKey(s => s.TransactionID);
                entity.Property(s => s.Region).IsRequired();
                entity.Property(s => s.Category).IsRequired();
            });

            modelBuilder.Entity<AggregatedSales>(entity =>
            {
                entity.HasKey(a => new { a.Region, a.Category });
                entity.Property(a => a.Region).IsRequired();
                entity.Property(a => a.Category).IsRequired();
                entity.Property(a => a.TopProduct).IsRequired();
            });

            modelBuilder.Entity<MonthlyTrends>(entity =>
            {
                entity.HasKey(m => new { m.Month, m.Category });
                entity.Property(m => m.Category).IsRequired();
            });
        }
    }
}