using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class EnumUtils
    {
        /// <summary>
        /// Returns all possible values of the given enumerable
        /// </summary>
        /// <typeparam name="TEnum">Enumerable whose values are to be returned</typeparam>
        /// <returns>List of possible values of the given enumerable</returns>
        public static List<TEnum> GetEnumList<TEnum>() where TEnum : Enum 
            => ((TEnum[])Enum.GetValues(typeof(TEnum))).ToList();
    }
}