using System;
namespace Zero.QuickMoney
{
    /// <summary>
    /// Convert one type of <see cref="IMoney"/> to another type of <see cref="IMoney"/>.
    /// </summary>
    public interface IExchangeRate
    {
        /// <summary>
        /// Get a value representing the base currency.
        /// </summary>
        ICurrency BaseCurrency { get; }

        /// <summary>
        /// Get a value representing the quote currency.
        /// </summary>
        ICurrency QuoteCurrency { get; }

        /// <summary>
        /// Get a value representing the exchange rate.
        /// </summary>
        decimal Rate { get; }

        /// <summary>
        /// Converts the specified <see cref="IMoney">.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns>The converted money.</returns>
        /// <exception cref="ArgumentException">Money should have the same currency as the base currency or the quote currency.</exception>
        IMoney Convert(IMoney money);
    }
}
