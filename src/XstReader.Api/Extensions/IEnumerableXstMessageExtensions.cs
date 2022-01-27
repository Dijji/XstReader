// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System.Collections.Generic;
using System.Linq;

namespace XstReader
{
    /// <summary>
    /// Extensions for IEnumerable[XstMessage]
    /// </summary>
    public static class IEnumerableXstMessageExtensions
    {
        /// <summary>
        /// Save all the properties in a collection of Messages in the specified file
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="fileName"></param>
        public static void SavePropertiesToFile(this IEnumerable<XstMessage> messages, string fileName)
            => messages.Select(m => m.Properties.ItemsNonBinary).SaveToFile(fileName);
    }
}
