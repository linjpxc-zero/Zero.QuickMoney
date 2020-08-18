using System;
using System.Collections.Generic;

namespace Zero.QuickMoney
{
    public readonly struct ExchangeRate : IExchangeRate<CurrencyInfo, Money>, IEquatable<ExchangeRate>
    {
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

        public CurrencyInfo BaseCurrency { get; }

        public CurrencyInfo QuoteCurrency { get; }

        public decimal Rate { get; }

        ICurrency IExchangeRate.BaseCurrency => this.BaseCurrency;

        ICurrency IExchangeRate.QuoteCurrency => this.QuoteCurrency;

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

        public bool Equals(ExchangeRate other)
        {
            return this.BaseCurrency == other.BaseCurrency
                && this.QuoteCurrency == other.QuoteCurrency
                && this.Rate == other.Rate;
        }

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

        public override int GetHashCode()
        {
            int hashCode = -817766104;
            hashCode = hashCode * -1521134295 + Rate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<CurrencyInfo>.Default.GetHashCode(this.BaseCurrency);
            hashCode = hashCode * -1521134295 + EqualityComparer<CurrencyInfo>.Default.GetHashCode(this.QuoteCurrency);
            return hashCode;
        }

        public override string ToString()
            => $"{this.BaseCurrency}/{this.QuoteCurrency} {this.Rate}";

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
