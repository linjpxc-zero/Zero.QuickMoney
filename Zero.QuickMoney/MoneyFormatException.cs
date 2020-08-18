using System;

namespace Zero.QuickMoney
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Zero.QuickMoney.CurrencyException" />
    public class MoneyFormatException : CurrencyException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoneyFormatException"/> class.
        /// </summary>
        public MoneyFormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoneyFormatException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MoneyFormatException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoneyFormatException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public MoneyFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}