using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ForeignExchangeRates.WebAPI.IntegrationTest
{
    public class ExchangeRatesControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "api/ExchangeRates";

        public ExchangeRatesControllerTest(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("2020-10-02,2020-01-06,2020-01-07", "sek", "Nok")]
        [InlineData("2020-02-04,2020-05-07,2020-04-02", "IDR", "PLn")]
        [InlineData("2020-07-03,2020-08-06,2020-02-12", "MXN", "usd")]

        public async Task GetTest(string dates, string baseCurrency, string targetCurrency)
        {
            var url = GetUrl(dates, baseCurrency, targetCurrency);
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(null, "sek", "nor")]
        [InlineData("", "EUR", "IDR")]
        [InlineData("2020-22-10", "Nor", "sek")]
        [InlineData("2052-01-10", "Nor", "sek")]
        public async Task GetValidationTestDates(string dates, string baseCurrency, string targetCurrency)
        {
            var url = GetUrl(dates, baseCurrency, targetCurrency);
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData("2020-01-03,2020-01-06,2020-01-07", "sek", "")]
        [InlineData("2020-10-02", "", "IDR")]
        [InlineData("2020-01-03,2020-01-06", "sek", null)]
        [InlineData("2020-01-03,2020-01-06", null, "IDR")]
        public async Task GetValidationTestCurrencies(string dates, string baseCurrency, string targetCurrency)
        {
            var url = GetUrl(dates, baseCurrency, targetCurrency);
            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        private static string GetUrl(string dates, string baseCurrency, string targetCurrency)
        {
            return $"{BaseUrl}?dates={dates}&baseCurrency={baseCurrency}&targetCurrency={targetCurrency}";
        }
    }
}
