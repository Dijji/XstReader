// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.

using System;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// Extensions for PropertySet enum
    /// </summary>
    public static class PropertySetExtensions
    {
        /// <summary>
        /// Returns the Guid (if any) of the PropertySet
        /// </summary>
        /// <param name="propertySet"></param>
        /// <returns></returns>
        public static Guid? Guid(this PropertySet propertySet)
            => propertySet.GetAttribute<GuidAttribute>()?.Guid;
    }
}
