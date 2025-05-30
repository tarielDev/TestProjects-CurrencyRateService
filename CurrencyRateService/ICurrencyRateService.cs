namespace CurrencyRateService
{
    public interface ICurrencyRateService
    {
        Task<decimal> GetRateAsync(string currency, DateTime date);
    }
}
