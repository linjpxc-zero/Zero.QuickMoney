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

    /// <summary>
    /// Convert one type of <see cref="M"/> to another type of <see cref="M"/>.
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="M"></typeparam>
    public interface IExchangeRate<C, M> : IExchangeRate
        where C : ICurrency
        where M : IMoney
    {
        /// <summary>
        /// Get a value representing the base currency.
        /// </summary>
        new C BaseCurrency { get; }

        /// <summary>
        /// Get a value representing the quote currency.
        /// </summary>
        new C QuoteCurrency { get; }

        /// <summary>
        /// Converts the specified <see cref="IMoney">.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns>The converted money.</returns>
        /// <exception cref="ArgumentException">Money should have the same currency as the base currency or the quote currency.</exception>
        M Convert(M money);
    }
}
