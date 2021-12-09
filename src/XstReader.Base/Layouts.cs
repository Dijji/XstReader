// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Runtime.InteropServices;

namespace XstReader
{
    // xst layouts that are the same for Unicode and ANSI
    // All layouts and names are taken from <MS-PST>

    // NDB layer

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct FileHeader1
    {
        public UInt32 dwMagic;
        public UInt32 dwCRCPartial;
        public UInt16 wMagicClient;
        public UInt16 wVer;
        public UInt16 wVerClient;
        public Byte bPlatformCreate;
        public Byte bPlatformAccess;
        public UInt32 dwReserved1;
        public UInt32 dwReserved2;
    }

    public enum EbCryptMethod : byte
    {
        NDB_CRYPT_NONE = 0x00,
        NDB_CRYPT_PERMUTE = 0x01,
        NDB_CRYPT_CYCLIC = 0x02,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NID
    {
        public UInt32 dwValue; // References use the whole four bytes
        public EnidType nidType {get { return (EnidType)(dwValue & 0x0000001f); } }  // Low order five bits of stored value
        public UInt32 nidIndex { get { return dwValue >> 5; } }
        public bool HasValue { get { return (dwValue != 0); } }

        public NID(UInt32 nid)
        {
            this.dwValue = nid;
        }

        public NID(EnidSpecial special)
        {
            this.dwValue = (UInt32)special;
        }

        public static NID TypedNID(EnidType nidType, NID nid)
        {
            // Note that type partially overwrites value
            return new NID((nid.dwValue & 0xffffffe0) | (UInt32)nidType);
        }
    }

    public enum EnidType : byte
    {
        HID = 0x00,   // Heap node
        INTERNAL = 0x01,   // Internal node
        NORMAL_FOLDER = 0x02,  // Normal Folder object
        SEARCH_FOLDER = 0x03,  // Search Folder object
        NORMAL_MESSAGE = 0x04,
        ATTACHMENT = 0x05,
        SEARCH_UPDATE_QUEUE = 0x06,
        SEARCH_CRITERIA_OBJECT = 0x07,
        ASSOC_MESSAGE = 0x08,   // Folder associated information (FAI) Message object
        CONTENTS_TABLE_INDEX = 0x0a, // Internal, persisted view-related
        RECEIVE_FOLDER_TABLE = 0x0b, // Receive Folder object (Inbox)
        OUTGOING_QUEUE_TABLE = 0x0c, // Outbound queue (Outbox)
        HIERARCHY_TABLE = 0x0d,
        CONTENTS_TABLE = 0x0e,
        ASSOC_CONTENTS_TABLE = 0x0f,   // FAI contents table
        SEARCH_CONTENTS_TABLE = 0x10,   // Contents table (TC) of a search Folder object
        ATTACHMENT_TABLE = 0x11,
        RECIPIENT_TABLE = 0x12,
        SEARCH_TABLE_INDEX = 0x13,  // Internal, persisted view-related
        LTP = 0x1f,
    }

    public enum EnidSpecial : uint
    {
        NID_MESSAGE_STORE = 0x21,
        NID_NAME_TO_ID_MAP = 0x61,
        NID_NORMAL_FOLDER_TEMPLATE = 0xa1,
        NID_SEARCH_FOLDER_TEMPLATE = 0xc1,
        NID_ROOT_FOLDER = 0x122,
        NID_SEARCH_MANAGEMENT_QUEUE = 0x1e1,
        NID_SEARCH_ACTIVITY_LIST = 0x201,
        NID_SEARCH_DOMAIN_OBJECT = 0x261,
        NID_SEARCH_GATHERER_QUEUE = 0x281,
        NID_SEARCH_GATHERER_DESCRIPTOR = 0x2a1,
        NID_SEARCH_GATHERER_FOLDER_QUEUE = 0x321,
        NID_ATTACHMENT_TABLE = 0x671,
        NID_RECIPIENT_TABLE = 0x692,
    }

    public enum Eptype : byte
    {
        ptypeBBT = 0x80,   // Block BTree page
        ptypeNBT = 0x81,   // Node BTree page
        ptypeFMap = 0x82,  // Free Map page
        ptypePMap = 0x83,  // Allocation Page Map page
        ptypeAMap = 0x84,  // Allocation Map page
        ptypeFPMap = 0x85, // Free Page Map page
        ptypeDL = 0x86,    // Density List page
    }

    // LTP layer

    // The HID is a 32-bit value, with the following internal structure
    // non-4K: 5-bit Type; 11-bit Index; 16-bit BlockIndex
    // 4K:     5-bit Type; 14-bit Index; 13-bit BlockIndex
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct HID
    {
        private UInt16 wValue1;
        private UInt16 wValue2;
 
        private UInt16 hidIndex { get { return (UInt16)(wValue1 >> 5); } }
        private UInt16 hidBlockIndex { get { return wValue2; } }
        private UInt16 hidIndex4K { get { return (UInt16)((wValue1 >> 5) | ((wValue2 & 0x0007) << 11)); } }
        private UInt16 hidBlockIndex4K { get { return (UInt16)(wValue2 >> 3); } }

        public HID(UInt16 wValue1, UInt16 wValue2)
        {
            this.wValue1 = wValue1;
            this.wValue2 = wValue2;
        }

        public EnidType hidType { get { return (EnidType)(wValue1 & 0x001f); } }

        public UInt16 GetIndex(bool isUnicode4K)
        {
            return isUnicode4K ? hidIndex4K : hidIndex;
        }

        public UInt16 GetBlockIndex(bool isUnicode4K)
        {
            return isUnicode4K ? hidBlockIndex4K : hidBlockIndex;
        }
    }

