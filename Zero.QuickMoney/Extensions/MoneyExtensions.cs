using System;
namespace Zero.QuickMoney.Extensions
{
    public static class MoneyExtensions
    {
        public static int ToMinor(this Money @this)
            => (int)(@this / @this.Currency.MinorUnit);

        public static Money ConvertTo(this Money @this, CurrencyInfo quoteCurrency, decimal rate)
        {
            if (@this.Currency == quoteCurrency)
            {
                return @this;
            }
            return new ExchangeRate(@this.Currency, quoteCurrency, rate).Convert(@this);
        }

        public static Money ConvertTo(this Money @this, ExchangeRate rate)
            => rate.Convert(@this);
    }
}
