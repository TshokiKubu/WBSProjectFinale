namespace WSB.API.Models
{
    public class RateHistory
    {
        public int RateHistoryId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Rate { get; set; }
        public DateTime RecordedAt { get; set; }

        public Currency? Currency { get; set; }
    }
}
