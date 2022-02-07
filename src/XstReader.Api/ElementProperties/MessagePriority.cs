// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.

using System;
using System.Collections.Generic;
using System.Text;

namespace XstReader.ElementProperties
{
    /// <summary>
    /// Values of the PidTagPriority property
    /// Enum names are taken from [MS-PST] <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxcmsg/65249839-e316-4695-82ce-86b78129c764"/>
    /// </summary>
    public enum MessagePriority : UInt32
    {
        /// <summary>
        /// The message is urgent.
        /// </summary>
        Urgent = 0x00000001,

        /// <summary>
        /// The message has normal priority.
        /// </summary>
        Normal = 0x00000000,

        /// <summary>
        /// The message is not urgent.
        /// </summary>
        NotUrgent = 0xFFFFFFFF,
    }
}
