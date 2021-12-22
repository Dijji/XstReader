// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System.Collections.Generic;

namespace XstReader.Common.BTrees
{
    // Non-terminal tree nodes have children
    internal class TreeIntermediate : TreeNode
    {
        public List<TreeNode> Children = new List<TreeNode>();
        public ulong? fileOffset = null;
        public bool ReadDeferred { get { return fileOffset != null; } }
    }
}
