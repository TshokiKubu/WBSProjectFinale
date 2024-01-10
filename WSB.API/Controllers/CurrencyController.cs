using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WSB.API.Models;
using WSB.API.Repository;
using WSB.API.Services;


namespace WSB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyService currencyService, ICurrencyRepository currencyRepository)
        {
            _currencyService = currencyService;
            _currencyRepository = currencyRepository;
        }

        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency([FromQuery] string fromCurrency, [FromQuery] string toCurrency)
        {
            var rate = await _currencyService.GetLatestCurrencyRateAsync(fromCurrency, toCurrency);

            if (rate.HasValue)
            {
                return Ok(new Conversion
                {
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    Rate = rate.Value
                });
            }
            else
            {
                return BadRequest("Failed to fetch the latest currency rate.");
            }
        }

        [HttpGet("currencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var currencies = await _currencyRepository.GetAllCurrencies();

            if (currencies != null)
            {
                return Ok(currencies);
            }
            else
            {
                return NotFound("No currencies found.");
            }
        }

        [HttpPost("addcurrency")]
        public async Task<IActionResult> AddCurrency([FromBody] Currency currency)
        {
            var result = await _currencyRepository.AddCurrency(currency);

            if (result > 0)
            {
                return Ok("Currency added successfully.");
            }
            else
            {
                return BadRequest("Failed to add currency.");
            }
        }

        [HttpGet("ratehistory/{currencyId}")]
        public async Task<IActionResult> GetRateHistory(int currencyId)
        {
            var rateHistory = await _currencyRepository.GetRateHistory(currencyId);

            if (rateHistory != null)
            {
                return Ok(rateHistory);
            }
            else
            {
                return NotFound($"No rate history found for currency with ID {currencyId}.");
            }
        }

        [HttpPost("addratehistory")]
        public async Task<IActionResult> AddRateHistory([FromBody] RateHistory rateHistory)
        {
            var result = await _currencyRepository.AddRateHistory(rateHistory);

            if (result > 0)
            {
                return Ok("Rate history added successfully.");
            }
            else
            {
                return BadRequest("Failed to add rate history.");
            }
        }
    }
}