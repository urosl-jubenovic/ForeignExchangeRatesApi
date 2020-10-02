using System;
using System.Linq;
using FluentValidation;
using ForeignExchangeRates.WebAPI.Dto;

namespace ForeignExchangeRates.WebAPI.Validators
{
    public class ExchangeRatesRequestDtoValidator : AbstractValidator<ExchangeRatesRequestDto>
    {

        public ExchangeRatesRequestDtoValidator()
        {
            RuleFor(x => x.Dates).NotEmpty()
                                          .Must(x => x?.Split(",").Select(stringDate => DateTime.TryParse(stringDate, out var date))
                                                                                  .All(parseResult => parseResult) ?? false)
                                          .WithMessage("One or more dates aren't in correct format (YYYY-MM-DD)! Use , as a separator.");

            RuleFor(x => x.BaseCurrency).NotEmpty();

            RuleFor(x => x.TargetCurrency).NotEmpty();
        }
    }
}
