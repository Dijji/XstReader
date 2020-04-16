// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace XstReader
{
    /// <summary>
    /// This class implements the NDB (Node Database) layer, Which provides lower-level storage facilities
    /// </summary>
    class NDB
    {
        private string fullName;
        private EbCryptMethod bCryptMethod;
        private BTree<Node> nodeTree = new BTree<Node>();
        private BTree<DataRef> dataTree = new BTree<DataRef>();

        // deferReadingNodes is a switch set only here
        // If true, reading node and data tree b-trees is deferred until look-up time,
        // and only required portions of the tree are read
        // If false, the full b-trees are read at file open time
        // The former setting (the default) gets the folder structure almost instantly after opening,
        // and consumes less memory in many scenarios,
        // but may make opening a folder slower and net less efficient
        private bool deferReadingNodes = true;
        private Action<TreeIntermediate> deferredReadAction;

        public int BlockSize { get; private set; } = 8192;
        public int BlockSize4K { get; private set; } = 65536;
        public bool IsUnicode { get; private set; }
        public bool IsUnicode4K { get; private set; } = false;

        #region Public methods

        public NDB(string fullName)
        {
            this.fullName = fullName;
            deferredReadAction = new Action<TreeIntermediate>(p => this.ReadDeferredIndex(p));
        }

        // Prepare to read the contents of a file
        public void Initialise()
        {
            ReadHeaderAndIndexes();
        }
        
        // Return a file stream that will allow the caller to read the current file
        public FileStream GetReadStream()
        {
            return new FileStream(fullName, FileMode.Open, FileAccess.Read);
        }

        // Decrypt the contents of a data buffer if necessary
        public void Decrypt(ref byte[] buffer, UInt32 key, int offset = 0, int length = 0)
        {
            Crypto.Decrypt(ref buffer, bCryptMethod, key, offset, length > 0 ? length : buffer.Length - offset);
        }

        // Look up a node in the main node b-tree
        public Node LookupNode(NID nid)
        {
            return nodeTree.Lookup(nid.dwValue, deferredReadAction);
        }

        // Look up a data block in the main data block b-tree
        public DataRef LookupDataBlock(UInt64 dataBid)
        {
            // Clear the LSB, which is reserved, but sometimes set
            return dataTree.Lookup(dataBid & 0xfffffffe, deferredReadAction);
        }

        // Read a sub-node's b-tree from the specified data block
        public BTree<Node> ReadSubNodeBtree(FileStream fs, UInt64 subDataBid)
        {
            var tree = new BTree<Node>();
            if (IsUnicode)
                ReadSubNodeBtreeUnicode(fs, subDataBid, tree.Root);
            else
                ReadSubNodeBtreeANSI(fs, subDataBid, tree.Root);
            return tree;
        }

        // Look up a node in a sub-node's b-tree
        public static Node LookupSubNode(BTree<Node> subNodeTree, NID nid)
        {
            if (subNodeTree == null)
                throw new XstException("No sub node data tree found");

            return subNodeTree.Lookup(nid.dwValue);
        }

        // Read raw data, accessed via a sub node
        // If it has a multiblock structure, return all of the blocks' contents concatenated
        public byte[] ReadSubNodeDataBlock(FileStream fs, BTree<Node> subNodeTree, NID nid)
        {
            if (!nid.HasValue)
                return null;
            var n = LookupSubNode(subNodeTree, nid);
            if (n == null)
                throw new XstException("Node not found in sub node tree");
            if (n.SubDataBid != 0)
                throw new XstException("Sub-nodes of sub-nodes not yet implemented");
            return ReadDataBlock(fs, n.DataBid);
        }

        // Read all the data blocks for a given BID, concatenating them into a single buffer
        public byte[] ReadDataBlock(FileStream fs, UInt64 dataBid)
        {
            int offset = 0;
            return ReadDataBlockInternal(fs, dataBid, ref offset);
        }

        // Read all the data blocks for a given BID, enumerating them one by one
        public IEnumerable<byte[]> ReadDataBlocks(FileStream fs, UInt64 dataBid)
        {
            foreach (var buf in ReadDataBlocksInternal(fs, dataBid))
            {
                yield return buf;
            }
        }

        // Copy data to the specified file stream
        // We write each external data block as we read it, so that we never have more than one in memory at the same time
        public void CopyDataBlocks(FileStream fs, Stream s, UInt64 dataBid)
        {
            foreach (var buf in ReadDataBlocksInternal(fs, dataBid))
            {
                s.Write(buf, 0, buf.Length);
            }
        }

        // Some nodes use sub-nodes to hold local data, so we need to give access to both levels
        public Node LookupNodeAndReadItsSubNodeBtree(FileStream fs, NID nid, out BTree<Node> subNodeTree)
        {
            subNodeTree = null;
            var rn = LookupNode(nid);
            if (rn == null)
                throw new XstException("Node block does not exist");
            // If there is a sub-node, read its btree so that we can resolve references to nodes in it later
            if (rn.SubDataBid != 0)
            {
                subNodeTree = ReadSubNodeBtree(fs, rn.SubDataBid);
            }
            return rn;
        }

        // Some sub-nodes use sub-sub-nodes to hold local data, so we need to give access to both levels
        public Node LookupSubNodeAndReadItsSubNodeBtree(FileStream fs, BTree<Node> subNodeTree, NID nid, out BTree<Node> childSubNodeTree)
        {
            childSubNodeTree = null;
            var rn = LookupSubNode(subNodeTree, nid);
            if (rn == null)
                throw new XstException("Node block does not exist");
            // If there is a sub-node, read its btree so that we can resolve references to nodes in it later
            if (rn.SubDataBid != 0)
            {
                childSubNodeTree = ReadSubNodeBtree(fs, rn.SubDataBid);
            }
            return rn;
        }

        #endregion


        #region Private methods

        // Read the file header, and the B trees that give us access to nodes and data blocks
        private void ReadHeaderAndIndexes()
        {
            using (var fs = GetReadStream())
            {
                var h = Map.ReadType<FileHeader1>(fs);

                if (h.dwMagic != 0x4e444221)
                    throw new XstException("File is not a .ost or .pst file: the magic cookie is missing");

                if (h.wVer == 0x15 || h.wVer == 0x17 )
                {
                    var h2 = Map.ReadType<FileHeader2Unicode>(fs);
                    bCryptMethod = h2.bCryptMethod;
                    IsUnicode = true;
                    ReadBTPageUnicode(fs, h2.root.BREFNBT.ib, nodeTree.Root);
                    ReadBTPageUnicode(fs, h2.root.BREFBBT.ib, dataTree.Root);
                }
                else if (h.wVer == 0x24)
                {
                    // This value indicates the use of 4K pages, as opposed to 512 bytes
                    // It is used only in .ost files, and was introduced in Office 2013
                    // It is not documented in [MS-PST], being .ost only
                    var h2 = Map.ReadType<FileHeader2Unicode>(fs);
                    bCryptMethod = h2.bCryptMethod;
                    IsUnicode = true;
                    IsUnicode4K = true;
                    ReadBTPageUnicode4K(fs, h2.root.BREFNBT.ib, nodeTree.Root);
                    ReadBTPageUnicode4K(fs, h2.root.BREFBBT.ib, dataTree.Root);
                }
                else if (h.wVer == 0x0e || h.wVer == 0x0f)
                {
                    var h2 = Map.ReadType<FileHeader2ANSI>(fs);
                    bCryptMethod = h2.bCryptMethod;
                    IsUnicode = false;
                    ReadBTPageANSI(fs, h2.root.BREFNBT.ib, nodeTree.Root);
                    ReadBTPageANSI(fs, h2.root.BREFBBT.ib, dataTree.Root);
                }
                else
                    throw new XstException("Unrecognised header type");
            }
        }

        // A callback to be used when searching a tree to read part of the index whose loading has been deferred
        private void ReadDeferredIndex(TreeIntermediate inter)
        {
            using (var fs = GetReadStream())
            {
                if (IsUnicode4K)
                    ReadBTPageUnicode4K(fs, (ulong)inter.fileOffset, inter);
                else if (IsUnicode)
                    ReadBTPageUnicode(fs, (ulong)inter.fileOffset, inter);
                else
                    ReadBTPageANSI(fs, (ulong)inter.fileOffset, inter);

                // Don't read it again
                inter.fileOffset = null;
            }
        }
        // Read a page containing part of a node or data block B-tree, and build the corresponding data structure
        private void ReadBTPageUnicode(FileStream fs, ulong fileOffset, TreeIntermediate parent)
        {
            fs.Seek((long)fileOffset, SeekOrigin.Begin);
            var p = Map.ReadType<BTPAGEUnicode>(fs);

            // read entries
            for (int i = 0; i < p.cEnt; i++)
            {
                if (p.cLevel > 0)
                {
                    BTENTRYUnicode e;
                    unsafe
                    {
                        e = Map.MapType<BTENTRYUnicode>(p.rgentries, LayoutsU.BTPAGEEntryBytes, i * p.cbEnt, p.cbEnt);
                    }
                    var inter = new TreeIntermediate { Key = e.btkey };
                    parent.Children.Add(inter);

                    if (deferReadingNodes)
                        inter.fileOffset = e.BREF.ib;
                    else
                    {
                        // Read child nodes in tree
                        ReadBTPageUnicode(fs, e.BREF.ib, inter);
                        inter.fileOffset = null;
                    }
                }
                else
                {
                    if (p.pageTrailer.ptype == Eptype.ptypeNBT)
                    {
                        unsafe
                        {
                            var e = Map.MapType<NBTENTRYUnicode>(p.rgentries, LayoutsU.BTPAGEEntryBytes, i * p.cbEnt, p.cbEnt);
                            var nb = new Node { Key = e.nid.dwValue, Type = e.nid.nidType, DataBid = e.bidData, SubDataBid = e.bidSub, Parent = e.nidParent };
                            parent.Children.Add(nb);
                        }
                    }
                    else if (p.pageTrailer.ptype == Eptype.ptypeBBT)
                    {
                        unsafe
                        {
                            var e = Map.MapType<BBTENTRYUnicode>(p.rgentries, LayoutsU.BTPAGEEntryBytes, i * p.cbEnt, p.cbEnt);
                            parent.Children.Add(new DataRef { Key = e.BREF.bid, Offset = e.BREF.ib, Length = e.cb });
                        }
                    }
                    else
                        throw new XstException("Unexpected page entry type");
                }
            }
        }

        // Read a page containing part of a node or data block B-tree, and build the corresponding data structure
        private void ReadBTPageUnicode4K(FileStream fs, ulong fileOffset, TreeIntermediate parent)
        {
            fs.Seek((long)fileOffset, SeekOrigin.Begin);
            var p = Map.ReadType<BTPAGEUnicode4K>(fs);

            // read entries
            for (int i = 0; i < p.cEnt; i++)
            {
                if (p.cLevel > 0)
                {
                    BTENTRYUnicode e;
                    unsafe
                    {
                        e = Map.MapType<BTENTRYUnicode>(p.rgentries, LayoutsU4K.BTPAGEEntryBytes, i * p.cbEnt, p.cbEnt);
                    }
                    var inter = new TreeIntermediate { Key = e.btkey };
                    parent.Children.Add(inter);

                    if (deferReadingNodes)
                        inter.fileOffset = e.BREF.ib;
                    else
                    {
                        // Read child nodes in tree
                        ReadBTPageUnicode4K(fs, e.BREF.ib, inter);
                        inter.fileOffset = null;
                    }
                }
                else
                {
                    if (p.pageTrailer.ptype == Eptype.ptypeNBT)
                    {
                        unsafe
                        {
                            var e = Map.MapType<NBTENTRYUnicode>(p.rgentries, LayoutsU4K.BTPAGEEntryBytes, i * p.cbEnt, p.cbEnt);
                            var nb = new Node { Key = e.nid.dwValue, Type = e.nid.nidType, DataBid = e.bidData, SubDataBid = e.bidSub, Parent = e.nidParent };
                            parent.Children.Add(nb);
                        }
                    }
                    else if (p.pageTrailer.ptype == Eptype.ptypeBBT)
                    {
                        unsafe
                        {
                            var e = Map.MapType<BBTENTRYUnicode4K>(p.rgentries, LayoutsU4K.BTPAGEEntryBytes, i * p.cbEnt, p.cbEnt);
                            parent.Children.Add(new DataRef { Key = e.BREF.bid, Offset = e.BREF.ib, Length = e.cbStored, InflatedLength = e.cbInflated });
                        }
                    }
                    else
                        throw new XstException("Unexpected page entry type");
                }
            }
        }

        private void ReadBTPageANSI(FileStream fs, ulong fileOffset, TreeIntermediate parent)
        {
            fs.Seek((long)fileOffset, SeekOrigin.Begin);
            var p = Map.ReadType<BTPAGEANSI>(fs);

            // read entries
            for (int i = 0; i < p.cEnt; i++)
            {
                if (p.cLevel > 0)
                {
                    BTENTRYANSI e;
                    unsafe
                    {
                        e = Map.MapType<BTENTRYANSI>(p.rgentries, LayoutsA.BTPAGEEntryBytes, i * p.cbEnt, p.cbEnt);
                    }
                    var inter = new TreeIntermediate { Key = e.btkey };
                    parent.Children.Add(inter);

                    if (deferReadingNodes)
                        inter.fileOffset = e.BREF.ib;
                    else
                    {
                        // Read child nodes in tree
                        ReadBTPageANSI(fs, e.BREF.ib, inter);
                        inter.fileOffset = null;
                    }
                }
                else
                {
                    if (p.pageTrailer.ptype == Eptype.ptypeNBT)
                    {
                        unsafe
                        {
                            var e = Map.MapType<NBTENTRYANSI>(p.rgentries, LayoutsA.BTPAGEEntryBytes, i * p.cbEnt, p.cbEnt);
                            var nb = new Node { Key = e.nid.dwValue, Type = e.nid.nidType, DataBid = e.bidData, SubDataBid = e.bidSub, Parent = e.nidParent };
                            parent.Children.Add(nb);
                        }
                    }
                    else if (p.pageTrailer.ptype == Eptype.ptypeBBT)
                    {
                        unsafe
                        {
                            var e = Map.MapType<BBTENTRYANSI>(p.rgentries, LayoutsA.BTPAGEEntryBytes, i * p.cbEnt, p.cbEnt);
                            parent.Children.Add(new DataRef { Key = e.BREF.bid, Offset = e.BREF.ib, Length = e.cb });
                        }
                    }
                    else
                        throw new XstException("Unexpected page entry type");
                }
            }
        }

        // Return all the data blocks referenced by a particular data Id
        private IEnumerable<byte[]> ReadDataBlocksInternal(FileStream fs, UInt64 dataBid, uint totalLength = 0)
        {
            var rb = LookupDataBlock(dataBid);
            int read;
            byte[] buffer = ReadAndDecompress(fs, rb, out read);

            if (rb.IsInternal)
            {
                // XBlock and XXBlock structures are very similar, we can use the same layout for both
                // Unicode and ANSI structures are also close, enabling us to use the same layout for them, too
                var xb = Map.MapType<XBLOCKUnicode>(buffer, 0);

                if (IsUnicode)
                {
                    var rgbid = Map.MapArray<UInt64>(buffer, Marshal.SizeOf(typeof(XBLOCKUnicode)), xb.cEnt);

                    for (int i = 0; i < rgbid.Length; i++)
                    {
                        // Recurse. XBlock and XXBlock can have common handling
                        // Pass what we know here of the total length through, so that it can be returned on the first block
                        foreach (var buf in ReadDataBlocksInternal(fs, rgbid[i], totalLength != 0 ? totalLength : xb.lcbTotal))
                            yield return buf;
                    }
                }
                else
                {
                    // The ANSI difference is that IDs are only 32 bits
                    var rgbid = Map.MapArray<UInt32>(buffer, Marshal.SizeOf(typeof(XBLOCKUnicode)), xb.cEnt);

                    for (int i = 0; i < rgbid.Length; i++)
                    {
                        // Recurse. XBlock and XXBlock can have common handling
                        // Pass what we know here of the total length through, so that it can be returned on the first block
                        foreach (var buf in ReadDataBlocksInternal(fs, rgbid[i], totalLength != 0 ? totalLength : xb.lcbTotal))
                            yield return buf;
                    }
                }
            }
            else
            {
                // Key for cyclic algorithm is the low 32 bits of the BID, so supply it in case it's needed
                Decrypt(ref buffer, (UInt32)(dataBid & 0xffff));

                yield return buffer;
            }
        }

        // Return all the data blocks referenced by a particular data Id, concatenating them into a single buffer
        private byte[] ReadDataBlockInternal(FileStream fs, UInt64 dataBid, ref int offset, byte[] buffer = null)
        {
            bool first = (buffer == null);  // Remember if we're at the top of a potential recursion
            var rb = LookupDataBlock(dataBid);
            if (rb == null)
                throw new XstException("Data block does not exist");
            if (first)
                offset = 0;

            // First guy in allocates enough to hold the initial block
            // This is either the one and only block, or gets replaced when we find out how much data there is in total
            int read;
            buffer = ReadAndDecompress(fs, rb, out read, buffer, offset);

            if (rb.IsInternal)
            {
                // XBlock and XXBlock structures are very similar, we can use the same layout for both
                // Unicode and ANSI structures are also close, enabling us to use the same layout for them, too
                var xb = Map.MapType<XBLOCKUnicode>(buffer, offset);

                if (IsUnicode)
                {
                    var rgbid = Map.MapArray<UInt64>(buffer, offset + Marshal.SizeOf(typeof(XBLOCKUnicode)), xb.cEnt);

                    if (first)
                    {
                        // First block, allocate a buffer big enough to hold all the data
                        buffer = new byte[xb.lcbTotal];
                        offset = 0;
                    }

                    for (int i = 0; i < rgbid.Length; i++)
                    {
                        // Recurse. XBlock and XXBlock can have common handling
                        ReadDataBlockInternal(fs, rgbid[i], ref offset, buffer);
                    }
                }
                else
                {
                    // The ANSI difference is that IDs are only 32 bits 
                    var rgbid = Map.MapArray<UInt32>(buffer, offset + Marshal.SizeOf(typeof(XBLOCKUnicode)), xb.cEnt);

                    if (first)
                    {
                        // First block, allocate a buffer big enough to hold all the data
                        buffer = new byte[xb.lcbTotal];
                        offset = 0;
                    }

                    for (int i = 0; i < rgbid.Length; i++)
                    {
                        // Recurse. XBlock and XXBlock can have common handling
                        ReadDataBlockInternal(fs, rgbid[i], ref offset, buffer);
                    }
                }
            }
            else
            {
                // Key for cyclic algorithm is the low 32 bits of the BID
                // Assume that this means the BID of each block, rather than all using the BID from the head of the tree
                Decrypt(ref buffer, (UInt32)(dataBid & 0xffff), offset, read);

                // Increment the offset by the length of the real data that we have read
                offset += read;
            }

            if (first)
            {
                // The recursion is over, check the results
                if (offset != buffer.Length)
                    throw new XstException("Data xblock length mismatch");

                return buffer;
            }
            else
                return null;  // Value only returned from the top level guy
        }

        private byte[] ReadAndDecompress(FileStream fs, DataRef rb, out int read, byte[] buffer = null, int offset = 0)
        {
            if (rb == null)
                throw new XstException("Data block does not exist");

            if (IsUnicode4K && rb.Length != rb.InflatedLength)
            {
                fs.Seek((long)rb.Offset, SeekOrigin.Begin);

                // The first two bytes are a zlib header which DeflateStream does not understand
                // They should be 0x789c, the magic code for default compression
                if (fs.ReadByte() != 0x78 || fs.ReadByte() != 0x9c)
                    throw new XstException("Unexpected header in compressed data stream");

                using (DeflateStream decompressionStream = new DeflateStream(fs, CompressionMode.Decompress, true))
                {
                    if (buffer == null)
                        buffer = new byte[rb.InflatedLength];

                    decompressionStream.Read(buffer, offset, rb.InflatedLength);
                }
                read = rb.InflatedLength;
            }
            else
            {
                if (buffer == null)
                    buffer = new byte[rb.Length];
                fs.Seek((long)rb.Offset, SeekOrigin.Begin);
                fs.Read(buffer, offset, rb.Length);
                read = rb.Length;
            }

            // To actually get the block trailer, we would need to skip to the next 64 byte boundary, minus size of trailer
            //var t = Map.ReadType<BLOCKTRAILERUnicode>(fs);

            return buffer;
        }

        // When a data block has a subnode, it can be a simple node, or a two-level tree
        // This reads a sub node and builds suitable data structures, so that we can later access data held in it
        private void ReadSubNodeBtreeUnicode(FileStream fs, UInt64 subDataBid, TreeIntermediate parent)
        {
            var rb = LookupDataBlock(subDataBid);
            if (rb == null)
                throw new XstException("SubNode data block does not exist");

            int read;
            byte[] buffer = ReadAndDecompress(fs, rb, out read);
            var sl = Map.MapType<SLBLOCKUnicode>(buffer);

            if (sl.cLevel > 0)
            {
                var rgbid = Map.MapArray<SIENTRYUnicode>(buffer, Marshal.SizeOf(typeof(SLBLOCKUnicode)), sl.cEnt);

                foreach (var sie in rgbid)
                {
                    var inter = new TreeIntermediate { Key = (sie.nid & 0xffffffff) };
                    parent.Children.Add(inter);

                    // Read child nodes in tree
                    ReadSubNodeBtreeUnicode(fs, sie.bid, inter);
                }
            }
            else
            {
                var rgbid = Map.MapArray<SLENTRYUnicode>(buffer, Marshal.SizeOf(typeof(SLBLOCKUnicode)), sl.cEnt);

                foreach (var sle in rgbid)
                {
                    // Only use low order dword of nid
                    var nb = new Node { Key = (sle.nid & 0xffffffff), DataBid = sle.bidData, SubDataBid = sle.bidSub };
                    parent.Children.Add(nb);
                }
            }
        }

        private void ReadSubNodeBtreeANSI(FileStream fs, UInt64 subDataBid, TreeIntermediate parent)
        {
            var rb = LookupDataBlock(subDataBid);
            if (rb == null)
                throw new XstException("SubNode data block does not exist");

            int read;
            byte[] buffer = ReadAndDecompress(fs, rb, out read);
            var sl = Map.MapType<SLBLOCKANSI>(buffer);

            if (sl.cLevel > 0)
            {
                var rgbid = Map.MapArray<SIENTRYANSI>(buffer, Marshal.SizeOf(typeof(SLBLOCKANSI)), sl.cEnt);

                foreach (var sie in rgbid)
                {
                    var inter = new TreeIntermediate { Key = (sie.nid & 0xffffffff) };
                    parent.Children.Add(inter);

                    // Read child nodes in tree
                    ReadSubNodeBtreeANSI(fs, sie.bid, inter);
                }
            }
            else
            {
                var rgbid = Map.MapArray<SLENTRYANSI>(buffer, Marshal.SizeOf(typeof(SLBLOCKANSI)), sl.cEnt);

                foreach (var sle in rgbid)
                {
                    var nb = new Node { Key = sle.nid, DataBid = sle.bidData, SubDataBid = sle.bidSub };
                    parent.Children.Add(nb);
                }
            }
        }
        #endregion
    }
}
