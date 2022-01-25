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
    // Enums and classes used in property handling
    // Enum names are taken from <MS-PST>

    // Values of the PidTagAttachFlags property
    internal enum AttachFlags : UInt32
    {
        attInvisibleInHtml = 0x00000001,
        attRenderedInBody = 0x00000004,
    }


}
