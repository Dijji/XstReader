// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Text;

namespace XstReader
{
    /// <summary>
    /// The types of an Attachment
    /// </summary>
    public enum XstAttachmentType
    {
        /// <summary>
        /// The Attachment is a File
        /// </summary>
        File,

        /// <summary>
        /// The Attachment is an Email
        /// </summary>
        Email,

        /// <summary>
        /// The Attachemnt is Other thing (unknown)
        /// </summary>
        Other
    }
}
