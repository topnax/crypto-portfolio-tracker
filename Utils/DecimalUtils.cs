using System;
using System.Globalization;

namespace Utils
{
    public static class DecimalUtils
    {
        // define white space separator number format, it is to be used in all format methods on this class
        private static readonly NumberFormatInfo WhitespaceSeparatorNf = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        
        static DecimalUtils()
        {
            // make the number format use whitespace as thousands separator
            WhitespaceSeparatorNf.NumberGroupSeparator = " ";
        }
        
        public static string FormatTrimZeros(decimal value) => value.ToString("#,0.#############", WhitespaceSeparatorNf);
        
        public static string FormatTwoDecimalPlaces(decimal value) => value.ToString("#,0.00", WhitespaceSeparatorNf);
        
        public static string FormatFiveDecimalPlaces(decimal value) => value.ToString("#,0.00000", WhitespaceSeparatorNf);

        public static string FormatTwoDecimalPlacesWithPlusSign(decimal value) =>
            (value > 0 ? "+" : "") + FormatTwoDecimalPlaces(value);
    }
}