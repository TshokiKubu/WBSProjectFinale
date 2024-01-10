using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSB.API.Data;
using WSB.API.Models;

namespace WSB.API.Repository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _context;

        public CurrencyRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Currency>> GetAllCurrencies()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task<int> AddCurrency(Currency currency)
        {
            _context.Currencies.Add(currency);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RateHistory>> GetRateHistory(int currencyId)
        {
            return await _context.RateHistories
                .Where(r => r.CurrencyId == currencyId)
                .ToListAsync();
        }

        public async Task<int> AddRateHistory(RateHistory rateHistory)
        {
            _context.RateHistories.Add(rateHistory);
            return await _context.SaveChangesAsync();
        }
    }
}
