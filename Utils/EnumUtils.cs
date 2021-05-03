using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class EnumUtils
    {
        public static List<TEnum> GetEnumList<TEnum>() where TEnum : Enum 
            => ((TEnum[])Enum.GetValues(typeof(TEnum))).ToList();
    }
}