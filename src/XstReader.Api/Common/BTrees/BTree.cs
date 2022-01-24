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
    //
    // Classes used to represent the various B-trees
    // It turns out that an efficient way to go is to read the B-trees in from the file to a 
    // memory representation that we then use for lookup, avoiding the overhead of building new dictionaries
    //

    //  Generic handling for all trees
    internal class BTree<T> where T : TreeNode
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
                    return n is TreeIntermediate ? LookupTreeNode((TreeIntermediate)n, key, readDeferred) : n;
                }
            }
            if (next != null)
                return LookupTreeNode(next, key, readDeferred);
            else
                return null; // Key does not exist
        }
    }

}
