using System;

namespace Zero.QuickMoney
{
    /// <summary>
    /// Basic currency information.
    /// </summary>
    /// <seealso cref="IEquatable{ICurrency}"/>
    public interface ICurrency : IEquatable<ICurrency>
    {
        /// <summary>
        /// Get a value representing the organization of the curreny, link ISO-4217.
        /// </summary>
        /// <value>
        /// The organization.
        /// </value>
        string Organization { get; }

        /// <summary>
        /// Get a value representing the currency code.
        /// </summary>
        /// <value>
        /// The currency code.
        /// </value>
        string Code { get; }

        /// <summary>
        /// Get a value representing the symbol of the currency.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        string Symbol { get; }

        /// <summary>
        /// Gets a value that represents the number of decimal places used in currency values.
        /// </summary>
        decimal DecimalDigits { get; }
    }
}
