using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace WSB.API.Services
{
    public interface IRedisCacheService
    {
       Task<decimal> GetOrSetExchangeRate(string key, Func<Task<decimal>> fetchFunction, TimeSpan cacheDuration);
     //   Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, DistributedCacheEntryOptions options);

    }
}
