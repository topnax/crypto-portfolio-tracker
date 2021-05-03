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
            var valueStr = String.Format("{0:.00}", value);
            switch (currency)
            {
                case Currency.Czk:
                    return $"{valueStr},- Kč";
                case Currency.Eur:
                    return $"{valueStr} €";
                case Currency.Usd:
                    return $"${valueStr}";
            }

            return "UNDEFINED";
        }
    }
}