using System;

namespace Utils
{
    public static class DecimalUtils
    {
        public static string FormatTwoDecimalPlaces(decimal value) => String.Format("{0:0.00}", Math.Abs(value));
    }
}