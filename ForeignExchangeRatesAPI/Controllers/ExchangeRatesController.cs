using System.Threading.Tasks;
using AutoMapper;
using ForeignExchangeRates.Service.Dto;
using Microsoft.AspNetCore.Mvc;
using ForeignExchangeRates.WebAPI.Dto;
using ForeignExchangeRates.Service.Services;

namespace ForeignExchangeRates.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController : Controller
    {
        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly IMapper _mapper;


        public ExchangeRatesController(IExchangeRatesService exchangeRatesService, IMapper mapper)
        {
            _exchangeRatesService = exchangeRatesService;
            _mapper = mapper;
        }

        //Dates are expected in yyyy-MM-dd format and separated by ,
        [HttpGet]
        public async Task<ExchangeRatesResult> Get([FromQuery] ExchangeRatesRequestDto requestDto)
        {
            var request = _mapper.Map<ExchangeRatesRequest>(requestDto);
            var result = await _exchangeRatesService.GetExchangeRates(request);
            return result;
        }
    }

}
