using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurrencyRateService.Models
{
    public class GetCurrencyRateService : ICurrencyRateService
    {
        private readonly CurrencyDbContext _db;
        private readonly HttpClient _http;

        public GetCurrencyRateService(CurrencyDbContext db, HttpClient http)
        {
            _db = db;
            _http = http;
        }

        public async Task<decimal> GetRateAsync(string sourceCurrency, string targetCurrency, DateTime date)
        {
            // Приводим к верхнему регистру
            sourceCurrency = sourceCurrency.ToUpper();
            targetCurrency = targetCurrency.ToUpper();

            // Ищем в БД
            var rate = await _db.CurrencyRates.FirstOrDefaultAsync(r =>
                r.SourceCurrency == sourceCurrency &&
                r.TargetCurrency == targetCurrency &&
                r.Date == date.Date);

            if (rate != null)
                return rate.Rate;

            // Строим URL
            string url = $"https://api.exchangerate.host/historical?access_key=c25a9b7c157b453362008fa6c86d3e3f" +
                            $"&date={date:yyyy-MM-dd}&source={sourceCurrency}&currencies={targetCurrency}&format=1";

            var response = await _http.GetFromJsonAsync<JsonElement>(url);

            if (!response.GetProperty("success").GetBoolean())
            {
                throw new Exception("Ошибка при запросе API: " + response);
            }

            var quotes = response.GetProperty("quotes");
            var key = sourceCurrency + targetCurrency;

            if (!quotes.TryGetProperty(key, out var rateValue))
                throw new Exception($"Курс валюты {key} не найден в ответе API");

            var rateDecimal = rateValue.GetDecimal();

            // Сохраняем в БД
            _db.CurrencyRates.Add(new CurrencyRate
            {
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency,
                Date = date.Date,
                Rate = rateDecimal
            });

            await _db.SaveChangesAsync();

            return rateDecimal;
        }
    }

    public class ExchangeRateResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("quotes")]
        public Dictionary<string, decimal> Quotes { get; set; }
    }



}

