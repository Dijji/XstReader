// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Runtime.InteropServices;

namespace XstReader
{
    // xst layouts that are specific to Unicode files with 4K pages
    // These are not documented in <MS-PST>, But taken instead from libpff 
    // and https://blog.mythicsoft.com/2015/07/10/ost-2013-file-format-the-missing-documentation/

    // Constants
    class LayoutsU4K
    {
        public const int BTPAGEEntryBytes = 4056;
    }

    // NDB layer

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct BTPAGEUnicode4K
    {
        public fixed Byte rgentries[LayoutsU4K.BTPAGEEntryBytes];
        public UInt16 cEnt;
        public UInt16 cEntMax;
        public Byte cbEnt;
        public Byte cLevel;
        public UInt16 dwPadding1;
        public UInt32 dwPadding2;
        public UInt32 dwPadding3;
        public PAGETRAILERUnicode4K pageTrailer;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct PAGETRAILERUnicode4K
    {
        public Eptype ptype;
        public Eptype ptypeRepeat;
        public UInt16 wSig;
        public UInt32 dwCRC;
        public UInt64 bid;
        public UInt64 unknown;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BBTENTRYUnicode4K
    {
        public BREFUnicode BREF;
        public UInt16 cbStored;
        public UInt16 cbInflated;
        public UInt16 cRef;
        public UInt16 wPadding;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BLOCKTRAILERUnicode4K
    {
        public UInt16 cb;
        public UInt16 wSig;
        public UInt32 dwCRC;
        public UInt64 bid;
        public UInt16 unknown1;
        public UInt16 cbInflated;
        public UInt32 unknown2;
    }
}
