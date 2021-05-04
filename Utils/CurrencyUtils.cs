using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Utils
{
    public class CurrencyUtils
    {
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

        public static string Format(decimal value, Currency currency)
        {
            var valueStr = String.Format("{0:0.00}", Math.Abs(value));
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
    }
}