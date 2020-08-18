using System;

namespace Zero.QuickMoney
{
    /// <summary>
    /// Define a <see cref="IMoney"/> with a specific <see cref="ICurrency"/>.
    /// </summary>
    public interface IMoney : IFormattable
    {
        /// <summary>
        /// Get a value representing the amount.
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Get a value representing the currency unit of the amount.
        /// </summary>
        ICurrency Currency { get; }
    }
}
