using Microsoft.EntityFrameworkCore;

namespace CurrencyRateService.Models
{
    public class CurrencyDbContext : DbContext
    {
        public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options)
            : base(options) { }

        public DbSet<CurrencyRate> CurrencyRates { get; set; }
    }
}
