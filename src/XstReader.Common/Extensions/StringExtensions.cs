using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstReader.App.Common
{
    public static class StringExtensions
    {

        public static bool ContainsIgnoringSymbols(this string source, string value)
            => CultureInfo.InvariantCulture.CompareInfo.IndexOf(source, value, 
                CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreNonSpace) != -1;
    }
}
