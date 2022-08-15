// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.

using System.Collections.Generic;
using System.Linq;

namespace XstReader
{
    /// <summary>
    /// Extensions for IEnumerable[XstElement]
    /// </summary>
    public static class IEnumerableXstElementExtensions
    {
        /// <summary>
        /// Save all the properties in a collection of Messages in the specified file
        /// </summary>
        /// <param name="element"></param>
        /// <param name="fileName"></param>
        public static void SavePropertiesToFile(this XstElement element, string fileName)
            => element.Properties.Items.NonBinary().SaveToFile(fileName);

        /// <summary>
        /// Save all the properties in a collection of Messages to String
        /// </summary>
        /// <param name="element"></param>
        public static string SavePropertiesToString(this XstElement element)
            => element.Properties.Items.NonBinary().SaveToString();

        /// <summary>
        /// Save all the properties in a collection of Messages in the specified file
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="fileName"></param>
        public static void SavePropertiesToFile(this IEnumerable<XstElement> elements, string fileName)
            => elements.Select(m => m.Properties.Items.NonBinary()).SaveToFile(fileName);

        /// <summary>
        /// Save all the properties in a collection of Messages to String
        /// </summary>
        /// <param name="elements"></param>
        public static string SavePropertiesToString(this IEnumerable<XstElement> elements)
            => elements.Select(m => m.Properties.Items.NonBinary()).SaveToString();

    }
}
