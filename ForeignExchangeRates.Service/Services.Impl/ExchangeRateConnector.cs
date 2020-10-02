using System;
using System.Text;
using System.Threading.Tasks;
using ForeignExchangeRates.Service.Dto;
using ForeignExchangeRates.Service.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using RestSharp;

namespace ForeignExchangeRates.Service.Services.Impl
{
    public class ExchangeRateConnector : IExchangeRateConnector
    {
        private const string HttpRequestPolicy = "HttpRequestPolicy";
        private readonly IAsyncPolicy _httpPolicy;

        private readonly ExchangeRatesApiSettings _exchangeRatesApiSettings;

        public ExchangeRateConnector(IOptions<ExchangeRatesApiSettings> options, IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _exchangeRatesApiSettings = options.Value;
            _httpPolicy = policyRegistry.Get<IAsyncPolicy>(HttpRequestPolicy); ;
        }

        public async Task<ExternalExchangeRatesResponse> GetExchangeRate(DateTime date, string baseCurrency, string targetCurrency)
        {
            return await _httpPolicy.ExecuteAsync(async () =>
            {

                var resourceUrl = GetResourceUrl(date, baseCurrency, targetCurrency);
                var request = new RestRequest(resourceUrl, DataFormat.Json);

                var client = new RestClient(_exchangeRatesApiSettings.Url);

                return await client.GetAsync<ExternalExchangeRatesResponse>(request);
            });
        }

        private string GetResourceUrl(DateTime date, string baseCurrency, string targetCurrency)
        {
            var stringBuilder = new StringBuilder($"/{date:yyyy-MM-dd}");
            stringBuilder.Append($"?base={baseCurrency}");
            stringBuilder.Append($"&symbols={targetCurrency}");

            return stringBuilder.ToString();
        }
    }
}
