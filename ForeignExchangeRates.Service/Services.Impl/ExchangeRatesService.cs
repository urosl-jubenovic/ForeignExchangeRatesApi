using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForeignExchangeRates.Service.Dto;
using ForeignExchangeRates.Service.Extensions;

namespace ForeignExchangeRates.Service.Services.Impl
{
    public class ExchangeRatesService : IExchangeRatesService
    {

        private readonly IExchangeRateConnector _connector;


        private readonly HashSet<string> _currencyList = new HashSet<string>()
        {
            "CAD","HKD","ISK","PHP","DKK","HUF","CZK","AUD",
            "IDR","INR", "BRL","RUB","CNY","NOK","SEK","RON",
            "HRK","JPY","THB","CHF","SGD","PLN","BGN","TRY",
            "NZD","ZAR","USD","MXN","ILS","GBP","KRW","MYR"
        };

        public ExchangeRatesService(IExchangeRateConnector connector)
        {
            _connector = connector;
        }

        public async Task<ExchangeRatesResult> GetExchangeRates(ExchangeRatesRequest request)
        {
            ValidateCurrencies(request.BaseCurrency, request.TargetCurrency);
            ValidateDates(request.Dates);

            var tasks = request.Dates.Select(date => _connector.GetExchangeRate(date, request.BaseCurrency, request.TargetCurrency));
            var exchangeRatesResponses = await Task.WhenAll(tasks);

            var dateRateList = exchangeRatesResponses.Select(x => new { x.Date, Rate = x.Rates.First().Value })
                                                     .OrderBy(x => x.Rate)
                                                     .ToList();

            var minRate = dateRateList.First();
            var maxRate = dateRateList.Last();
            var average = dateRateList.Average(x => x.Rate);

            var minRateDate = new RateOnDate { Rate = minRate.Rate, Date = minRate.Date };
            var maxRateDate = new RateOnDate { Rate = maxRate.Rate, Date = maxRate.Date };

            return new ExchangeRatesResult(minRateDate, maxRateDate, average);
        }

        private void ValidateCurrencies(string baseCurrency, string targetCurrency)
        {
            if (_currencyList.Contains(baseCurrency) &&
                _currencyList.Contains(targetCurrency)) return;

            var message = "Base or target currency are not valid! " +
                          "Value for currency can be one from following: " +
                          $"{string.Join(",", _currencyList)}";

            throw new InvalidClientRequestException(message);
        }

        private void ValidateDates(List<DateTime> requestDates)
        {
            var allDatesAreNotFromFuture = requestDates.All(x => x.Date <= DateTime.Now.Date);
            if (allDatesAreNotFromFuture) return;

            var message = $"Biggest date can be today {DateTime.Now:yyyy-MM-dd}";

            throw new InvalidClientRequestException(message);
        }

    }
}
