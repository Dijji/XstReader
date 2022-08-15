// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System.Globalization;

namespace XstReader.App.Common
{
    public static class StringExtensions
    {

        public static bool ContainsIgnoringSymbols(this string source, string value)
            => CultureInfo.InvariantCulture.CompareInfo.IndexOf(source, value,
                CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreNonSpace) != -1;
    }
}
