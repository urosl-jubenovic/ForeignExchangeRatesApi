using System;

namespace ForeignExchangeRates.Service.Dto
{
    public class RateOnDate
    {
        public decimal Rate { set; get; }
        public DateTime Date { set; get; }
    }
}
