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
using System.Text;

namespace XstReader
{
    /// <summary>
    /// Extensions for IEnumerable[XstAttachment]
    /// </summary>
    public static class IEnumerableXstAttachmentExtensions
    {
        /// <summary>
        /// Save a collection of Attachments to the specified folder
        /// </summary>
        /// <param name="attachments"></param>
        /// <param name="fullFolderName"></param>
        /// <param name="creationTime"></param>
        public static void SaveToFolder(this IEnumerable<XstAttachment> attachments, string fullFolderName, DateTime? creationTime)
        {
            foreach (var attachment in attachments)
                attachment.SaveToFolder(fullFolderName, creationTime);
        }

    }
}
