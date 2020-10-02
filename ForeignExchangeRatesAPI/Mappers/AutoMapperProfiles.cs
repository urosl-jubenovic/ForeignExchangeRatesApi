using System;
using System.Linq;
using AutoMapper;
using ForeignExchangeRates.Service.Dto;
using ForeignExchangeRates.WebAPI.Dto;

namespace ForeignExchangeRates.WebAPI.Mappers
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<ExchangeRatesRequestDto, ExchangeRatesRequest>().ForMember(
                    dest => dest.Dates,
                    opt => opt.MapFrom(
                        src => src.Dates.Split(',', StringSplitOptions.None).Select(DateTime.Parse).ToList())
                )

                .ForMember(
                    dest => dest.BaseCurrency,
                    opt => opt.MapFrom(src => src.BaseCurrency.ToUpper()))

                .ForMember(
                    dest => dest.TargetCurrency,
                    opt => opt.MapFrom(src => src.TargetCurrency.ToUpper()));
        }
    }
}