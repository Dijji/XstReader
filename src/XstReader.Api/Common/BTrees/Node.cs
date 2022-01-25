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

namespace XstReader.Common.BTrees
{
    // Terminal node in node tree
    // Key is NID, contains BIDs
    // BIDs are always held as 64-bit values, even though only 32 bits are used in ANSI files
    internal class Node : TreeNode
    {
        public EnidType Type;
        public UInt64 DataBid;
        public UInt64 SubDataBid;
        public UInt32 Parent;
    }
}
