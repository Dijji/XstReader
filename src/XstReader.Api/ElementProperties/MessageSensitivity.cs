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
    /// Values of the PidTagSensitivity  property
    /// Enum names are taken from [MS-PST] <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxcmsg/4c5f91f9-e25c-47bd-901f-71e0610d6346"/>
    /// </summary>
    public enum MessageSensitivity : Int32
    {
        /// <summary>
        /// Normal
        /// </summary>
        Normal = 0x00000000,

        /// <summary>
        /// Personal
        /// </summary>
        Personal = 0x00000001,

        /// <summary>
        /// Private
        /// </summary>
        Private = 0x00000002,

        /// <summary>
        /// Confidential
        /// </summary>
        Confidential = 0x00000003,
    }
}
