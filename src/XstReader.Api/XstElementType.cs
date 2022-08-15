// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.

namespace XstReader
{
    /// <summary>
    /// Type of an XstElement
    /// </summary>
    public enum XstElementType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// A Xst File
        /// </summary>
        File = 1,

        /// <summary>
        /// A Xst Folder
        /// </summary>
        Folder = 2,

        /// <summary>
        /// A Xst Message
        /// </summary>
        Message = 3,

        /// <summary>
        /// A Xst Attachment
        /// </summary>
        Attachment = 4,

        /// <summary>
        /// A Xst Recipient
        /// </summary>
        Recipient = 5,
    }
}
