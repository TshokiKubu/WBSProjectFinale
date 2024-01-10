using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using WSB.API.Services;

namespace WSB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IRedisCacheService _cacheService;
        private readonly ExternalDataService _externalDataService;

        public DataController(IRedisCacheService cacheService, ExternalDataService externalDataService)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _externalDataService = externalDataService ?? throw new ArgumentNullException(nameof(externalDataService));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDataById(string id)
        {           
            Func<Task<decimal>> fetchExchangeRateCallback = async () =>
            {
                var exchangeRateString = await _externalDataService.GetDataById(id);

                if (decimal.TryParse(exchangeRateString, out var exchangeRate))
                {
                    return exchangeRate;
                }
                else
                {
                      return 0.0m; 
                }
            };
         
            var cacheDuration = TimeSpan.FromMinutes(10);

            try
            {
                 var exchangeRate = await _cacheService.GetOrSetExchangeRate(id, fetchExchangeRateCallback, cacheDuration);
         
                return Ok(exchangeRate);
            }
            catch (Exception ex)
            {                
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}