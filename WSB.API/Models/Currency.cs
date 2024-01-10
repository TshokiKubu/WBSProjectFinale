namespace WSB.API.Models
{
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public ICollection<RateHistory> RateHistories { get; set; }
    }
}
