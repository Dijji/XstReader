// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;

namespace XstReader.Common.BTrees
{
    // Terminal node in data tree
    // Key is BID, contains IB
    internal class DataRef : TreeNode
    {
        public UInt64 Offset;
        public int Length;
        public int InflatedLength; // Only used for Unicode4K
        public bool IsInternal { get { return (Key & 0x02) != 0; } }
    }
}
