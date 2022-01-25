// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.

using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// Extensions for PropertyArea enum
    /// </summary>
    public static class PropertyAreaExtensions
    {
        /// <summary>
        /// Gets the FriendlyName of the PropertyArea
        /// </summary>
        /// <param name="propertyArea"></param>
        /// <returns></returns>
        public static string FriendlyName(this PropertyArea propertyArea)
            => propertyArea.GetAttribute<FriendlyNameAttribute>()?.FriendlyName;
    }
}
