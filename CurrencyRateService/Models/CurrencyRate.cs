public class CurrencyRate
{
    public int Id { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Rate { get; set; }
}
