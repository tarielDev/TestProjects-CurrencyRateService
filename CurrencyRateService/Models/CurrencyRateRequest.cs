namespace CurrencyRateService.Models
{
    public class CurrencyRateRequest
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public DateTime Date { get; set; }
    }
}
