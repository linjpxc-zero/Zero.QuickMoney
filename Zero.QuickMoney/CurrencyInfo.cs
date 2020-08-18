using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Zero.QuickMoney
{
    /// <summary>
    /// Implementation of ISO-4217 Currency Representation.
    /// </summary>
    /// <seealso cref="Zero.QuickMoney.ICurrency" />
    /// <seealso cref="System.IEquatable{Zero.QuickMoney.CurrencyInfo}" />
    /// <seealso cref="System.IComparable" />
    /// <seealso cref="System.IComparable{Zero.QuickMoney.CurrencyInfo}" />
    [SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "<挂起>")]
    [DebuggerDisplay(nameof(Code))]
    public readonly partial struct CurrencyInfo : ICurrency, IEquatable<CurrencyInfo>, IComparable, IComparable<CurrencyInfo>
    {
        private const string ISO_4217 = "ISO-4217";

        internal CurrencyInfo(string code = default, string numeric = default, int decimalDigits = default, string englishName = default, string symbol = default, bool isFund = false)
        {
            this.Organization = ISO_4217;
            this.Code = code;
            this.Numeric = numeric;
            this.DecimalDigits = decimalDigits;
            this.EnglishName = englishName;
            this.Symbol = symbol;
            this.IsFund = isFund;
        }

        /// <summary>
        /// Get a value representing the organization of the curreny, link ISO-4217.
        /// </summary>
        /// <value>
        /// The organization.
        /// </value>
        public string Organization { get; }

        /// <summary>
        /// Get a value representing the currency code.
        /// </summary>
        /// <value>
        /// The currency code.
        /// </value>
        public string Code { get; }

        /// <summary>
        /// Get a value representing the symbol of the currency.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol { get; }

        /// <summary>
        /// Gets a value that represents the number of decimal places used in currency values.
        /// </summary>
        public decimal DecimalDigits { get; }

        /// <summary>
        /// Get a value representing the English name of the currency.
        /// </summary>
        /// <value>
        /// The name of the english.
        /// </value>
        public string EnglishName { get; }

        /// <summary>
        /// Get a value representing the major unit of currency.
        /// </summary>
        /// <value>
        /// The major unit.
        /// </value>
        public decimal MajorUnit => decimal.One;

        /// <summary>
        /// Get a value representing the minor unit of currency.
        /// </summary>
        /// <value>
        /// The minor unit.
        /// </value>
        public decimal MinorUnit => this.DecimalDigits <= decimal.Zero ? this.MajorUnit : new decimal(1D / Math.Pow(10D, (int)this.DecimalDigits));

        /// <summary>
        /// Get a value that represents the three-digit representation of currency.
        /// </summary>
        /// <value>
        /// The numeric.
        /// </value>
        public string Numeric { get; }

        /// <summary>
        /// Get a value indicating whether the currency is the fund currency.
        /// </summary>
        public bool IsFund { get; }

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
        public int CompareTo(CurrencyInfo other)
            => string.Compare(this.Code, other.Code, StringComparison.OrdinalIgnoreCase);

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
            if (obj is CurrencyInfo currency)
            {
                return this.CompareTo(currency);
            }
            throw new ArgumentException($"The obj is not {nameof(CurrencyInfo)}", nameof(obj));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(ICurrency other)
        {
            if (other == null)
            {
                return false;
            }
            return string.Equals(this.Organization, other.Organization, StringComparison.OrdinalIgnoreCase)
                && string.Equals(this.Code, other.Code, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(CurrencyInfo other)
            => string.Equals(this.Code, other.Code, StringComparison.OrdinalIgnoreCase);

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
            if (obj is CurrencyInfo currencyInfo)
            {
                return this.Equals(currencyInfo);
            }
            if (obj is ICurrency currency)
            {
                return this.Equals(currency);
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
            int hashCode = 827287470;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Organization);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
            return hashCode;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
            => this.Code;

        /// <summary>
        /// Gets the <see cref="CurrencyInfo"/> that represents the currency used by the current thread.
        /// </summary>
        /// <value>
        /// The current currency.
        /// </value>
        public static CurrencyInfo CurrentCurrency => FromRegion(RegionInfo.CurrentRegion);

        /// <summary>
        /// Compares two instance objects and returns an integer indicating whether the position of the <paramref name="left"/> instance in the sort order is before, after, or the same as the position of the <paramref name="right"/> object.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object.</param>
        /// <returns>A value indicating the relative order of the objects to be compared. The meaning of the return value is as follows: Value meaning is less than zero <paramref name="left"/> is before <paramref name="right"/> in the sort order. The position of zero <paramref name="left"/> in the sort order is the same as <paramref name="right"/>. Greater than zero <paramref name="left"/> is after <paramref name="right"/> in the sort order.</returns>
        public static int Compare(CurrencyInfo left, CurrencyInfo right)
            => left.CompareTo(right);

        /// <summary>
        /// Compare the properties of the two instance objects <see cref="Numeric"/> and return an integer indicating whether the position of the <paramref name="left"/> instance in the sort order is before, after, or the same as the position of the <paramref name="right"/> object.
        /// </summary>
        /// <param name="left">The left object.</param>
        /// <param name="right">The right object.</param>
        /// <returns>A value indicating the relative order of the objects to be compared. The meaning of the return value is as follows: Value meaning is less than zero <paramref name="left"/> is before <paramref name="right"/> in the sort order. The position of zero <paramref name="left"/> in the sort order is the same as <paramref name="right"/>. Greater than zero <paramref name="left"/> is after <paramref name="right"/> in the sort order.</returns>
        public static int CompareNumeric(CurrencyInfo left, CurrencyInfo right)
            => string.Compare(left.Numeric, right.Numeric, StringComparison.OrdinalIgnoreCase);

        public static bool operator ==(CurrencyInfo left, CurrencyInfo right)
            => left.Equals(right);

        public static bool operator !=(CurrencyInfo left, CurrencyInfo right)
            => !(left == right);
    }
}