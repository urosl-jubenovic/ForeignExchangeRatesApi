namespace ForeignExchangeRates.WebAPI.Dto
{
    public class ExchangeRatesRequestDto
    {
        public string Dates { set; get; } //Dates are expected in yyyy-MM-dd format and separated by ,

        public string BaseCurrency { set; get; }

        public string TargetCurrency { set; get; }
    }
}
