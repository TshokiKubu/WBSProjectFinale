using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace WSB.API.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, DistributedCacheEntryOptions options)
        {
            var serializedItem = await _cache.GetStringAsync(key);

            if (serializedItem == null)
            {
                var item = await getItemCallback();

                if (item != null)
                {
                    serializedItem = JsonConvert.SerializeObject(item);

                    await _cache.SetStringAsync(key, serializedItem, options);
                }
            }

            return serializedItem != null ? JsonConvert.DeserializeObject<T>(serializedItem) : default;
        }

        public async Task<decimal> GetOrSetExchangeRate(string key, Func<Task<decimal>> fetchFunction, TimeSpan cacheDuration)
        {
            var cachedValue = await _cache.GetStringAsync(key);

            if (cachedValue == null)
            {
                var exchangeRate = await fetchFunction();

                if (exchangeRate != default)
                {
                    cachedValue = JsonConvert.SerializeObject(exchangeRate);

                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = cacheDuration
                    };

                    await _cache.SetStringAsync(key, cachedValue, cacheOptions);
                }
            }

            return cachedValue != null ? JsonConvert.DeserializeObject<decimal>(cachedValue) : default;
        }
    }
}