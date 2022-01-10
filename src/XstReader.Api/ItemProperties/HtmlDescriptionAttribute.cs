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
    /// Attribute to specify an HTML Description
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class HtmlDescriptionAttribute : Attribute
    {
        /// <summary>
        /// The Description in Html format
        /// </summary>
        public string HtmlDescription { get; private set; }
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="htmlDescription"></param>
        public HtmlDescriptionAttribute(string htmlDescription)
        {
            HtmlDescription = htmlDescription;
        }
    }
}
