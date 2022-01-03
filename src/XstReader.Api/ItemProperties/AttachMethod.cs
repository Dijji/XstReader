// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;

namespace XstReader.ItemProperties
{
    // Enums and classes used in property handling
    // Enum names are taken from <MS-PST>

    // Values of the PidTagAttachMethod property
    internal enum AttachMethod : Int32
    {
        afByValue = 0x00000001,
        afEmbeddedMessage = 0x00000005,
        afStorage = 0x00000006,
    }
}
