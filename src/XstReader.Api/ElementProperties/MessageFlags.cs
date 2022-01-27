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
    /// Values of the PidTagMessageFlags property
    /// Enum names are taken from [MS-PST] <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxcmsg/a0c52fe2-3014-43a7-942d-f43f6f91c366"/>
    /// </summary>
    [Flags]
    public enum MessageFlags : Int32
    {
        /// <summary>
        /// The message is marked as having been read.
        /// </summary>
        mfRead = 0x00000001,

        /// <summary>
        /// The message is still being composed and is treated as a Draft Message object. 
        /// This bit is cleared by the server when responding to the RopSubmitMessage ROP ([MS-OXCROPS] section 2.2.7.1) with a success code.
        /// </summary>
        mfUnsent = 0x00000008,

        /// <summary>
        /// The message includes a request for a resend operation with a non-delivery report. 
        /// For more details, see [MS-OXOMSG] section 3.2.4.5.
        /// </summary>
        mfResend = 0x00000080,

        /// <summary>
        /// The message has not been modified since it was first saved (if unsent) or it was delivered (if sent).
        /// </summary>
        mfUnmodified = 0x00000002,

        /// <summary>
        /// The message is marked for sending as a result of a call to the RopSubmitMessage ROP
        /// </summary>
        mfSubmitted = 0x00000004,

        /// <summary>
        /// The message has at least one attachment. 
        /// This flag corresponds to the message's PidTagHasAttachments property (section 2.2.1.2).
        /// </summary>
        mfHasAttach = 0x00000010,

        /// <summary>
        /// The user receiving the message was also the user who sent the message.
        /// </summary>
        mfFromMe = 0x00000020,

        /// <summary>
        /// The message is an FAI message.
        /// </summary>
        mfFAI = 0x00000040,

        /// <summary>
        /// The user who sent the message has requested notification when a recipient (1) first reads it.
        /// </summary>
        mfNotifyRead = 0x00000100,

        /// <summary>
        /// The user who sent the message has requested notification when a recipient (1) deletes it before reading or the Message object expires.
        /// </summary>
        mfNotifyUnread = 0x00000200,

        /// <summary>
        /// The message has been read at least once. 
        /// This flag is set or cleared by the server whenever the mfRead flag is set or cleared. Clients SHOULD ignore this flag.
        /// </summary>
        mfEverRead = 0x00000400,

        /// <summary>
        /// The incoming message arrived over the Internet and originated either outside the organization or from a source the gateway does not consider trusted.
        /// </summary>
        mfInternet = 0x00002000,

        /// <summary>
        /// The incoming message arrived over an external link other than X.400 or the Internet. 
        /// It originated either outside the organization or from a source the gateway does not consider trusted.
        /// </summary>
        mfUntrusted = 0x00008000,
    }
}