    // A variation on HID used where the value can be either a HID or a NID
    // It is a HID iff hidType is EnidType.HID and the wValues are not both zero
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct HNID
    {
        private UInt16 wValue1;
        private UInt16 wValue2;

        private UInt16 hidIndex { get { return (UInt16)(wValue1 >> 5); } }
        private UInt16 hidBlockIndex { get { return wValue2; } }
        private UInt16 hidIndex4K { get { return (UInt16)((wValue1 >> 5) | ((wValue2 & 0x0007) << 11)); } }
        private UInt16 hidBlockIndex4K { get { return (UInt16)(wValue2 >> 3); } }

        public bool HasValue { get { return (wValue1 != 0) || (wValue2 != 0); } }
        public bool IsHID { get { return HasValue && hidType == EnidType.HID; } }
        public EnidType hidType { get { return (EnidType)(wValue1 & 0x001f); } }
        public EnidType nidType { get { return (EnidType)(wValue1 & 0x001f); } }  // Low order five bits of stored value
        public UInt32 dwValue { get { return (UInt32)(wValue2 << 16) | wValue1; } }  // References use the whole four bytes
        public HID HID { get { return new HID(wValue1, wValue2); } }
        public NID NID { get { return new NID(dwValue); } }

        public UInt16 GetIndex(bool isUnicode4K)
        {
            return isUnicode4K ? hidIndex4K : hidIndex;
        }

        public UInt16 GetBlockIndex(bool isUnicode4K)
        {
            return isUnicode4K ? hidBlockIndex4K : hidBlockIndex;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct HNHDR
    {
        public UInt16 ibHnpm;       // The byte offset to the HN page Map record
        public Byte bSig;
        public EbType bClientSig;
        public HID hidUserRoot;  // HID that points to the User Root record
        public UInt32 rgbFillLevel;
    }

    public enum EbType : byte
    {
        bTypeX = 0x01,   // XBLOCK or XXBLOCK
        bTypeS = 0x02,   // SLBLOCK or SIBLOCK
        bTypeTC = 0x7c,   // Table Context
        bTypeBTH = 0xb5,   // BTree-on-Heap
        bTypePC = 0xbc,  // Property Context
    }

    //
    // Page structures
    //

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct HNPAGEHDR
    {
        public UInt16 ibHnpm;       // The byte offset to the HN page Map record
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct HNBITMAPHDR
    {
        public UInt16 ibHnpm;       // The byte offset to the HN page Map record
        public fixed Byte rgbFillLevel[64];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct HNPAGEMAP
    {
        public UInt16 cAlloc;
        public UInt16 cFree;
        // Marshal the following array manually
        //public UInt16[] rgibAlloc;
    }

    //
    // BTree-on-Heap structures
    //

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BTHHEADER
    {
        public EbType btype;
        public Byte cbKey;
        public Byte cbEnt;
        public Byte bIdxLevels;
        public HID hidRoot;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct IntermediateBTH2
    {
        public fixed Byte key[2];
        public HID hidNextLevel;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct IntermediateBTH4
    {
        public fixed Byte key[4];
        public HID hidNextLevel;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct IntermediateBTH8
    {
        public fixed Byte key[8];
        public HID hidNextLevel;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct IntermediateBTH16
    {
        public fixed Byte key[16];
        public HID hidNextLevel;
    }

    //
    // Property Context structures
    //

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct PCBTH
    {
        public EpropertyTag wPropId;
        public EpropertyType wPropType;
        public HNID dwValueHnid;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct MVVariable
    {
        public UInt32 ulCount;
        public UInt32[] rgulDataOffsets;
        public Byte[] rgDataItems;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct PtypObjectProperty
    {
        public NID nid;
        public UInt32 ulSize;
    }

    //
    // Table Context structures
    //

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TCINFO
    {
        public EbType btype;    // Must be bTypeTC
        public Byte cCols;
        public UInt16 rgibTCI_4b;
        public UInt16 rgibTCI_2b;
        public UInt16 rgibTCI_1b;
        public UInt16 rgibTCI_bm;
        public HID hidRowIndex;
        public HNID hnidRows;
        public HID hidIndex;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TCOLDESC
    {
        public UInt32 tag;
        public EpropertyTag wPropId { get { return (EpropertyTag)(tag >> 16); } }
        public EpropertyType wPropType { get { return (EpropertyType)(tag & 0x0000ffff); } }
        public UInt16 ibData;
        public Byte cbData;
        public Byte iBit;
    }

    // This is right, but we don't use it, because there is no way to marshal it
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //struct RowData
    //{
    //    public UInt32 dwRowID;
    //    public UInt32[] rgdwData;
    //    public UInt16[] rgwData;
    //    public Byte[] rgbData;
    //    public Byte[] rgbCEB;
    //}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct NAMEID
    {
        public UInt32 dwPropertyID;
        private UInt16 wValue;
        public UInt16 wPropIdx;
        public bool N { get { return (wValue & 0x0001) == 1; } }
        public UInt16 wGuid { get { return (UInt16)(wValue >> 1); } }
    }

    public enum EwGuid : UInt16
    {
        NAMEID_GUID_NONE = 0,
        NAMEID_GUID_MAPI = 1,
        NAMEID_GUID_PUBLIC_STRINGS = 2,
        InStream = 3,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PtypObjectValue
    {
        public UInt32 Nid;
        public UInt32 ulSize;
    }
}
