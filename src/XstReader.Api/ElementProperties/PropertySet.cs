// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.


// This file was generated automatically on 2022-01-10 10:28 +01:00 from the documentation at https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/

using System;
using System.ComponentModel;

namespace XstReader.ElementProperties
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
        [Description("Address")]
        PSETID_Address,

        /// <summary> PSETID_Common (00062008-0000-0000-C000-000000000046) </summary>
        [Guid("00062008-0000-0000-C000-000000000046")]
        [Description("Common")]
        PSETID_Common,

        /// <summary> PSETID_Appointment (00062002-0000-0000-C000-000000000046) </summary>
        [Guid("00062002-0000-0000-C000-000000000046")]
        [Description("Appointment")]
        PSETID_Appointment,

        /// <summary> PSETID_Meeting (6ED8DA90-450B-101B-98DA-00AA003F1305) </summary>
        [Guid("6ED8DA90-450B-101B-98DA-00AA003F1305")]
        [Description("Meeting")]
        PSETID_Meeting,

        /// <summary> PS_PUBLIC_STRINGS (00020329-0000-0000-C000-000000000046) </summary>
        [Guid("00020329-0000-0000-C000-000000000046")]
        [Description("Public Strings")]
        PS_PUBLIC_STRINGS,

        /// <summary> PSETID_CalendarAssistant (11000E07-B51B-40D6-AF21-CAA85EDAB1D0) </summary>
        [Guid("11000E07-B51B-40D6-AF21-CAA85EDAB1D0")]
        [Description("Calendar Assistant")]
        PSETID_CalendarAssistant,

        /// <summary> PSETID_Log (0006200A-0000-0000-C000-000000000046) </summary>
        [Guid("0006200A-0000-0000-C000-000000000046")]
        [Description("Log")]
        PSETID_Log,

        /// <summary> PSETID_Note (0006200E-0000-0000-C000-000000000046) </summary>
        [Guid("0006200E-0000-0000-C000-000000000046")]
        [Description("Note")]
        PSETID_Note,

        /// <summary> PSETID_Task (00062003-0000-0000-C000-000000000046) </summary>
        [Guid("00062003-0000-0000-C000-000000000046")]
        [Description("Task")]
        PSETID_Task,

        /// <summary> PSETID_PostRss (00062041-0000-0000-C000-000000000046) </summary>
        [Guid("00062041-0000-0000-C000-000000000046")]
        [Description("Post Rss")]
        PSETID_PostRss,

        /// <summary> PSETID_Sharing (00062040-0000-0000-C000-000000000046) </summary>
        [Guid("00062040-0000-0000-C000-000000000046")]
        [Description("Sharing")]
        PSETID_Sharing,

        /// <summary> PSTID_Sharing (00062040-0000-0000-C000-000000000046) </summary>
        [Guid("00062040-0000-0000-C000-000000000046")]
        [Description("Pstid Sharing")]
        PSTID_Sharing,


        // ---------------
        // Added manually:
        // 

        /// <summary> MAPI (00020328-0000-0000-C000-000000000046) </summary>
        [Guid("00020328-0000-0000-C000-000000000046")]
        [Description("MAPI")]
        MAPI,

        /// <summary> InternetHeaders (00020386-0000-0000-C000-000000000046) </summary>
        [Guid("00020386-0000-0000-C000-000000000046")]
        [Description("Internet headers")]
        InternetHeaders,

        /// <summary> Unknown1 (0006200B-0000-0000-C000-000000000046) </summary>
        [Guid("0006200B-0000-0000-C000-000000000046")]
        [Description("Unknown - 1")]
        Unknown1,

        /// <summary> Unknown2 (00062014-0000-0000-C000-000000000046) </summary>
        [Guid("00062014-0000-0000-C000-000000000046")]
        [Description("Unknown - 2")]
        Unknown2,

        /// <summary> PostRSS (00062041-0000-0000-C000-000000000046) </summary>
        [Guid("00062041-0000-0000-C000-000000000046")]
        [Description("Post RSS")]
        PostRSS,

        /// <summary> Unknown3 (29F3AB56-554D-11D0-A97C-00A0C911F50A) </summary>
        [Guid("29F3AB56-554D-11D0-A97C-00A0C911F50A")]
        [Description("Unknown - 3")]
        Unknown3,

        /// <summary> UnifiedMessaging (4442858E-A9E3-4E80-B900-317A210CC15B) </summary>
        [Guid("4442858E-A9E3-4E80-B900-317A210CC15B")]
        [Description("Unified Messaging")]
        UnifiedMessaging,

        /// <summary> AirSync (71035549-0739-4DCB-9163-00F0580DBBDF) </summary>
        [Guid("71035549-0739-4DCB-9163-00F0580DBBDF")]
        [Description("Air Sync")]
        AirSync,

        /// <summary> Attachment (96357F7F-59E1-47D0-99A7-46515C183B54) </summary>
        [Guid("96357F7F-59E1-47D0-99A7-46515C183B54")]
        [Description("Attachment")]
        Attachment,

    }
}
