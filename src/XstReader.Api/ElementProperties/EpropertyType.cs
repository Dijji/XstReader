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

namespace XstReader.ElementProperties
{
    /// <summary>
    /// Type of a Property
    /// Enum names are taken from [MS-PST]
    /// </summary>
    public enum EpropertyType : UInt16
    {
        /// <summary>
        /// Binary
        /// </summary>
        PtypBinary = 0x0102,

        /// <summary>
        /// Boolean
        /// </summary>
        PtypBoolean = 0x000b,

        /// <summary>
        /// Float
        /// </summary>
        PtypFloating64 = 0x0005,

        /// <summary>
        /// Guid
        /// </summary>
        PtypGuid = 0x0048,

        /// <summary>
        /// Int16
        /// </summary>
        PtypInteger16 = 0x0002,

        /// <summary>
        /// Int32
        /// </summary>
        PtypInteger32 = 0x0003,

        /// <summary>
        /// Int64
        /// </summary>
        PtypInteger64 = 0x0014,

        /// <summary>
        /// Multiple Int32
        /// </summary>
        PtypMultipleInteger32 = 0x1003,

        /// <summary>
        /// Object
        /// </summary>
        PtypObject = 0x000d,

        /// <summary>
        /// String
        /// </summary>
        PtypString = 0x001f,

        /// <summary>
        /// String8
        /// </summary>
        PtypString8 = 0x001e,

        /// <summary>
        /// Multiple String
        /// </summary>
        PtypMultipleString = 0x101F,

        /// <summary>
        /// Binary
        /// </summary>
        PtypMultipleBinary = 0x1102,

        /// <summary>
        /// DateTime
        /// </summary>
        PtypTime = 0x0040,
    }
}
