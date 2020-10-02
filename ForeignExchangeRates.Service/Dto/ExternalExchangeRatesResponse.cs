using System;
using System.Collections.Generic;

namespace ForeignExchangeRates.Service.Dto
{
    public class ExternalExchangeRatesResponse
    {
        public string Base { set; get; }

        public DateTime Date { set; get; }

        public Dictionary<string, decimal> Rates { set; get; }
    }
}
