using System.Threading.Tasks;
using ForeignExchangeRates.Service.Dto;

namespace ForeignExchangeRates.Service.Services
{
    public interface IExchangeRatesService
    {
        Task<ExchangeRatesResult> GetExchangeRates(ExchangeRatesRequest request);
    }
}
