using System;
using System.Collections.Generic;
using System.Globalization;

namespace Zero.QuickMoney
{
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

        public string Organization { get; }

        public string Code { get; }

        public string Symbol { get; }

        public decimal DecimalDigits { get; }

        public string EnglishName { get; }

        public decimal MajorUnit => decimal.One;

        public decimal MinorUnit => this.DecimalDigits <= decimal.Zero ? this.MajorUnit : new decimal(1D / Math.Pow(10D, (int)this.DecimalDigits));

        public string Numeric { get; }

        public bool IsFund { get; }

        public int CompareTo(CurrencyInfo other)
            => string.Compare(this.Code, other.Code, StringComparison.OrdinalIgnoreCase);

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

        public bool Equals(ICurrency other)
        {
            if (other == null)
            {
                return false;
            }
            return string.Equals(this.Organization, other.Organization, StringComparison.OrdinalIgnoreCase)
                && string.Equals(this.Code, other.Code, StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(CurrencyInfo other)
            => string.Equals(this.Code, other.Code, StringComparison.OrdinalIgnoreCase);

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

        public override int GetHashCode()
        {
            int hashCode = 827287470;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Organization);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
            return hashCode;
        }

        public override string ToString()
            => this.Code;

        public static CurrencyInfo CurrentCurrency => FromRegion(RegionInfo.CurrentRegion);

        public static int Compare(CurrencyInfo left, CurrencyInfo right)
            => left.CompareTo(right);

        public static int CompareNumeric(CurrencyInfo left, CurrencyInfo right)
            => string.Compare(left.Numeric, right.Numeric, StringComparison.OrdinalIgnoreCase);

        public static bool operator ==(CurrencyInfo left, CurrencyInfo right)
            => left.Equals(right);

        public static bool operator !=(CurrencyInfo left, CurrencyInfo right)
            => !(left == right);
    }
}
