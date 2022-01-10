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

namespace XstReader.ItemProperties
{
    /// <summary>
    /// Attribute to specify the Guid related to an element
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class GuidAttribute : Attribute
    {
        /// <summary>
        /// The Guid
        /// </summary>
        public Guid? Guid { get; private set; } = null;

        /// <summary>
        /// Ctor
        /// </summary>
        public GuidAttribute()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="guid"></param>
        public GuidAttribute(Guid guid)
        {
            Guid = guid;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="guid"></param>
        public GuidAttribute(string guid)
        {
            Guid = new Guid(guid);
        }
    }
}
