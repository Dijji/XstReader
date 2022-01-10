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
using System.Collections.Generic;

namespace XstReader.ItemProperties
{
    /// <summary>
    /// Attribute to define Alternate Names
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AlternateNamesAttribute : Attribute
    {
        /// <summary>
        /// The Alternate Names
        /// </summary>
        public IEnumerable<string> AlternateNames { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="names"></param>
        public AlternateNamesAttribute(params string[] names)
        {
            AlternateNames = names;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AlternateNamesAttribute()
        {
            AlternateNames = new List<string>();
        }
    }
}
