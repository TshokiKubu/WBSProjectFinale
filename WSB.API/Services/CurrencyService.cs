using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WSB.API.Models;
using WSB.API.Services;

namespace WSB.API.Service
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CurrencyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<decimal?> GetLatestCurrencyRateAsync(string fromCurrency, string toCurrency)
        {
           string openExchangeRatesApiKey = "042d263f9fd21384b6de2af1"; 
            
            var apiUrl = $"https://open.er-api.com/v6/latest/{fromCurrency}?app_id={openExchangeRatesApiKey}&symbols={toCurrency}";

            using var httpClient = _httpClientFactory.CreateClient();

            try
            {
              
                var response = await httpClient.GetStringAsync(apiUrl);
             
                var jsonResponse = JsonConvert.DeserializeObject<OpenExchangeRatesResponse>(response);

                if (jsonResponse.Success && jsonResponse.Rates != null)
                {                
                    var latestRate = jsonResponse.Rates.ContainsKey(toCurrency) ? jsonResponse.Rates[toCurrency] : 0; // Default to 0 if toCurrency is not found
                    return latestRate;
                }
                else
                {
                   Console.WriteLine($"API request failed. Error: {jsonResponse.Error?.Info}");
                    return 0;
                }
            }
            catch (HttpRequestException ex)
            {            
                Console.WriteLine($"HTTP request failed. Error: {ex.Message}");
                return 0; 
            }
            catch (JsonException ex)
            {             
                Console.WriteLine($"JSON parsing failed. Error: {ex.Message}");
                return 0; 
            }

        }
    }
}