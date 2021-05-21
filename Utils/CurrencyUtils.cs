using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Utils
{
    public class CurrencyUtils
    {
        /// <summary>
        /// Returns a label of the given currency
        /// </summary>
        /// <param name="currency">Currency whose label should be returned</param>
        /// <returns>Label of the given currency</returns>
        public static string GetCurrencyLabel(Currency currency)
        {
            switch (currency)
            {
                case Currency.Czk:
                    return "CZK";
                case Currency.Eur:
                    return "EUR";
                case Currency.Usd:
                    return "USD";
            }

            return "UNDEFINED";
        }

        /// <summary>
        /// Format's the given value and currency due to currency's formatting rules
        /// </summary>
        /// <param name="value">Value to be formatted</param>
        /// <param name="currency">Currency to be used</param>
        /// <returns>A string containing both the value and the currency symbol in the correct format</returns>
        public static string Format(decimal value, Currency currency)
        {
            var valueStr = DecimalUtils.FormatTwoDecimalPlaces(Math.Abs(value));
            var output = "";
            switch (currency)
            {
                case Currency.Czk:
                    output =  $"{valueStr},- Kč";
                    break;
                case Currency.Eur:
                    output =  $"€{valueStr}";
                    break;
                case Currency.Usd:
                    output = $"${valueStr}";
                    break;
                default:
                    output = "UNDEFINED_CURRENCY";
                    break;
            }

            if (value < 0)
            {
                output = "-" + output;
            }

            return output;
        }

        /// <summary>
        /// Format's the given value and currency due to currency's formatting rules. Adds the plus symbol when the value
        /// is positive.
        /// </summary>
        /// <param name="value">Value to be formatted</param>
        /// <param name="currency">Currency to be used</param>
        /// <returns>A string containing both the value and the currency symbol in the correct format</returns>
        public static string FormatWithPlusSign(decimal value, Currency currency) =>
            (value > 0 ? "+" : "") + Format(value, currency);
    }
}