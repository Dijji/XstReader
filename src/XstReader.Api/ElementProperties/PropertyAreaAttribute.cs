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

namespace XstReader.ElementProperties
{
    /// <summary>
    /// Attribute to specify the PropertyArea related
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PropertyAreaAttribute : Attribute
    {
        /// <summary>
        /// The PropertyArea
        /// </summary>
        public PropertyArea? PropertyArea { get; private set; } = null;

        /// <summary>
        /// Ctor
        /// </summary>
        public PropertyAreaAttribute()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="propertyArea"></param>
        public PropertyAreaAttribute(PropertyArea propertyArea)
        {
            PropertyArea = propertyArea;
        }
    }
}
