using System.Globalization;

namespace Zero.QuickMoney.Extensions
{
    public static class CurrencyExtensions
    {
        public static Money Money(this CurrencyInfo @this)
             => new Money(@this, decimal.Zero);

        public static Money Money(this CurrencyInfo @this, decimal amount)
            => new Money(@this, amount);

        public static CurrencyInfo GetCurrency(this RegionInfo @this)
            => CurrencyInfo.FromRegion(@this);

        public static CurrencyInfo GetCurrency(this CultureInfo @this)
            => CurrencyInfo.FromCulture(@this);
    }
}
