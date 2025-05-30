public class CurrencyRate
{
    public int Id { get; set; }
    public string SourceCurrency { get; set; }
    public string TargetCurrency { get; set; }
    public DateTime Date { get; set; }
    public decimal Rate { get; set; }
}
