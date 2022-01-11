// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;

namespace XstReader.ElementProperties
{
    // Enums and classes used in property handling
    // Enum names are taken from <MS-PST>

    // Values of the PidTagNativeBody property
    internal enum BodyType : Int32
    {
        Undefined = 0x00000000,
        PlainText = 0x00000001,
        RTF = 0x00000002,
        HTML = 0x00000003,
        ClearSigned = 0x00000004,
    }
}
