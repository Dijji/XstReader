using System;
using System.Collections.Generic;
using System.Text;

namespace XstReader.ItemProperties
{
    internal enum NamedPropertyTag : UInt16
    {
        // Named properties
        PidTagNameidStreamGuid = 0x0002,
        PidTagNameidStreamEntry = 0x0003,
        PidTagNameidStreamString = 0x0004,
    }
}
