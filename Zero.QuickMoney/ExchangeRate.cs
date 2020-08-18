using System;
using System.Collections.Generic;

namespace Zero.QuickMoney
{
    /// <summary>
    /// Convert one type of <see cref="Money"/> to another type of <see cref="Money"/>.
    /// </summary>
    /// <seealso cref="Zero.QuickMoney.IExchangeRate{Zero.QuickMoney.CurrencyInfo, Zero.QuickMoney.Money}" />
    /// <seealso cref="System.IEquatable{Zero.QuickMoney.ExchangeRate}" />
    public readonly struct ExchangeRate : IExchangeRate<CurrencyInfo, Money>, IEquatable<ExchangeRate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRate"/> struct.
        /// </summary>
        /// <param name="baseCurrency">The base currency.</param>
        /// <param name="quoteCurrency">The quote currency.</param>
        /// <param name="rate">The rate.</param>
        /// <exception cref="ArgumentException">
        /// Is not a valid currency. - baseCurrency
        /// or
        /// Is not a valid currency. - quoteCurrency
        /// or
        /// The base currency cannot be the same as the quote currency.
        /// or
        /// Rate must be greater than zero.
        /// </exception>
        public ExchangeRate(CurrencyInfo baseCurrency, CurrencyInfo quoteCurrency, decimal rate)
        {
            if (string.IsNullOrEmpty(baseCurrency.Code))
            {
                throw new ArgumentException("Is not a valid currency.", nameof(baseCurrency));
            }
            if (string.IsNullOrEmpty(quoteCurrency.Code))
            {
                throw new ArgumentException("Is not a valid currency.", nameof(quoteCurrency));
            }
            if (baseCurrency == quoteCurrency)
            {
                throw new ArgumentException("The base currency cannot be the same as the quote currency.");
            }
            if (rate <= decimal.Zero)
            {
                throw new ArgumentException("Rate must be greater than zero.");
            }

            this.BaseCurrency = baseCurrency;
            this.QuoteCurrency = quoteCurrency;
            this.Rate = rate;
        }

        /// <summary>
        /// Get a value representing the base currency.
        /// </summary>
        public CurrencyInfo BaseCurrency { get; }

        /// <summary>
        /// Get a value representing the quote currency.
        /// </summary>
        public CurrencyInfo QuoteCurrency { get; }

        /// <summary>
        /// Get a value representing the exchange rate.
        /// </summary>
        public decimal Rate { get; }

        /// <summary>
        /// Get a value representing the base currency.
        /// </summary>
        ICurrency IExchangeRate.BaseCurrency => this.BaseCurrency;

        /// <summary>
        /// Get a value representing the quote currency.
        /// </summary>
        ICurrency IExchangeRate.QuoteCurrency => this.QuoteCurrency;

        /// <summary>
        /// Converts the specified <see cref="Money">.
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The currency of money should be the same as the base currency or quote currency.</exception>
        /// <!-- Badly formed XML comment ignored for member "M:Zero.QuickMoney.IExchangeRate`2.Convert(`1)" -->
        public Money Convert(Money money)
        {
            if (money.Currency == this.BaseCurrency)
            {
                return new Money(this.QuoteCurrency, (decimal)money * this.Rate);
            }
            if (money.Currency == this.QuoteCurrency)
            {
                return new Money(this.BaseCurrency, (decimal)money / this.Rate);
            }
            throw new ArgumentException("The currency of money should be the same as the base currency or quote currency.");
        }

        /// <summary>
        /// Converts the specified <see cref="Money">.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">money</exception>
        /// <exception cref="ArgumentException">Not for {nameof(Money)}.</exception>
        IMoney IExchangeRate.Convert(IMoney money)
        {
            if (money == null)
            {
                throw new ArgumentNullException(nameof(money));
            }
            if (money is Money tmp)
            {
                return this.Convert(tmp);
            }
            throw new ArgumentException($"Not for {nameof(Money)}.");
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(ExchangeRate other)
        {
            return this.BaseCurrency == other.BaseCurrency
                && this.QuoteCurrency == other.QuoteCurrency
                && this.Rate == other.Rate;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is ExchangeRate exchangeRate)
            {
                return this.Equals(exchangeRate);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            int hashCode = -817766104;
            hashCode = hashCode * -1521134295 + Rate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<CurrencyInfo>.Default.GetHashCode(this.BaseCurrency);
            hashCode = hashCode * -1521134295 + EqualityComparer<CurrencyInfo>.Default.GetHashCode(this.QuoteCurrency);
            return hashCode;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
            => $"{this.BaseCurrency}/{this.QuoteCurrency} {this.Rate}";

        /// <summary>
        /// Convert the specified string to the equivalent <see cref="ExchangeRate"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="FormatException">Is not a valid exchange rate format.</exception>
        public static ExchangeRate Parse(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (TryParse(value, out var result))
            {
                return result;
            }
            throw new FormatException("Is not a valid exchange rate format.");
        }

        /// <summary>
        /// Convert the specified string to the equivalent <see cref="ExchangeRate"/>, a return value indicating whether it was successful.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryParse(string value, out ExchangeRate result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            value = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (value.Length < 6)
            {
                return false;
            }

            if (CurrencyInfo.TryFromCode(value.Substring(0, 3), out var baseCurrency))
            {
                var index = value.IndexOf("/", StringComparison.OrdinalIgnoreCase);
                if (index < 0)
                {
                    index = 3;
                }
                else
                {
                    index++;
                }
                if (value.Length - index < 3)
                {
                    return false;
                }
                if (CurrencyInfo.TryFromCode(value.Substring(index, 3), out var quoteCurrency))
                {
                    if (decimal.TryParse(value.Substring(index + 3).Trim(), out var rate))
                    {
                        result = new ExchangeRate(baseCurrency, quoteCurrency, rate);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool operator ==(ExchangeRate left, ExchangeRate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExchangeRate left, ExchangeRate right)
        {
            return !(left == right);
        }
    }
}