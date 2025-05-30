using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CurrencyRateService.Models
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly CurrencyDbContext _db;
        private readonly HttpClient _http;

        public CurrencyRateService(CurrencyDbContext db, HttpClient http)
        {
            _db = db;
            _http = http;
        }

        public async Task<decimal> GetRateAsync(string currency, DateTime date)
        {
            var rate = await _db.CurrencyRates.FirstOrDefaultAsync(x => x.Currency == currency && x.Date == date);
            if (rate != null) return rate.Rate;

            var url = $"https://api.exchangerate.host/{date:yyyy-MM-dd}?base=USD&symbols={currency}";
            var response = await _http.GetFromJsonAsync<JsonElement>(url);
            var rateValue = response.GetProperty("rates").GetProperty(currency).GetDecimal();

            _db.CurrencyRates.Add(new CurrencyRate { Currency = currency, Date = date, Rate = rateValue });
            await _db.SaveChangesAsync();

            return rateValue;
        }
    }

}
