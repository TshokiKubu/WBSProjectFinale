using System.Threading.Tasks;

namespace WSB.API.Services
{
    public interface ICurrencyService
    {
        Task<decimal?> GetLatestCurrencyRateAsync(string fromCurrency, string toCurrency);
    }
}
