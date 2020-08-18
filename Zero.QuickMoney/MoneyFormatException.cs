using System;
namespace Zero.QuickMoney
{
    public class MoneyFormatException : CurrencyException
    {
        public MoneyFormatException()
        {
        }

        public MoneyFormatException(string message) : base(message)
        {
        }

        public MoneyFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
