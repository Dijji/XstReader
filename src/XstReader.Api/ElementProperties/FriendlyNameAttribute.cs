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
    /// Attribute to specify the FriendlyName of an element
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]

    public class FriendlyNameAttribute : Attribute
    {
        /// <summary>
        /// The Friendly Name
        /// </summary>
        public string FriendlyName { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="friendlyName"></param>
        public FriendlyNameAttribute(string friendlyName)
        {
            FriendlyName = friendlyName;
        }
    }
}
