// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;

namespace XstReader
{
    //
    // Classes used to represent the various B-trees
    // It turns out that an efficient way to go is to read the B-trees in from the file to a 
    // memory representation that we then use for lookup, avoiding the overhead of building new dictionaries
    //

    //  Generic handling for all trees
    public class BTree<T> where T : TreeNode
    {
        public TreeIntermediate Root { get; private set; } = new TreeIntermediate();

        public T Lookup(UInt32 key, Action<TreeIntermediate> readDeferred = null)
        {
            return (T)LookupTreeNode(Root, key, readDeferred);
        }

        public T Lookup(UInt64 key, Action<TreeIntermediate> readDeferred = null)
        {
            return (T)LookupTreeNode(Root, key, readDeferred);
        }

        // Perform a lookup in the b-tree
        private static TreeNode LookupTreeNode(TreeIntermediate parent, UInt64 key, Action<TreeIntermediate> readDeferred)
        {
            if (parent.ReadDeferred)
            {
                if (readDeferred != null)
                    readDeferred(parent);
                else
                    throw new XstException("Deferred index found, but no reader supplied");
            }

            TreeIntermediate next = null;
            foreach (var n in parent.Children)
            {
                if (key > n.Key)
                {
                    next = n as TreeIntermediate;
                }
                else if (key < n.Key)
                {
                    if (next != null)
                        return LookupTreeNode(next, key, readDeferred);
                    else
                        return null; // Key does not exist
                }
                else // key matches
                {
                    if (n is TreeIntermediate)
                        return LookupTreeNode((TreeIntermediate)n, key, readDeferred);
                    else
                        return n;
                }
            }
            if (next != null)
                return LookupTreeNode(next, key, readDeferred);
            else
                return null; // Key does not exist
        }
    }

    // The only thing that a tree node must have is a key
    public class TreeNode
    {
        public UInt64 Key;
    }

    // Non-terminal tree nodes have children
    public class TreeIntermediate : TreeNode
    {
        public List<TreeNode> Children = new List<TreeNode>();
        public ulong? fileOffset = null;
        public bool ReadDeferred { get { return fileOffset != null; } }
    }

    // Terminal node in node tree
    // Key is NID, contains BIDs
    // BIDs are always held as 64-bit values, even though only 32 bits are used in ANSI files
    public class Node : TreeNode
    {
        public EnidType Type;
        public UInt64 DataBid;
        public UInt64 SubDataBid;
        public UInt32 Parent;
    }

    // Terminal node in data tree
    // Key is BID, contains IB
    public class DataRef : TreeNode
    {
        public UInt64 Offset;
        public int Length;
        public int InflatedLength; // Only used for Unicode4K
        public bool IsInternal { get { return (Key & 0x02) != 0; } }
    }
}
