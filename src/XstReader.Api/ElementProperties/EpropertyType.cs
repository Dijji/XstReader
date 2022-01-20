// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;

namespace XstReader.ElementProperties
{
    // Enums and classes used in property handling
    // Enum names are taken from <MS-PST>

    public enum EpropertyType : UInt16
    {
        PtypBinary = 0x0102,
        PtypBoolean = 0x000b,
        PtypFloating64 = 0x0005,
        PtypGuid = 0x0048,
        PtypInteger16 = 0x0002,
        PtypInteger32 = 0x0003,
        PtypInteger64 = 0x0014,
        PtypMultipleInteger32 = 0x1003,
        PtypObject = 0x000d,
        PtypString = 0x001f,
        PtypString8 = 0x001e,
        PtypMultipleString = 0x101F,
        PtypMultipleBinary = 0x1102,
        PtypTime = 0x0040,
    }
}
