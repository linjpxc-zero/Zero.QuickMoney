using System;
using System.Globalization;

namespace Zero.QuickMoney
{
    /// <summary>
    /// Define a <see cref="Money"/> with a specific <see cref="CurrencyInfo"/>.
    /// </summary>
    /// <seealso cref="Zero.QuickMoney.IMoney" />
    /// <seealso cref="System.IEquatable{Zero.QuickMoney.Money}" />
    /// <seealso cref="System.IComparable" />
    /// <seealso cref="System.IComparable{Zero.QuickMoney.Money}" />
    public readonly struct Money : IMoney, IEquatable<Money>, IComparable, IComparable<Money>
    {
        private readonly decimal amount;

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> struct.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <param name="amount">The amount.</param>
        /// <exception cref="ArgumentException">Is not a valid currency. - currency</exception>
        public Money(CurrencyInfo currency, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(currency.Code))
            {
                throw new ArgumentException("Is not a valid currency.", nameof(currency));
            }
            this.Currency = currency;
            this.amount = Math.Round(amount, (int)currency.DecimalDigits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Get a value representing the amount.
        /// </summary>
        decimal IMoney.Amount => this.amount;

        /// <summary>
        /// Get a value representing the currency unit of the amount.
        /// </summary>
        ICurrency IMoney.Currency => this.Currency;

        /// <summary>
        /// Get a value representing the currency unit of the amount.
        /// </summary>
        public CurrencyInfo Currency { get; }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This instance precedes <paramref name="obj" /> in the sort order.
        /// Zero
        /// This instance occurs in the same position in the sort order as <paramref name="obj" />.
        /// Greater than zero
        /// This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        /// <exception cref="ArgumentException">obj</exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            if (obj is Money money)
            {
                return this.CompareTo(money);
            }
            throw new ArgumentException("", nameof(obj));
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This instance precedes <paramref name="other" /> in the sort order.
        /// Zero
        /// This instance occurs in the same position in the sort order as <paramref name="other" />.
        /// Greater than zero
        /// This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        public int CompareTo(Money other)
        {
            AssertIsSameCurrency(this.Currency, other.Currency);
            return this.amount.CompareTo(other.amount);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
            => ToString(null, null);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
            => ToString(format, null);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider provider)
        {
            if (!string.IsNullOrWhiteSpace(format) && format.StartsWith("I", StringComparison.OrdinalIgnoreCase) && (format.Length >= 1 && format.Length <= 2))
            {
                format = format.Replace("I", "C").Replace("i", "C");
                provider = GetFormatProvider(this.Currency, provider, true);
            }
            else
            {
                provider = GetFormatProvider(this.Currency, provider, false);
            }
            return this.amount.ToString(format ?? "C", provider);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
            => obj is Money money && Equals(money);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(Money other)
            => this.Currency == other.Currency
            && this.amount == other.amount;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            int hashCode = -1315411705;
            hashCode = hashCode * -1521134295 + amount.GetHashCode();
            hashCode = hashCode * -1521134295 + Currency.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Use <see cref="CurrencyInfo.CurrentCurrency"/> to represent the amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static Money Current(decimal amount)
            => new Money(CurrencyInfo.CurrentCurrency, amount);

        /// <summary>
        /// Compares two instance objects and returns an integer indicating whether the position of the <paramref name="left"/> instance in the sort order is before, after, or the same as the position of the <paramref name="right"/> object.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object.</param>
        /// <returns>A value indicating the relative order of the objects to be compared. The meaning of the return value is as follows: Value meaning is less than zero <paramref name="left"/> is before <paramref name="right"/> in the sort order. The position of zero <paramref name="left"/> in the sort order is the same as <paramref name="right"/>. Greater than zero <paramref name="left"/> is after <paramref name="right"/> in the sort order.</returns>
        public static int Compare(Money left, Money right)
            => left.CompareTo(right);

        /// <summary>
        /// Add two specified <see cref="Money"/> together.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <exception cref="InvalidOperationException">The requested operation expected the currency <paramref name="left"/>, but the actual value was the currency <paramref name="right"/>!</exception>
        /// <returns></returns>
        public static Money Add(Money left, Money right)
        {
            AssertIsSameCurrency(left.Currency, right.Currency);
            return new Money(left.Currency, decimal.Add(left.amount, right.amount));
        }

        /// <summary>
        /// Add <see cref="Money"/> to the specified <see cref="decimal"/>.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Money Add(Money money, decimal value)
            => new Money(money.Currency, decimal.Add(money.amount, value));

        /// <summary>
        /// Subtract the specified value from one <see cref="Money"/> value.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static Money Subtract(Money left, Money right)
        {
            AssertIsSameCurrency(left.Currency, right.Currency);
            return new Money(left.Currency, decimal.Subtract(left.amount, right.amount));
        }

        /// <summary>
        /// Subtract a specified <see cref="decimal"/> value from a <see cref="Money"/> value.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Money Subtract(Money money, decimal value)
            => new Money(money.Currency, decimal.Subtract(money.amount, value));

        /// <summary>
        /// Subtract a specified <see cref="Money"/> value from a <see cref="decimal"/> value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static Money Subtract(decimal value, Money money)
            => new Money(money.Currency, decimal.Subtract(value, money.amount));

        /// <summary>
        /// Multiply two specified <see cref="Money"/>.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static Money Multiply(Money left, Money right)
        {
            AssertIsSameCurrency(left.Currency, right.Currency);
            return new Money(left.Currency, decimal.Multiply(left.amount, right.amount));
        }

        /// <summary>
        /// Multiply <see cref="Money"/> by a specified <see cref="decimal"/> value.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Money Multiply(Money money, decimal value)
            => new Money(money.Currency, decimal.Multiply(money.amount, value));

        /// <summary>
        /// Divide two specified <see cref="Money"/>.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static Money Divide(Money left, Money right)
        {
            AssertIsSameCurrency(left.Currency, right.Currency);
            return new Money(left.Currency, decimal.Divide(left.amount, right.amount));
        }

        /// <summary>
        /// Divide <see cref="Money"/> by a specified <see cref="decimal"/> value.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Money Divide(Money money, decimal value)
            => new Money(money.Currency, decimal.Divide(money.amount, value));

        /// <summary>
        /// Divide the specified <see cref="decimal"/> value by a <see cref="Money"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static Money Divide(decimal value, Money money)
            => new Money(money.Currency, decimal.Divide(value, money.amount));

        /// <summary>
        /// Returns the result of multiplying the specified <see cref="Money" /> value by negative one.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static Money Negate(Money money)
            => new Money(money.Currency, decimal.Negate(money.amount));

        /// <summary>
        /// Converts the value of a specified instance of <see cref="Money" /> to its equivalent binary representation.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static int[] GetBits(Money money)
            => decimal.GetBits(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent 8-bit unsigned integer.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static byte ToByte(Money money)
            => decimal.ToByte(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent 8-bit signed integer.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static sbyte ToSByte(Money money)
            => decimal.ToSByte(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent 16-bit signed integer.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static short ToInt16(Money money)
            => decimal.ToInt16(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent 16-bit unsigned integer.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ushort ToUInt16(Money money)
            => decimal.ToUInt16(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent 32-bit signed integer.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static int ToInt32(Money money)
            => decimal.ToInt32(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent 32-bit unsigned integer.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static uint ToUInt32(Money money)
            => decimal.ToUInt32(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent 64-bit signed integer.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static long ToInt64(Money money)
            => decimal.ToInt64(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent 64-bit unsigned integer.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ulong ToUInt64(Money money)
            => decimal.ToUInt64(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent single-precision floating-point number.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static float ToSingle(Money money)
            => decimal.ToSingle(money.amount);

        /// <summary>
        /// Converts the value of the specified <see cref="Money" /> to the equivalent double-precision floating-point number.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        public static double ToDouble(Money money)
            => decimal.ToDouble(money.amount);

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Money" /> equivalent.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">value</exception>
        /// <exception cref="MoneyFormatException">Is not a valid money format.</exception>
        public static Money Parse(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (TryParse(value, out var money))
            {
                return money;
            }
            throw new MoneyFormatException("Is not a valid money format.");
        }

        /// <summary>
        /// Converts the string representation of a number to its <see cref="Money" /> equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryParse(string value, out Money result)
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
            if (char.IsNumber(value[0]))
            {
                if (decimal.TryParse(value, out var tmp))
                {
                    result = new Money(CurrencyInfo.CurrentCurrency, tmp);
                    return true;
                }
                return false;
            }

            if (value.Length < 3)
            {
                return false;
            }
            if (CurrencyInfo.TryFromCode(value.Substring(0, 3), out var currency))
            {
                if (value.Length == 3)
                {
                    result = new Money(currency, decimal.Zero);
                }
                else
                {
                    if (decimal.TryParse(value.Substring(3).Trim().TrimStart(currency.Symbol.ToCharArray()), out var tmp))
                    {
                        result = new Money(currency, tmp);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static bool operator ==(Money left, Money right)
            => left.Equals(right);

        public static bool operator !=(Money left, Money right)
            => !(left == right);

        public static Money operator +(Money left, Money right)
            => Add(left, right);

        public static Money operator +(Money money, decimal value)
            => Add(money, value);

        public static Money operator +(decimal value, Money money)
            => Add(money, value);

        public static Money operator -(Money left, Money right)
            => Subtract(left, right);

        public static Money operator -(Money money, decimal value)
            => Subtract(money, value);

        public static Money operator -(decimal value, Money money)
            => Subtract(value, money);

        public static Money operator *(Money left, Money right)
            => Multiply(left, right);

        public static Money operator *(Money money, decimal value)
            => Multiply(money, value);

        public static Money operator *(decimal value, Money money)
            => Multiply(money, value);

        public static Money operator /(Money left, Money right)
            => Divide(left, right);

        public static Money operator /(Money money, decimal value)
            => Divide(money, value);

        public static Money operator /(decimal value, Money money)
            => Divide(value, money);

        public static bool operator >(Money left, Money right)
            => left.CompareTo(right) > 0;

        public static bool operator <(Money left, Money right)
            => left.CompareTo(right) < 0;

        public static bool operator >=(Money left, Money right)
            => left.CompareTo(right) >= 0;

        public static bool operator <=(Money left, Money right)
            => left.CompareTo(right) <= 0;

        public static explicit operator double(Money money)
            => (double)money.amount;

        public static explicit operator long(Money money)
            => (long)money.amount;

        public static explicit operator decimal(Money money)
            => money.amount;

        public static explicit operator float(Money money)
            => (float)money.amount;

        public static implicit operator Money(long value)
            => Current(value);

        [CLSCompliant(false)]
        public static implicit operator Money(ulong value)
            => Current(value);

        public static implicit operator Money(byte value)
            => Current(value);

        [CLSCompliant(false)]
        public static implicit operator Money(ushort value)
            => Current(value);

        [CLSCompliant(false)]
        public static implicit operator Money(uint value)
            => Current(value);

        public static explicit operator Money(double value)
            => Current((decimal)value);

        public static explicit operator Money(float value)
            => Current((decimal)value);

        /// <summary>
        /// Asserts the is same currency.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <exception cref="InvalidOperationException">The requested operation expected the currency {left}, but the actual value was the currency {right}!</exception>
        private static void AssertIsSameCurrency(CurrencyInfo left, CurrencyInfo right)
        {
            if (left != right)
            {
                throw new InvalidOperationException($"The requested operation expected the currency {left}, but the actual value was the currency {right}!");
            }
        }

        private static IFormatProvider GetFormatProvider(CurrencyInfo currency, IFormatProvider provider, bool useCode = false)
        {
            NumberFormatInfo numberFormatInfo = null;
            if (provider != null)
            {
                if (provider is CultureInfo culture)
                {
                    numberFormatInfo = culture.NumberFormat.Clone() as NumberFormatInfo;
                }

                if (provider is NumberFormatInfo format)
                {
                    numberFormatInfo = format.Clone() as NumberFormatInfo;
                }
            }
            else
            {
                numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat.Clone() as NumberFormatInfo;
            }

            if (numberFormatInfo == null)
            {
                return null;
            }

            numberFormatInfo.CurrencyDecimalDigits = (int)currency.DecimalDigits;
            numberFormatInfo.CurrencySymbol = currency.Symbol;

            if (useCode)
            {
                numberFormatInfo.CurrencySymbol = currency.Code;
                if (numberFormatInfo.CurrencyPositivePattern == 0)
                {
                    numberFormatInfo.CurrencyPositivePattern = 2;
                }

                if (numberFormatInfo.CurrencyPositivePattern == 1)
                {
                    numberFormatInfo.CurrencyPositivePattern = 3;
                }

                switch (numberFormatInfo.CurrencyNegativePattern)
                {
                    case 0: numberFormatInfo.CurrencyNegativePattern = 14; break;
                    case 1: numberFormatInfo.CurrencyNegativePattern = 9; break;
                    case 2: numberFormatInfo.CurrencyNegativePattern = 12; break;
                    case 3: numberFormatInfo.CurrencyNegativePattern = 11; break;
                    case 4: numberFormatInfo.CurrencyNegativePattern = 15; break;
                    case 5: numberFormatInfo.CurrencyNegativePattern = 8; break;
                    case 6: numberFormatInfo.CurrencyNegativePattern = 13; break;
                    case 7: numberFormatInfo.CurrencyNegativePattern = 10; break;
                }
            }

            return numberFormatInfo;
        }
    }
}