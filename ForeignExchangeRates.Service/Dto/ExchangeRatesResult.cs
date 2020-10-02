namespace ForeignExchangeRates.Service.Dto
{
    public class ExchangeRatesResult
    {
        public ExchangeRatesResult(RateOnDate minRate, RateOnDate maxRate, decimal averageRate)
        {
            MinRate = minRate;
            MaxRate = maxRate;
            AverageRate = averageRate;
        }

        public decimal AverageRate { get; }
        public RateOnDate MinRate { get; }
        public RateOnDate MaxRate { get; }
    }
}
