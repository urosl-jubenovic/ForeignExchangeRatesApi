using System;
using System.Threading.Tasks;
using ForeignExchangeRates.Service.Dto;

namespace ForeignExchangeRates.Service.Services
{
    public interface IExchangeRateConnector
    {
        Task<ExternalExchangeRatesResponse> GetExchangeRate(DateTime date, string baseCurrency, string targetCurrency);
    }
}
