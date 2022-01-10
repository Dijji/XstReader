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
using System.ComponentModel;
using XstReader.ItemProperties;

namespace XstReader
{
    /// <summary>
    /// Extensions for PropertyCanonicalName Enum
    /// </summary>
    public static class PropertyCanonicalNameExtensions
    {
        /// <summary>
        /// Returns the Id in Hexa representation
        /// </summary>
        /// <param name="propertyCanonicalName"></param>
        /// <returns></returns>
        public static string Id0x(this PropertyCanonicalName propertyCanonicalName)
            => String.Format("0x{0:x4}", (UInt16)propertyCanonicalName);

        /// <summary>
        /// Returns the FriendlyName of the PropertyCanonicalName
        /// </summary>
        /// <param name="propertyCanonicalName"></param>
        /// <returns></returns>
        public static string FriendlyName(this PropertyCanonicalName propertyCanonicalName)
            => propertyCanonicalName.GetAttribute<FriendlyNameAttribute>()?.FriendlyName ??
               $"Unknown ({propertyCanonicalName.Id0x()})";

        /// <summary>
        /// Returns the CanonicalName of the PropertyCanonicalName
        /// </summary>
        /// <param name="propertyCanonicalName"></param>
        /// <returns></returns>
        public static string CanonicalName(this PropertyCanonicalName propertyCanonicalName)
        {
            string canonicalName = propertyCanonicalName.ToString();
            if (Char.IsNumber(canonicalName[0]))
                return "";
            return canonicalName;
        }

        /// <summary>
        /// Returns the PropertyArea (if any) of the PropertyCanonicalName
        /// </summary>
        /// <param name="propertyCanonicalName"></param>
        /// <returns></returns>
        public static PropertyArea? PropertyArea(this PropertyCanonicalName propertyCanonicalName)
            => propertyCanonicalName.GetAttribute<PropertyAreaAttribute>()?.PropertyArea;

        /// <summary>
        /// Returns the PropertySet (if any) of the PropertyCanonicalName
        /// </summary>
        /// <param name="propertyCanonicalName"></param>
        /// <returns></returns>
        public static PropertySet? PropertySet(this PropertyCanonicalName propertyCanonicalName)
            => propertyCanonicalName.GetAttribute<PropertySetAttribute>()?.PropertySet;

        /// <summary>
        /// Returns the AlternateNames of the PropertyCanonicalName
        /// </summary>
        /// <param name="propertyCanonicalName"></param>
        /// <returns></returns>
        public static IEnumerable<string> AlternateNames(this PropertyCanonicalName propertyCanonicalName)
            => propertyCanonicalName.GetAttribute<AlternateNamesAttribute>()?.AlternateNames;

        /// <summary>
        /// Returns the Description of the PropertyCanonicalName
        /// </summary>
        /// <param name="propertyCanonicalName"></param>
        /// <returns></returns>
        public static string Description(this PropertyCanonicalName propertyCanonicalName)
            => propertyCanonicalName.GetAttribute<DescriptionAttribute>()?.Description;

        /// <summary>
        /// Return the Html Description of the PropertyCanonicalName
        /// </summary>
        /// <param name="propertyCanonicalName"></param>
        /// <returns></returns>
        public static string HtmlDescription(this PropertyCanonicalName propertyCanonicalName)
            => propertyCanonicalName.GetAttribute<HtmlDescriptionAttribute>()?.HtmlDescription;
    }
}
