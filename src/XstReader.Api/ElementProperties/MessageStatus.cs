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
    /// Values of the PidTagMessageStatus property
    /// Enum names are taken from [MS-PST] <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxcmsg/63c86b71-9ba6-4560-bd37-c17dca93b5d8"/>
    /// </summary>
    [Flags]
    public enum MessageStatus : Int32
    {
        /// <summary>
        /// The message has been marked for downloading from the remote message store to the local client.
        /// </summary>
        msRemoteDownload = 0x00001000,

        /// <summary>
        /// This is a conflict resolve message as specified in [MS-OXCFXICS] section 3.1.5.6.2.1.
        /// This is a read-only value for the client.
        /// </summary>
        msInConflict = 0x00000800,

        /// <summary>
        /// The message has been marked for deletion at the remote message store without downloading to the local client.
        /// </summary>
        msRemoteDelete = 0x00002000
    }
}
