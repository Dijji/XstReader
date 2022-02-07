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

namespace XstReader.ElementProperties
{
    /// <summary>
    /// Values of the PidTagImportance property
    /// Enum names are taken from [MS-PST] <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxcmsg/c9403ea3-d88b-46d8-b116-af2bd4d4ff7b"/>
    /// </summary>
    public enum MessageImportance : Int32
    {
        /// <summary>
        /// Low Importance
        /// </summary>
        LowImportance = 0x00000000,

        /// <summary>
        /// Norma Importance
        /// </summary>
        NormalImportance = 0x00000001,

        /// <summary>
        /// Hight Importance
        /// </summary>
        HightImportance = 0x00000002
    }
}
