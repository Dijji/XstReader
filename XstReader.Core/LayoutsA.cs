// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Runtime.InteropServices;

namespace XstReader
{
    // xst layouts that are ANSI specific    
    // All layouts and names are taken from <MS-PST>

    // Constants
    class LayoutsA
    {
        public const int BTPAGEEntryBytes = 496;
    }

    // NDB layer

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct FileHeader2ANSI
    {
        public UInt32 bidNextB;
        public UInt32 bidNextP;
        public UInt32 dwUnique;
        public fixed UInt32 rgnid[32];
        public RootANSI root;
        public fixed Byte rgbFM[128];
        public fixed Byte rgbFP[128];
        public Byte bSentinel;
        public EbCryptMethod bCryptMethod;
        public UInt16 rgbReserved;
        public UInt64 ullReserved;
        public UInt32 dwReserved;
        public UInt32 rgbReserved2;
        public fixed Byte rgbReserved3[32];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct RootANSI
    {
        public UInt32 dwReserved;
        public UInt32 ibFileEof;
        public UInt32 ibAMapLast;
        public UInt32 cbAMapFree;
        public UInt32 cbPMapFree;
        public BREFANSI BREFNBT;
        public BREFANSI BREFBBT;
        public Byte fAMapValid;
        public Byte bReserved;
        public UInt16 wReserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BREFANSI
    {
        public UInt32 bid;
        public UInt32 ib;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct BTPAGEANSI
    {
        public fixed Byte rgentries[LayoutsA.BTPAGEEntryBytes];
        public Byte cEnt;
        public Byte cEntMax;
        public Byte cbEnt;
        public Byte cLevel;
        public PAGETRAILERANSI pageTrailer;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct PAGETRAILERANSI
    {
        public Eptype ptype;
        public Eptype ptypeRepeat;
        public UInt16 wSig;
        public UInt32 bid;
        public UInt32 dwCRC;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BTENTRYANSI
    {
        public UInt32 btkey;
        public BREFANSI BREF;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BBTENTRYANSI
    {
        public BREFANSI BREF;
        public UInt16 cb;
        public UInt16 cRef;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct NBTENTRYANSI
    {
        public NID nid;
        public UInt32 bidData;
        public UInt32 bidSub;
        public UInt32 nidParent;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BLOCKTRAILERANSI
    {
        public UInt16 cb;
        public UInt16 wSig;
        public UInt32 bid;
        public UInt32 dwCRC;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct XBLOCKANSI
    {
        public EbType btype;
        public Byte cLevel;
        public UInt16 cEnt;
        public UInt32 lcbTotal;
        // Marshal the following array manually
        //public UInt32[] rgbid;
    }

    // We just use the XBLOCKANSI structure for XXBLOCKANSI, as it is the same
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //struct XXBLOCKANSI
    //{
    //    public EbType btype;
    //    public Byte cLevel;
    //    public UInt16 cEnt;
    //    public UInt32 lcbTotal;
    //    //public UInt32[] rgbid;
    //}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SLENTRYANSI
    {
        public UInt32 nid;
        public UInt32 bidData;
        public UInt32 bidSub;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SLBLOCKANSI
    {
        public EbType btype;
        public Byte cLevel;
        public UInt16 cEnt;
        //public UInt32 dwPadding;    // [MS-PST] v20150904 lies, asserting that this occurs in ANSI files. A bug has been raised with Microsoft
        // Marshal the following array manually
        //public SLENTRYANSI[] rgentries;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct SIENTRYANSI
    {
        public UInt32 nid;
        public UInt32 bid;
    }

    // We just use the SLBLOCKANSI structure for SIBLOCKANSI, as it is the same apart from the array type
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //struct SIBLOCKANSI
    //{
    //    public EbType btype;
    //    public Byte cLevel;
    //    public UInt16 cEnt;
    //    public UInt32 dwPadding;
    //    public SIENTRYANSI[] rgentries;
    //}

    // LTP layer

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TCROWIDANSI
    {
        public UInt32 dwRowID;
        public UInt16 dwRowIndex;
    }
}