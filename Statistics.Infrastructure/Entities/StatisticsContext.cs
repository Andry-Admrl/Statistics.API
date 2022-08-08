using Microsoft.EntityFrameworkCore;
using Statistics.Infrastructure.Configuration;

namespace Statistics.Infrastructure.Entities
{
    public class StatisticsContext : DbContext
    {
        public StatisticsContext(DbContextOptions<StatisticsContext> options) : base(options)
        {
        }
        public DbSet<CallInfo> Calls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CallInfo>().HasIndex(c => new { c.BeginDateTime,c.EndDateTime});
            modelBuilder.ApplyConfiguration(new CallConfiguration());
        }
    }
}
