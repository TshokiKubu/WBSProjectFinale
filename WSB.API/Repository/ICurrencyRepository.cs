using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WSB.API.Models;

namespace WSB.API.Repository
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetAllCurrencies();
        Task<int> AddCurrency(Currency currency);
        Task<IEnumerable<RateHistory>> GetRateHistory(int currencyId);
        Task<int> AddRateHistory(RateHistory rateHistory);
    }
}
