namespace CurrencyRateService.Models
{
    public interface ICurrencyRateService
    {
        Task<decimal> GetRateAsync(string sourceCurrency, string targetCurrency, DateTime date);
    }
}
