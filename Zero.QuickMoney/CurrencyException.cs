using System;
namespace Zero.QuickMoney
{
    public class CurrencyException : Exception
    {
        public CurrencyException()
        {
        }

        public CurrencyException(string message)
            : base(message)
        {
        }

        public CurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
