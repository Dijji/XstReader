using System;
using System.Collections.Generic;
using System.Text;

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
