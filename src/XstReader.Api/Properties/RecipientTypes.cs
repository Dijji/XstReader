// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;

namespace XstReader.Properties
{
    // Enums and classes used in property handling
    // Enum names are taken from <MS-PST>

    // Values of the PidTagRecipientType property
    [Flags]
    public enum RecipientTypes : Int32
    {
        Originator = 0x00000000,
        To = 0x00000001,
        Cc = 0x00000002,
        Bcc = 0x00000003,
    }

}
