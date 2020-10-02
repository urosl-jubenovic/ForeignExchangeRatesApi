using System;

namespace ForeignExchangeRates.Service.Extensions
{
    public class InvalidClientRequestException : Exception
    {
        public InvalidClientRequestException()
        {

        }

        public InvalidClientRequestException(string message)
            : base(message)
        {
        }
    }
}
