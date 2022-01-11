// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System;

namespace XstReader.ElementProperties
{
    /// <summary>
    /// Attribute to specify the PropertySet of an element
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PropertySetAttribute : Attribute
    {
        /// <summary>
        /// The PropertySet
        /// </summary>
        public PropertySet? PropertySet { get; private set; } = null;

        /// <summary>
        /// Ctor
        /// </summary>
        public PropertySetAttribute()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="propertySet"></param>
        public PropertySetAttribute(PropertySet propertySet)
        {
            PropertySet = propertySet;
        }
    }
}
