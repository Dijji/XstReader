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
    /// Values of the PidTagAttachMethod property
    /// Enum names are taken from <MS-PST> 
    /// </summary>
    internal enum AttachMethod : Int32
    {
        afByValue = 0x00000001,
        afEmbeddedMessage = 0x00000005,
        afStorage = 0x00000006,
    }
}
