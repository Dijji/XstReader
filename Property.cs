// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;

namespace XstReader
{
    // Enums and classes used in property handling
    // Enum names are taken from <MS-PST>

    public enum EpropertyType : UInt16
    {
        PtypBinary = 0x0102,
        PtypBoolean = 0x000b,
        PtypGuid = 0x0048,
        PtypInteger32 = 0x0003,
        PtypInteger64 = 0x0014,
        PtypMultipleInteger32 = 0x1003,
        PtypObject = 0x000d,
        PtypString = 0x001f,
        PtypString8 = 0x001e,
        PtypTime = 0x0040,
    }

    public enum EpropertyTag : UInt16
    {
        PidTagDisplayName = 0x3001,

        // Root folder
        PidTagRecordKey = 0x0FF9,
        PidTagIpmSubTreeEntryId = 0x35E0,

        // Folder
        PidTagContentCount = 0x3602,
        PidTagSubfolders = 0x360A,

        // Message List
        PidTagSubjectW = 0x0037,
        PidTagDisplayCcW = 0x0E03,
        PidTagDisplayToW = 0x0E04,
        PidTagMessageFlags = 0x0E07,
        PidTagMessageDeliveryTime = 0x0E06,
        PidTagSentRepresentingNameW = 0x0042,
        PidTagClientSubmitTime = 0x0039,
        PidTagLastModificationTime = 0x3008,
        //PidTagLtpRowId = 0x67F2,

        // Message body
        PidTagNativeBody = 0x1016,
        PidTagBody = 0x1000,
        PidTagInternetCodepage = 0x3FDE,
        PidTagHtml = 0x1013,
        //PidTagBodyHtml = 0x1013,
        PidTagRtfCompressed = 0x1009,

        // Attachment
        PidTagAttachFilenameW = 0x3704,
        PidTagAttachLongFilename = 0x3707,
        PidTagAttachmentSize = 0x0E20,
        PidTagAttachMethod = 0x3705,
        PidTagAttachDataBinary = 0x3701,
    }

    // Values of the PidTagNativeBody property
    public enum BodyType : Int32
    {
        Undefined = 0x00000000,
        PlainText = 0x00000001,
        RTF = 0x00000002,
        HTML = 0x00000003,
        ClearSigned = 0x00000004,
    }

    // Values of the PidTagMessageFlags property
    [Flags]
    public enum MessageFlags : Int32
    {
        mfHasAttach = 0x00000010,
    }

    // Values of the PidTagAttachMethod property
    public enum AttachMethods : Int32
    {
        afByValue = 0x00000001,
        afStorage = 0x00000006,
    }

    // Property getters are used to specify which properties should be retrieved from a property context
    // or table context, and where they should be stored.
    // T is the target object, Action arguments are target object, column value 
    public class PropertyGetters<T> : Dictionary<EpropertyTag, Action<T, dynamic>>
    {
    }
}
