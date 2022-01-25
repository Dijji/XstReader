// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.
// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;

namespace XstReader.ElementProperties
{
    /// <summary>
    /// Message Recipient Types
    /// Values of the PidTagRecipientType property
    /// Enum names are taken from <MS-PST>
    /// </summary>
    public enum RecipientType : Int32
    {
        /// <summary>
        /// Recipient Originator
        /// </summary>
        Originator = 0x00000000,

        /// <summary>
        /// Recipient in To
        /// </summary>
        To = 0x00000001,

        /// <summary>
        /// Recipient in Cc
        /// </summary>
        Cc = 0x00000002,

        /// <summary>
        /// Recipient in Bcc
        /// </summary>
        Bcc = 0x00000003,

        /// <summary>
        /// Original Recipient who "Sent Representing"
        /// This is a Custom Type (not defined in <MS-PST>, but present in Message Properties
        /// </summary>
        OriginalSentRepresenting = 0x00000011, //Custom value

        /// <summary>
        /// Recipient who "Sent Representing"
        /// This is a Custom Type (not defined in <MS-PST>, but present in Message Properties
        /// </summary>
        SentRepresenting = 0x00000012, //Custom value

        /// <summary>
        /// Recipient who "Receive Representing"
        /// This is a Custom Type (not defined in <MS-PST>, but present in Message Properties
        /// </summary>
        ReceivedRepresenting = 0x00000013, //Custom value

        /// <summary>
        /// Recipient Sender of the Message
        /// This is a Custom Type (not defined in <MS-PST>, but present in Message Properties
        /// </summary>
        Sender = 0x00000014, //Custom value

        /// <summary>
        /// Recipient who receive the Message
        /// This is a Custom Type (not defined in <MS-PST>, but present in Message Properties
        /// </summary>
        ReceivedBy = 0x00000015, //Custom value
    }

}
