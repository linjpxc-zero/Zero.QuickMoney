using System;

namespace Zero.QuickMoney
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CurrencyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyException"/> class.
        /// </summary>
        public CurrencyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CurrencyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing"/> in Visual Basic) if no inner exception is specified.</param>
        public CurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}