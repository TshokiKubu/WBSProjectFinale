using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using WSB.API.Models;

namespace WSB.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<RateHistory> RateHistories { get; set; }

        public DbSet<Conversion> Conversions { get; set; }
        public DbSet<OpenExchangeRatesResponse> RateOpenExchangeRatesResponseHistories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<RateHistory>()
                .HasOne(r => r.Currency)
                .WithMany(c => c.RateHistories)
                .HasForeignKey(r => r.CurrencyId);
           
        }
    }
}
