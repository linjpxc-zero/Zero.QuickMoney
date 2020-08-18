using System;
using System.Globalization;

namespace Zero.QuickMoney
{
    public readonly struct Money : IMoney, IEquatable<Money>, IComparable, IComparable<Money>
    {
        private readonly decimal amount;

        public Money(CurrencyInfo currency, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(currency.Code))
            {
                throw new ArgumentException("Is not a valid currency.", nameof(currency));
            }
            this.Currency = currency;
            this.amount = Math.Round(amount, (int)currency.DecimalDigits, MidpointRounding.AwayFromZero);
        }

        decimal IMoney.Amount => this.amount;

        ICurrency IMoney.Currency => this.Currency;

        public CurrencyInfo Currency { get; }

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

        public int CompareTo(Money other)
        {
            AssertIsSameCurrency(this.Currency, other.Currency);
            return this.amount.CompareTo(other.amount);
        }

        public override string ToString()
            => ToString(null, null);

        public string ToString(string format)
            => ToString(format, null);

        public string ToString(IFormatProvider provider)
            => ToString(null, provider);

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

        public override bool Equals(object obj)
            => obj is Money money && Equals(money);

        public bool Equals(Money other)
            => this.Currency == other.Currency
            && this.amount == other.amount;

        public override int GetHashCode()
        {
            int hashCode = -1315411705;
            hashCode = hashCode * -1521134295 + amount.GetHashCode();
            hashCode = hashCode * -1521134295 + Currency.GetHashCode();
            return hashCode;
        }

        public static Money Current(decimal amount)
            => new Money(CurrencyInfo.CurrentCurrency, amount);

        public static int Compare(Money left, Money right)
            => left.CompareTo(right);

        public static Money Add(Money left, Money right)
        {
            AssertIsSameCurrency(left.Currency, right.Currency);
            return new Money(left.Currency, decimal.Add(left.amount, right.amount));
        }

        public static Money Add(Money money, decimal value)
            => new Money(money.Currency, decimal.Add(money.amount, value));

        public static Money Subtract(Money left, Money right)
        {
            AssertIsSameCurrency(left.Currency, right.Currency);
            return new Money(left.Currency, decimal.Subtract(left.amount, right.amount));
        }

        public static Money Subtract(Money money, decimal value)
            => new Money(money.Currency, decimal.Subtract(money.amount, value));

        public static Money Subtract(decimal value, Money money)
            => new Money(money.Currency, decimal.Subtract(value, money.amount));

        public static Money Multiply(Money left, Money right)
        {
            AssertIsSameCurrency(left.Currency, right.Currency);
            return new Money(left.Currency, decimal.Multiply(left.amount, right.amount));
        }

        public static Money Multiply(Money money, decimal value)
            => new Money(money.Currency, decimal.Multiply(money.amount, value));

        public static Money Divide(Money left, Money right)
        {
            AssertIsSameCurrency(left.Currency, right.Currency);
            return new Money(left.Currency, decimal.Divide(left.amount, right.amount));
        }

        public static Money Divide(Money money, decimal value)
            => new Money(money.Currency, decimal.Divide(money.amount, value));

        public static Money Divide(decimal value, Money money)
            => new Money(money.Currency, decimal.Divide(value, money.amount));

        public static Money Negate(Money money)
            => new Money(money.Currency, decimal.Negate(money.amount));

        public static int[] GetBits(Money money)
            => decimal.GetBits(money.amount);

        public static byte ToByte(Money money)
            => decimal.ToByte(money.amount);

        [CLSCompliant(false)]
        public static sbyte ToSByte(Money money)
            => decimal.ToSByte(money.amount);

        public static short ToInt16(Money money)
            => decimal.ToInt16(money.amount);

        [CLSCompliant(false)]
        public static ushort ToUInt16(Money money)
            => decimal.ToUInt16(money.amount);

        public static int ToInt32(Money money)
            => decimal.ToInt32(money.amount);

        [CLSCompliant(false)]
        public static uint ToUInt32(Money money)
            => decimal.ToUInt32(money.amount);

        public static long ToInt64(Money money)
            => decimal.ToInt64(money.amount);

        [CLSCompliant(false)]
        public static ulong ToUInt64(Money money)
            => decimal.ToUInt64(money.amount);

        public static float ToSingle(Money money)
            => decimal.ToSingle(money.amount);

        public static double ToDouble(Money money)
            => decimal.ToDouble(money.amount);

        public static Money Parse(string value)
        {
            if (TryParse(value, out var money))
            {
                return money;
            }
            throw new MoneyFormatException("Is not a valid money format.");
        }

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
