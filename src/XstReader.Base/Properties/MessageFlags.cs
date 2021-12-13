// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;

namespace XstReader.Properties
{
    // Enums and classes used in property handling
    // Enum names are taken from <MS-PST>

    // Values of the PidTagMessageFlags property
    [Flags]
    internal enum MessageFlags : Int32
    {
        mfHasAttach = 0x00000010,
    }
}
