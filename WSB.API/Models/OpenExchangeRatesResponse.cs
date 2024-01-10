using Newtonsoft.Json;

namespace WSB.API.Models
{
    public class OpenExchangeRatesResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public OpenExchangeRatesError? Error{ get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, decimal>? Rates { get; set; }
    }

    public class OpenExchangeRatesError
    {
        [JsonProperty("info")]
        public string? Info { get; set; }
    }
}