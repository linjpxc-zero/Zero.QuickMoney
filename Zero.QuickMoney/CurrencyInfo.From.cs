using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Zero.QuickMoney
{
    public readonly partial struct CurrencyInfo
    {
        private const int CodeLength = 3;

        /// <summary>
        /// Convert the three-letter ISO-4217 currency code to the equivalent <see cref="CurrencyInfo"/>, a return value indicating whether it was successful.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryFromCode(string code, out CurrencyInfo result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }
            code = code.Trim();
            if (code.Length != CodeLength)
            {
                return false;
            }

            result = currencies.SingleOrDefault(item => string.Equals(item.Code, code, StringComparison.OrdinalIgnoreCase));
            return !string.IsNullOrWhiteSpace(result.Code);
        }

        /// <summary>
        /// Convert the three-digit ISO-4217 currency code to the equivalent <see cref="CurrencyInfo"/>, a return value indicating whether it was successful.
        /// </summary>
        /// <param name="numeric">The numeric.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryFromNumeric(string numeric, out CurrencyInfo result)
        {
            result = default;
            if (string.IsNullOrWhiteSpace(numeric))
            {
                return false;
            }
            numeric = numeric.Trim();
            if (numeric.Length != CodeLength)
            {
                return false;
            }
            result = currencies.SingleOrDefault(item => string.Equals(item.Numeric, numeric, StringComparison.OrdinalIgnoreCase));
            return !string.IsNullOrWhiteSpace(result.Code);
        }

        /// <summary>
        /// Obtain relevant <currency> information from <see cref="RegionInfo"/>, a return value indicating whether it was successful.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static bool TryFromRegion(RegionInfo region, out CurrencyInfo result)
        {
            result = default;
            if (region == null)
            {
                return false;
            }
            return TryFromCode(region.ISOCurrencySymbol, out result);
        }

        /// <summary>
        /// Get relevant <see cref="CurrencyInfo"/> information from the specified <see cref="CultureInfo"/>, a return value indicating whether it was successful.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<挂起>")]
        public static bool TryFromCulture(CultureInfo culture, out CurrencyInfo result)
        {
            result = default;
            if (culture == null)
            {
                return false;
            }
            if (culture.IsNeutralCulture)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(culture.Name))
            {
                return false;
            }
            try
            {
                return TryFromRegion(new RegionInfo(culture.Name), out result);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Convert the three-letter ISO-4217 currency code to the equivalent <see cref="CurrencyInfo"/>.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The <{code}> is not a valid currency code. - code</exception>
        public static CurrencyInfo FromCode(string code)
        {
            if (TryFromCode(code, out var result))
            {
                return result;
            }
            throw new ArgumentException($"The <{code}> is not a valid currency code.", nameof(code));
        }

        /// <summary>
        /// Convert the three-digit ISO-4217 currency code to the equivalent <see cref="CurrencyInfo"/>.
        /// </summary>
        /// <param name="numeric">The numeric.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The <{numeric}> is not a valid currency numeric. - Code</exception>
        public static CurrencyInfo FromNumeric(string numeric)
        {
            if (TryFromNumeric(numeric, out var result))
            {
                return result;
            }
            throw new ArgumentException($"The <{numeric}> is not a valid currency numeric.", nameof(Code));
        }

        /// <summary>
        /// Obtain relevant <currency> information from <see cref="RegionInfo"/>.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">region</exception>
        /// <exception cref="Zero.QuickMoney.CurrencyException">Could not find currency information for the specified region.</exception>
        public static CurrencyInfo FromRegion(RegionInfo region)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            if (TryFromRegion(region, out var result))
            {
                return result;
            }
            throw new CurrencyException("Could not find currency information for the specified region.");
        }

        /// <summary>
        /// Get relevant <see cref="CurrencyInfo"/> information from the specified <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">culture</exception>
        /// <exception cref="ArgumentException">
        /// Culture {culture.Name} is a neutral culture.
        /// or
        /// Culture name is empty.
        /// </exception>
        /// <exception cref="Zero.QuickMoney.CurrencyException">Could not find currency information for the specified culture.</exception>
        public static CurrencyInfo FromCulture(CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }
            if (culture.IsNeutralCulture)
            {
                throw new ArgumentException($"Culture {culture.Name} is a neutral culture.");
            }
            if (string.IsNullOrWhiteSpace(culture.Name))
            {
                throw new ArgumentException("Culture name is empty.");
            }
            if (TryFromRegion(new RegionInfo(culture.Name), out var result))
            {
                return result;
            }
            throw new CurrencyException("Could not find currency information for the specified culture.");
        }

        /// <summary>
        /// Gets the currencies.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CurrencyInfo> GetCurrencies()
            => currencies.AsEnumerable();

        private static readonly HashSet<CurrencyInfo> currencies = new HashSet<CurrencyInfo>
         {
             new CurrencyInfo("AED", "784", 2, "UAE Dirham", "د.إ"),
             new CurrencyInfo("AFN", "971", 2, "Afghani", "؋"),
             new CurrencyInfo("ALL", "008", 2, "Lek", "L"),
             new CurrencyInfo("AMD", "051", 2, "Armenian Dram", "֏"),
             new CurrencyInfo("ANG", "532", 2, "Netherlands Antillean Guilder", "ƒ"),
             new CurrencyInfo("AOA", "973", 2, "Kwanza", "Kz"),
             new CurrencyInfo("ARS", "032", 2, "Argentine Peso", "$"),
             new CurrencyInfo("AUD", "036", 2, "Australian Dollar", "$"),
             new CurrencyInfo("AWG", "533", 2, "Aruban Florin", "ƒ"),
             new CurrencyInfo("AZN", "944", 2, "Azerbaijan Manat", "ман"),
             new CurrencyInfo("BAM", "977", 2, "Convertible Mark", "KM"),
             new CurrencyInfo("BBD", "052", 2, "Barbados Dollar", "$"),
             new CurrencyInfo("BDT", "050", 2, "Taka", "৳"),
             new CurrencyInfo("BGN", "975", 2,"Bulgarian Lev", "лв."),
             new CurrencyInfo("BHD", "048", 3, "Bahraini Dinar", "BD"),
             new CurrencyInfo("BIF", "108", 0, "Burundi Franc", "FBu"),
             new CurrencyInfo("BMD", "060", 2, "Bermudian Dollar", "$"),
             new CurrencyInfo("BND", "096", 2, "Brunei Dollar", "$"),
             new CurrencyInfo("BOB", "068", 2, "Boliviano", "Bs."),
             new CurrencyInfo("BOV", "984", 2, "Mvdol", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol, true),
             new CurrencyInfo("BRL", "986", 2, "Brazilian Real", "R$"),
             new CurrencyInfo("BSD", "044", 2, "Bahamian Dollar", "$"),
             new CurrencyInfo("BTN", "064", 2, "Ngultrum", "Nu."),
             new CurrencyInfo("BWP", "072", 2, "Pula", "P"),
             new CurrencyInfo("BYN", "933", 2, "Belarusian Ruble", "Br"),
             new CurrencyInfo("BZD", "084", 2, "Belize Dollar", "BZ$"),
             new CurrencyInfo("CAD", "124", 2, "Canadian Dollar", "$"),
             new CurrencyInfo("CDF", "976", 2, "Congolese Franc", "FC"),
             new CurrencyInfo("CHE", "947", 2, "WIR Euro", "CHE", true),
             new CurrencyInfo("CHF", "756", 2, "Swiss Franc", "fr."),
             new CurrencyInfo("CHW", "948", 2, "WIR Franc", "CHW", true),
             new CurrencyInfo("CLF", "990", 4, "Unidad de Fomento","CLF", true),
             new CurrencyInfo("CLP", "152", 0, "Chilean Peso", "$"),
             new CurrencyInfo("CNY", "156", 2, "Yuan Renminbi", "¥"),
             new CurrencyInfo("COP", "170", 2, "Colombian Peso", "$"),
             new CurrencyInfo("COU", "970", 2, "Unidad de Valor Real", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol, true),
             new CurrencyInfo("CRC", "188", 2, "Costa Rican Colon", "₡"),
             new CurrencyInfo("CUC", "931", 2, "Peso Convertible", "CUC$"),
             new CurrencyInfo("CUP", "192", 2, "Cuban Peso", "$"),
             new CurrencyInfo("CVE", "132", 2, "Cabo Verde Escudo", "$"),
             new CurrencyInfo("CZK", "203", 2, "Czech Koruna", "Kč"),
             new CurrencyInfo("DJF", "262", 0, "Djibouti Franc", "Fdj"),
             new CurrencyInfo("DKK", "208", 2, "Danish Krone", "kr."),
             new CurrencyInfo("DOP", "214", 2, "Dominican Peso", "RD$"),
             new CurrencyInfo("DZD", "012", 2, "Algerian Dinar", "DA"),
             new CurrencyInfo("EGP", "818", 2, "Egyptian Pound", "LE"),
             new CurrencyInfo("ERN", "232", 2, "Nakfa", "ERN"),
             new CurrencyInfo("ETB", "230", 2, "Ethiopian Birr", "Br"),
             new CurrencyInfo("EUR", "978", 2, "Euro", "€"),
             new CurrencyInfo("FJD", "242", 2, "Fiji Dollar", "$"),
             new CurrencyInfo("FKP", "238", 2, "Falkland Islands Pound", "£"),
             new CurrencyInfo("GBP", "826", 2, "Pound Sterling", "£"),
             new CurrencyInfo("GEL", "981", 2, "Lari", "ლ."),
             new CurrencyInfo("GHS", "936", 2, "Ghana Cedi", "GH¢"),
             new CurrencyInfo("GIP", "292", 2, "Gibraltar Pound","£"),
             new CurrencyInfo("GMD", "270", 2, "Dalasi", "D"),
             new CurrencyInfo("GNF", "324", 0, "Guinean Franc", "FG"),
             new CurrencyInfo("GTQ", "320", 2, "Quetzal", "Q"),
             new CurrencyInfo("GYD", "328", 2, "Guyana Dollar", "$"),
             new CurrencyInfo("HKD", "344", 2, "Hong Kong Dollar", "HK$"),
             new CurrencyInfo("HNL", "340", 2, "Lempira", "L"),
             new CurrencyInfo("HRK", "191", 2, "Kuna", "kn"),
             new CurrencyInfo("HTG", "332", 2, "Gourde", "G"),
             new CurrencyInfo("HUF", "348", 2, "Forint", "Ft"),
             new CurrencyInfo("IDR", "360", 2, "Rupiah", "Rp"),
             new CurrencyInfo("ILS", "376", 2, "New Israeli Sheqel", "₪"),
             new CurrencyInfo("INR", "356", 2, "Indian Rupee", "₹"),
             new CurrencyInfo("IQD", "368", 3, "Iraqi Dinar", "د.ع"),
             new CurrencyInfo("IRR", "364", 2, "Iranian Rial", "ريال"),
             new CurrencyInfo("ISK", "352", 0, "Iceland Krona", "kr"),
             new CurrencyInfo("JMD", "388", 2, "Jamaican Dollar", "J$"),
             new CurrencyInfo("JOD", "400", 3, "Jordanian Dinar", /*"د.ا.\x200F"*/ "د.ا.‏"),
             new CurrencyInfo("JPY", "392", 0, "Yen", "¥"),
             new CurrencyInfo("KES", "404", 2, "Kenyan Shilling", "KSh"),
             new CurrencyInfo("KGS", "417", 2, "Som", "сом"),
             new CurrencyInfo("KHR", "116", 2, "Riel", "៛"),
             new CurrencyInfo("KMF", "174", 0, "Comorian Franc", "CF"),
             new CurrencyInfo("KPW", "408", 2, "North Korean Won", "₩"),
             new CurrencyInfo("KRW", "410", 0, "Won", "₩"),
             new CurrencyInfo("KWD", "414", 3, "Kuwaiti Dinar", "د.ك"),
             new CurrencyInfo("KYD", "136", 2, "Cayman Islands Dollar", "$"),
             new CurrencyInfo("KZT", "398", 2, "Tenge", "₸"),
             new CurrencyInfo("LAK", "418", 2, "Lao Kip", "₭"),
             new CurrencyInfo("LBP", "422", 2, "Lebanese Pound", "ل.ل"),
             new CurrencyInfo("LKR", "144", 2, "Sri Lanka Rupee", "Rs"),
             new CurrencyInfo("LRD", "430", 2, "Liberian Dollar", "$"),
             new CurrencyInfo("LSL", "426", 2, "Loti", "L"),
             new CurrencyInfo("LYD", "434", 3, "Libyan Dinar", "ل.د"),
             new CurrencyInfo("MAD", "504", 2, "Moroccan Dirham", "د.م."),
             new CurrencyInfo("MDL", "498", 2, "Moldovan Leu", "L"),
             new CurrencyInfo("MGA", "969", 2, "Malagasy Ariary", "Ar"),
             new CurrencyInfo("MKD", "807", 2, "Denar", "ден"),
             new CurrencyInfo("MMK", "104", 2, "Kyat", "K"),
             new CurrencyInfo("MNT", "496", 2, "Tugrik", "₮"),
             new CurrencyInfo("MOP", "446", 2, "Pataca", "MOP$"),
             new CurrencyInfo("MRU", "929", 2, "Ouguiya", "UM"),
             new CurrencyInfo("MUR", "480", 2, "Mauritius Rupee", "Rs"),
             new CurrencyInfo("MVR", "462", 2, "Rufiyaa", "Rf"),
             new CurrencyInfo("MWK", "454", 2, "Malawi Kwacha", "MK"),
             new CurrencyInfo("MXN", "484", 2, "Mexican Peso", "$"),
             new CurrencyInfo("MXV", "979", 2, "Mexican Unidad de Inversion (UDI)", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol, true),
             new CurrencyInfo("MYR", "458", 2, "Malaysian Ringgit", "RM"),
             new CurrencyInfo("MZN", "943", 2, "Mozambique Metical", "MTn"),
             new CurrencyInfo("NAD", "516", 2, "Namibia Dollar", "N$"),
             new CurrencyInfo("NGN", "566", 2, "Naira", "₦"),
             new CurrencyInfo("NIO", "558", 2, "Cordoba Oro", "C$"),
             new CurrencyInfo("NOK", "578", 2, "Norwegian Krone", "kr"),
             new CurrencyInfo("NPR", "524", 2, "Nepalese Rupee", "Rs"),
             new CurrencyInfo("NZD", "554", 2, "New Zealand Dollar", "$"),
             new CurrencyInfo("OMR", "512", 3, "Rial Omani", "ر.ع."),
             new CurrencyInfo("PAB", "590", 2, "Balboa", "B/."),
             new CurrencyInfo("PEN", "604", 2, "Sol", "S/."),
             new CurrencyInfo("PGK", "598", 2, "Kina", "K"),
             new CurrencyInfo("PHP", "608", 2, "Philippine Peso", "₱"),
             new CurrencyInfo("PKR", "586", 2, "Pakistan Rupee", "Rs"),
             new CurrencyInfo("PLN", "985", 2, "Zloty", "zł"),
             new CurrencyInfo("PYG", "600", 0, "Guarani", "₲"),
             new CurrencyInfo("QAR", "634", 2, "Qatari Rial", "ر.ق"),
             new CurrencyInfo("RON", "946", 2, "Romanian Leu", "lei"),
             new CurrencyInfo("RSD", "941", 2, "Serbian Dinar", "РСД"),
             new CurrencyInfo("RUB", "643", 2, "Russian Ruble", "₽"),
             new CurrencyInfo("RWF", "646", 0, "Rwanda Franc", "RFw"),
             new CurrencyInfo("SAR", "682", 2, "Saudi Riyal", "ر.س"),
             new CurrencyInfo("SBD", "090", 2, "Solomon Islands Dollar", "SI$"),
             new CurrencyInfo("SCR", "690", 2, "Seychelles Rupee", "SR"),
             new CurrencyInfo("SDG", "938", 2, "Sudanese Pound", "ج.س."),
             new CurrencyInfo("SEK", "752", 2, "Swedish Krona", "kr"),
             new CurrencyInfo("SGD", "702", 2, "Singapore Dollar", "S$"),
             new CurrencyInfo("SHP", "654", 2, "Saint Helena Pound", "£"),
             new CurrencyInfo("SLL", "694", 2, "Leone", "Le"),
             new CurrencyInfo("SOS", "706", 2, "Somali Shilling", "S"),
             new CurrencyInfo("SRD", "968", 2, "Surinam Dollar", "$"),
             new CurrencyInfo("SSP", "728", 2, "South Sudanese Pound", "£"),
             new CurrencyInfo("STN", "930", 2, "Dobra", "Db"),
             new CurrencyInfo("SVC", "222", 2, "El Salvador Colon","₡"),
             new CurrencyInfo("SYP", "760", 2,"Syrian Pound", /*"ܠ.ܣ.\x200F"*/ "ل.س.‏"),
             new CurrencyInfo("SZL", "748", 2, "Lilangeni", "L"),
             new CurrencyInfo("THB", "764", 2, "Baht", "฿"),
             new CurrencyInfo("TJS", "972", 2, "Somoni", "смн"),
             new CurrencyInfo("TMT", "934", 2, "Turkmenistan New Manat", "m"),
             new CurrencyInfo("TND", "788", 3, "Tunisian Dinar", "د.ت"),
             new CurrencyInfo("TOP", "776", 2, "Pa’anga", "T$"),
             new CurrencyInfo("TRY", "949", 2, "Turkish Lira", "₺"),
             new CurrencyInfo("TTD", "780", 2, "Trinidad and Tobago Dollar", "$"),
             new CurrencyInfo("TWD", "901", 2, "New Taiwan Dollar", "NT$"),
             new CurrencyInfo("TZS", "834", 2, "Tanzanian Shilling", "x/y"),
             new CurrencyInfo("UAH", "980", 2, "Hryvnia", "₴"),
             new CurrencyInfo("UGX", "800", 0, "Uganda Shilling", "USh"),
             new CurrencyInfo("USD", "840", 2, "US Dollar", "$"),
             new CurrencyInfo("USN", "997", 2, "US Dollar (Next day)", "$", true),
             new CurrencyInfo("UYI", "940", 0, "Uruguay Peso en Unidades Indexadas (UI)", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol, true),
             new CurrencyInfo("UYU", "858", 2, "Peso Uruguayo", "$"),
             new CurrencyInfo("UYW", "927", 4, "Unidad Previsional", "Db"),
             new CurrencyInfo("UZS", "860", 2, "Uzbekistan Sum", "лв"),
             new CurrencyInfo("VES", "928", 2, "Bolívar Soberano", "Bs."),
             new CurrencyInfo("VND", "704", 0, "Dong", "₫"),
             new CurrencyInfo("VUV", "548", 0, "Vatu", "VT"),
             new CurrencyInfo("WST", "882", 2, "Tala", "WS$"),
             new CurrencyInfo("XAF", "950", 0, "CFA Franc BEAC", "FCFA"),
             new CurrencyInfo("XAG", "961", 0, "Silver", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XAU", "959", 0, "Gold", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XBA", "955", 0, "Bond Markets Unit European Composite Unit (EURCO)", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XBB", "956", 0, "Bond Markets Unit European Monetary Unit (E.M.U.-6)", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XBC", "957", 0, "Bond Markets Unit European Unit of Account 9 (E.U.A.-9)", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XBD", "958", 0, "Bond Markets Unit European Unit of Account 17 (E.U.A.-17)", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XCD", "951", 2, "East Caribbean Dollar", "$"),
             new CurrencyInfo("XDR", "960", 0, "SDR (Special Drawing Right)", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XOF", "952", 0, "CFA Franc BCEAO", "CFA"),
             new CurrencyInfo("XPD", "964", 0, "Palladium", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XPF", "953", 0, "CFP Franc", "F"),
             new CurrencyInfo("XPT", "962", 0, "Platinum", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XSU", "994", 0, "Sucre", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XTS", "963", 0, "Codes specifically reserved for testing purposes", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XUA", "965", 0, "ADB Unit of Account", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("XXX", "999", 0, "The codes assigned for transactions where no currency is involved", CultureInfo.InvariantCulture.NumberFormat.CurrencySymbol),
             new CurrencyInfo("YER", "886", 2, "Yemeni Rial", "﷼"),
             new CurrencyInfo("ZAR", "710", 2, "Rand", "R"),
             new CurrencyInfo("ZMW", "967", 2, "Zambian Kwacha", "ZK"),
             new CurrencyInfo("ZWL", "932", 2, "Zimbabwe Dollar", "$")
         };
    }
}