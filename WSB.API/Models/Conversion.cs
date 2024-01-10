namespace WSB.API.Models
{
    public class Conversion
    {
        public int Id { get; set; }
       // public string? BaseCurrency { get; set; }
      //  public string? TargetCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Result { get; set; }
        public DateTime Timestamp { get; set; }

       public string? FromCurrency { get; set; }
        public string? ToCurrency { get; set; }
        public decimal Rate { get; set; }
    }
}
