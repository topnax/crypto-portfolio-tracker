using System;
using System.Globalization;

namespace Utils
{
    public static class DecimalUtils
    {
        private static NumberFormatInfo whitespaceSeparatorNfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        static DecimalUtils()
        {
            whitespaceSeparatorNfi.NumberGroupSeparator = " ";
        }
        
        public static string FormatTrimZeros(decimal value) => value.ToString("#,0.#############", whitespaceSeparatorNfi);
        
        public static string FormatTwoDecimalPlaces(decimal value) => value.ToString("#,0.00", whitespaceSeparatorNfi);
        
        public static string FormatFiveDecimalPlaces(decimal value) => value.ToString("#,0.00000", whitespaceSeparatorNfi);

        public static string FormatTwoDecimalPlacesWithPlusSign(decimal value) =>
            (value > 0 ? "+" : "") + FormatTwoDecimalPlaces(value);
    }
}