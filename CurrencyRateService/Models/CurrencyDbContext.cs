using Microsoft.EntityFrameworkCore;

public class CurrencyDbContext : DbContext
{
    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options) : base(options) { }

    public DbSet<CurrencyRate> CurrencyRates => Set<CurrencyRate>();
}
