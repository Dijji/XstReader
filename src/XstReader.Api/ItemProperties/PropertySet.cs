// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.


// This file was generated automatically on 2022-01-10 10:28 +01:00 from the documentation at https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/

using System;

namespace XstReader.ItemProperties
{
    /// <summary>
    /// Property Sets for the properties defined in 
    /// <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/../ms-oxcfold/c0f31b95-c07f-486c-98d9-535ed9705fbf">
    /// [MS-OXCFOLD]</see>
    /// </summary>
    public enum PropertySet
    {

        /// <summary> PSETID_Address (00062004-0000-0000-C000-000000000046) </summary>
        [Guid("00062004-0000-0000-C000-000000000046")]
        PSETID_Address,

        /// <summary> PSETID_Common (00062008-0000-0000-C000-000000000046) </summary>
        [Guid("00062008-0000-0000-C000-000000000046")]
        PSETID_Common,

        /// <summary> PSETID_Appointment (00062002-0000-0000-C000-000000000046) </summary>
        [Guid("00062002-0000-0000-C000-000000000046")]
        PSETID_Appointment,

        /// <summary> PSETID_Meeting (6ED8DA90-450B-101B-98DA-00AA003F1305) </summary>
        [Guid("6ED8DA90-450B-101B-98DA-00AA003F1305")]
        PSETID_Meeting,

        /// <summary> PS_PUBLIC_STRINGS (00020329-0000-0000-C000-000000000046) </summary>
        [Guid("00020329-0000-0000-C000-000000000046")]
        PS_PUBLIC_STRINGS,

        /// <summary> PSETID_CalendarAssistant (11000E07-B51B-40D6-AF21-CAA85EDAB1D0) </summary>
        [Guid("11000E07-B51B-40D6-AF21-CAA85EDAB1D0")]
        PSETID_CalendarAssistant,

        /// <summary> PSETID_Log (0006200A-0000-0000-C000-000000000046) </summary>
        [Guid("0006200A-0000-0000-C000-000000000046")]
        PSETID_Log,

        /// <summary> PSETID_Note (0006200E-0000-0000-C000-000000000046) </summary>
        [Guid("0006200E-0000-0000-C000-000000000046")]
        PSETID_Note,

        /// <summary> PSETID_Task (00062003-0000-0000-C000-000000000046) </summary>
        [Guid("00062003-0000-0000-C000-000000000046")]
        PSETID_Task,

        /// <summary> PSETID_PostRss (00062041-0000-0000-C000-000000000046) </summary>
        [Guid("00062041-0000-0000-C000-000000000046")]
        PSETID_PostRss,

        /// <summary> PSETID_Sharing (00062040-0000-0000-C000-000000000046) </summary>
        [Guid("00062040-0000-0000-C000-000000000046")]
        PSETID_Sharing,

        /// <summary> PSTID_Sharing (00062040-0000-0000-C000-000000000046) </summary>
        [Guid("00062040-0000-0000-C000-000000000046")]
        PSTID_Sharing,

    }
}
