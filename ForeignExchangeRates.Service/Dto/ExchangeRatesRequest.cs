using System;
using System.Collections.Generic;

namespace ForeignExchangeRates.Service.Dto
{
    public class ExchangeRatesRequest
    {
        public List<DateTime> Dates { set; get; }

        public string BaseCurrency { set; get; }

        public string TargetCurrency { set; get; }
    }
}
