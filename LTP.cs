// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace XstReader
{
    /// <summary>
    /// This class implements the LTP (Lists, Tables and Properties) layer, which provides higher-level concepts 
    /// on top of the NDB, notably the Property Context (PC) and Table Context (TC)
    /// </summary>
    class LTP
    {
        // A heap-on-node data block
        private class HNDataBlock
        {
            public int Index;
            public byte[] Buffer;
            public UInt16[] rgibAlloc;

            // In first block only
            public EbType bClientSig;
            public HID hidUserRoot;  // HID that points to the User Root record
        }

        // Used when reading table data to normalise handling of in-line and sub node data storage
        private class RowDataBlock
        {
            public byte[] Buffer;
            public int Offset;
            public int Length;
        }

        private NDB ndb;


        #region Public methods

        public LTP(NDB ndb)
        {
            this.ndb = ndb;
        }

        // Read the properties in a property context
        // T is the type of the object to be populated
        // Property getters must be supplied to map property Ids to members of T
        // The returned value is the subnode tree, if any, related to the contents of the properties
        //
        // First form takes a node ID for a node in the main node tree
        public BTree<Node> ReadProperties<T>(FileStream fs, NID nid, PropertyGetters<T> g, T target)
        {
            BTree<Node> subNodeTree;
            var rn = ndb.LookupNodeAndReadItsSubNodeBtree(fs, nid, out subNodeTree);

            return ReadPropertiesInternal<T>(fs, subNodeTree, rn.DataBid, g, target);
        }

        // Second form takes a node ID for a node in the supplied sub node tree
        public BTree<Node> ReadProperties<T>(FileStream fs, BTree<Node> subNodeTree, NID nid, PropertyGetters<T> g, T target)
        {
            BTree<Node> childSubNodeTree;
            var rn = ndb.LookupSubNodeAndReadItsSubNodeBtree(fs, subNodeTree, nid, out childSubNodeTree);
            if (rn == null)
                throw new Exception("Node block does not exist");

            ReadPropertiesInternal<T>(fs, subNodeTree, rn.DataBid, g, target);
            return childSubNodeTree;
        }

        // This is a cutdown version of the table reader to fetch subfolder NIDs from the hierarchy table of a folder,
        // avoiding the overhead of reading the data rows when scanning the folder tree
        public IEnumerable<NID> ReadTableRowIds(FileStream fs, NID nid)
        {
            var blocks = ReadHeapOnNode(fs, nid);
            var h = blocks.First();
            if (h.bClientSig != EbType.bTypeTC)
                throw new Exception("Was expecting a table");

            // Read the table information
            var t = MapType<TCINFO>(blocks, h.hidUserRoot);

            // Read the row index and return the NIDs from it
            if (ndb.IsUnicode)
                return ReadBTHIndex<TCROWIDUnicode>(blocks, t.hidRowIndex).Select(r => new NID(r.dwRowID));
            else
                return ReadBTHIndex<TCROWIDANSI>(blocks, t.hidRowIndex).Select(r => new NID(r.dwRowID));
        }

        // This is the full-scale table reader, that reads all of the data in a table
        // T is the type of the row object to be populated
        // Property getters must be supplied to map property IDs to members of T
        //
        // First form takes a node ID for a node in the main node tree
        public IEnumerable<T> ReadTable<T>(FileStream fs, NID nid, PropertyGetters<T> g, Action<T, UInt32> idGetter = null) where T : new()
        {
            BTree<Node> subNodeTree;
            var rn = ndb.LookupNodeAndReadItsSubNodeBtree(fs, nid, out subNodeTree);

            return ReadTableInternal<T>(fs, subNodeTree, rn.DataBid, g, idGetter);
        }

        // Second form takes a node ID for a node in the supplied sub node tree
        public IEnumerable<T> ReadTable<T>(FileStream fs, BTree<Node> subNodeTree, NID nid, PropertyGetters<T> g, Action<T, UInt32> idGetter = null) where T : new()
        {
            var rn = NDB.LookupSubNode(subNodeTree, nid);
            if (rn == null)
                throw new Exception("Node block does not exist");

            return ReadTableInternal<T>(fs, subNodeTree, rn.DataBid, g, idGetter);
        }
        #endregion

        #region Private methods

        // Common implementation of property reading takes a data ID for a block in the main block tree
        private BTree<Node> ReadPropertiesInternal<T>(FileStream fs, BTree<Node> subNodeTree, UInt64 dataBid, PropertyGetters<T> g, T target)
        {
            var blocks = ReadHeapOnNode(fs, dataBid);
            var h = blocks.First();
            if (h.bClientSig != EbType.bTypePC)
                throw new Exception("Was expecting a PC");

            // Read the index of properties
            var props = ReadBTHIndex<PCBTH>(blocks, h.hidUserRoot).ToArray();

            foreach (var prop in props)
            {
                if (!g.ContainsKey(prop.wPropId))
                    continue;

                dynamic val = null;
                byte[] buf = null;

                switch (prop.wPropType)
                {
                    case EpropertyType.PtypInteger32:
                        val = prop.dwValueHnid.dwValue;
                        break;

                    case EpropertyType.PtypBoolean:
                        val = (prop.dwValueHnid.dwValue == 0x01);
                        break;

                    case EpropertyType.PtypBinary:
                        if (prop.dwValueHnid.HasValue && prop.dwValueHnid.hidType != EnidType.HID && prop.wPropId == EpropertyTag.PidTagAttachDataBinary)
                        {
                            // Special case for out of line attachment contents: don't dereference to binary yet
                            val = prop.dwValueHnid.NID;
                        }
                        else
                        {
                            buf = GetBytesForHNID(fs, blocks, subNodeTree, prop.dwValueHnid);

                            if (buf == null)
                                val = "<Could not read Binary value>";
                            else
                                val = buf;
                        }
                        break;

                    case EpropertyType.PtypString: // Unicode string
                        buf = GetBytesForHNID(fs, blocks, subNodeTree, prop.dwValueHnid);

                        if (buf == null)
                            val = "<Could not read string value>";
                        else
                        {
                            val = Encoding.Unicode.GetString(buf, 0, buf.Length);
                        }
                        break;

                    case EpropertyType.PtypString8:  // Multipoint string in variable encoding
                        buf = GetBytesForHNID(fs, blocks, subNodeTree, prop.dwValueHnid);

                        if (buf == null)
                            val = "<Could not read string value>";
                        else
                            val = Encoding.UTF8.GetString(buf, 0, buf.Length);
                        break;

                    case EpropertyType.PtypTime:
                        // In a Property Context, time values are references to data
                        buf = GetBytesForHNID(fs, blocks, subNodeTree, prop.dwValueHnid);

                        if (buf != null)
                        {
                            var fileTime = Map.MapType<Int64>(buf);
                            val = DateTime.FromFileTimeUtc(fileTime);
                        }
                        break;

                    default:
                        val = null;
                        break;
                }

                g[prop.wPropId](target, val);
            }
            return subNodeTree;
        }

        // Common implementation of table reading takes a data ID for a block in the main block tree
        private IEnumerable<T> ReadTableInternal<T>(FileStream fs, BTree<Node> subNodeTree, UInt64 dataBid, PropertyGetters<T> g, Action<T, UInt32> idGetter = null) where T : new()
        {
            var blocks = ReadHeapOnNode(fs, dataBid);
            var h = blocks.First();
            if (h.bClientSig != EbType.bTypeTC)
                throw new Exception("Was expecting a table");

            // Read the table information
            var t = MapType<TCINFO>(blocks, h.hidUserRoot);

            // Read the column descriptions
            var cols = MapArray<TCOLDESC>(blocks, h.hidUserRoot, t.cCols, Marshal.SizeOf(typeof(TCINFO)));

            // Read the row index
            TCROWIDUnicode[] indexes;
            if (ndb.IsUnicode)
                indexes = ReadBTHIndex<TCROWIDUnicode>(blocks, t.hidRowIndex).ToArray();
            else
                // For ANSI, convert the index entries to the slightly more capacious Unicode equivalents
                indexes = ReadBTHIndex<TCROWIDANSI>(blocks, t.hidRowIndex).Select(e => new TCROWIDUnicode { dwRowID = e.dwRowID, dwRowIndex = e.dwRowIndex }).ToArray();

            // Work out which of the columns are both present in the table and have getters defined
            var colsToGet = cols.Where(c => g.ContainsKey(c.wPropId)).ToList();

            // The data rows may be held in line, or in a sub node
            if (t.hnidRows.IsHID)
            {
                // Data is in line
                var buf = GetBytesForHNID(fs, blocks, subNodeTree, t.hnidRows);
                var dataBlocks = new List<RowDataBlock>
                {
                    new RowDataBlock
                    {
                        Buffer = buf,
                        Offset = 0,
                        Length = buf.Length,
                    }
                };
                return ReadTableData<T>(fs, t, blocks, dataBlocks, colsToGet, subNodeTree, indexes, g, idGetter);
            }
            else
            {
                // Don't use GetBytesForHNID in this case, as we need to handle multiple blocks
                var dataBlocks = ReadSubNodeRowDataBlocks(fs, subNodeTree, t.hnidRows.NID);
                return ReadTableData<T>(fs, t, blocks, dataBlocks, colsToGet, subNodeTree, indexes, g, idGetter);
            }
        }

        // Read the data rows of a table, populating the members of target type T as specified by the supplied property getters
        private IEnumerable<T> ReadTableData<T>(FileStream fs, TCINFO t, List<HNDataBlock> blocks, List<RowDataBlock> dataBlocks, List<TCOLDESC> colsToGet,
             BTree<Node> subNodeTree, TCROWIDUnicode[] indexes, PropertyGetters<T> g, Action<T, UInt32> idGetter = null) where T : new()
        {
            int rgCEBSize = (int)Math.Ceiling((decimal)t.cCols / 8);
            int rowsPerBlock;
            if (ndb.IsUnicode)
                rowsPerBlock = (ndb.BlockSize - Marshal.SizeOf(typeof(BLOCKTRAILERUnicode))) / t.rgibTCI_bm;
            else
                rowsPerBlock = (ndb.BlockSize - Marshal.SizeOf(typeof(BLOCKTRAILERANSI))) / t.rgibTCI_bm;

            foreach (var index in indexes)
            {
                int blockNum = (int)(index.dwRowIndex / rowsPerBlock);
                if (blockNum >= dataBlocks.Count)
                    throw new Exception("Data block number out of bounds");

                var db = dataBlocks[blockNum];

                long rowOffset = db.Offset + (index.dwRowIndex % rowsPerBlock) * t.rgibTCI_bm;
                T row = new T();

                // Retrieve the node ID that accesses the message
                if (idGetter != null)
                    idGetter(row, index.dwRowID);

                if (rowOffset + t.rgibTCI_bm > db.Offset + db.Length)
                {
                    throw new Exception("Out of bounds reading table data");
                }

                // Read the column existence data
                var rgCEB = Map.MapArray<Byte>(db.Buffer, (int)(rowOffset + t.rgibTCI_1b), rgCEBSize);

                foreach (var col in colsToGet)
                {
                    // Check if the column exists
                    if ((rgCEB[col.iBit / 8] & (0x01 << (7 - (col.iBit % 8)))) == 0)
                        continue;

                    dynamic val = null;
                    HNID hnid;

                    switch (col.wPropType)
                    {
                        case EpropertyType.PtypInteger32:
                            if (col.cbData != 4)
                                throw new Exception("Unexpected property length");
                            val = Map.MapType<Int32>(db.Buffer, (int)rowOffset + col.ibData);
                            break;

                        case EpropertyType.PtypBoolean:
                            val.Value = (db.Buffer[rowOffset + col.ibData] == 0x01);
                            break;

                        case EpropertyType.PtypString:  // Unicode string
                            if (col.cbData != 4)
                                throw new Exception("Unexpected property length");
                            hnid = Map.MapType<HNID>(db.Buffer, (int)rowOffset + col.ibData);

                            if (!hnid.HasValue)
                                val = "";
                            else
                            {
                                var buf = GetBytesForHNID(fs, blocks, subNodeTree, hnid);

                                if (buf == null)
                                    val = "<Could not read string value>";
                                else
                                {
                                    int skip = 0;
                                    if (col.wPropId == EpropertyTag.PidTagSubjectW)
                                        if (buf[0] == 0x01 && buf[1] == 0x00)  // Unicode 0x01
                                            skip = 4;
                                    val = Encoding.Unicode.GetString(buf, skip, buf.Length - skip);
                                }
                            }
                            if (val == "" && col.wPropId == EpropertyTag.PidTagSubjectW)
                                val = "<No subject>";
                            break;

                        case EpropertyType.PtypString8: // Multibyte string in variable encoding
                            if (col.cbData != 4)
                                throw new Exception("Unexpected property length");
                            hnid = Map.MapType<HNID>(db.Buffer, (int)rowOffset + col.ibData);

                            if (!hnid.HasValue)
                                val = "";
                            else
                            {
                                var buf = GetBytesForHNID(fs, blocks, subNodeTree, hnid);

                                if (buf == null)
                                    val = "<Could not read string value>";
                                else
                                {
                                    int skip = 0;

                                    if (col.wPropId == EpropertyTag.PidTagSubjectW)
                                        if (buf[0] == 0x01)  // ANSI 0x01
                                            skip = 2;
                                    val = Encoding.UTF8.GetString(buf, skip, buf.Length - skip);
                                }
                            }
                            if (val == "" && col.wPropId == EpropertyTag.PidTagSubjectW)
                                val = "<No subject>";
                            break;

                        case EpropertyType.PtypTime:
                            // In a Table Context, time values are held in line
                            if (col.cbData != 8)
                                throw new Exception("Unexpected property length");
                            var fileTime = Map.MapType<Int64>(db.Buffer, (int)rowOffset + col.ibData);
                            try
                            {
                                val = DateTime.FromFileTimeUtc(fileTime);
                            }
                            catch (System.ArgumentOutOfRangeException)
                            {
                                val = null;
                            }
                            break;

                        default:
                            val = null;
                            break;
                    }
                    g[col.wPropId](row, val);
                }
                yield return row;
            }
            yield break; // No more entries
        }


        // Walk a b-tree implemented on a heap, and return all the type T entries
        // This is used when reading the index of properties in a PC (property context), or rows in a TC (table context)
        private IEnumerable<T> ReadBTHIndex<T>(List<HNDataBlock> blocks, HID hid)
        {
            var b = MapType<BTHHEADER>(blocks, hid);
            if (b.bIdxLevels == 0)
            {
                int recCount = HidSize(blocks, b.hidRoot) / Marshal.SizeOf(typeof(T));
                if (b.hidRoot.hidIndex != 0)
                {
                    // The T record also forms the key of the BTH entry
                    foreach (var row in MapArray<T>(blocks, b.hidRoot, recCount))
                        yield return row;
                }
            }
            else
            {
                int recCount = HidSize(blocks, b.hidRoot) / Marshal.SizeOf(typeof(IntermediateBTH4));
                var inters = MapArray<IntermediateBTH4>(blocks, b.hidRoot, recCount);

                foreach (var inter in inters)
                {
                    int count = HidSize(blocks, inter.hidNextLevel) / Marshal.SizeOf(typeof(T));
                    foreach (var row in MapArray<T>(blocks, inter.hidNextLevel, count))
                        yield return row;
                }
            }
            yield break; // No more entries
        }


        // Read all of the data blocks for a table, in the case where the rows are to be accessed via a sub node
        // The variation here is that for reading rows, we need to retain the block structure, so we return a set of blocks
        private List<RowDataBlock> ReadSubNodeRowDataBlocks(FileStream fs, BTree<Node> subNodeTree, NID nid)
        {
            var blocks = new List<RowDataBlock>();
            var n = NDB.LookupSubNode(subNodeTree, nid);
            if (n == null)
                throw new Exception("Sub node NID not found");
            if (n.SubDataBid != 0)
                throw new Exception("Sub-nodes of sub-nodes not yet implemented");

            foreach (var buf in ndb.ReadDataBlocks(fs, n.DataBid))
            {
                blocks.Add(new RowDataBlock
                {
                    Buffer = buf,
                    Offset = 0,
                    Length = buf.Length,
                });
            }

            return blocks;
        }

        // Read a heap on node data structure referenced by another node
        private List<HNDataBlock> ReadHeapOnNode(FileStream fs, NID nid)
        {
            var rn = ndb.LookupNode(nid);
            if (rn == null)
                throw new Exception("Node block does not exist");
            return ReadHeapOnNode(fs, rn.DataBid);
        }

        // Read a heap on node data structure. The division of data into blocks is preserved,
        // because references into it have two parts: block index, and offset within block
        private List<HNDataBlock> ReadHeapOnNode(FileStream fs, UInt64 dataBid) 
        {
            var blocks = new List<HNDataBlock>();

            int index = 0;
            foreach (var buf in ndb.ReadDataBlocks(fs, dataBid))
            {
                // First block contains a HNHDR
                if (index == 0)
                {
                    var h = Map.MapType<HNHDR>(buf, 0);
                    var pm = Map.MapType<HNPAGEMAP>(buf, h.ibHnpm);
                    var b = new HNDataBlock
                    {
                        Index = index,
                        Buffer = buf,
                        bClientSig = h.bClientSig,
                        hidUserRoot = h.hidUserRoot,
                        rgibAlloc = Map.MapArray<UInt16>(buf, h.ibHnpm + Marshal.SizeOf(pm), pm.cAlloc + 1),  //+1 to get the dummy entry that gives us the size of the last one
                    };
                    blocks.Add(b);
                }
                // Blocks 8, 136, then every 128th contains a HNBITMAPHDR
                else if (index == 8 || (index >= 136 && (index - 8) % 128 == 0))
                {
                    var h = Map.MapType<HNBITMAPHDR>(buf, 0);
                    var pm = Map.MapType<HNPAGEMAP>(buf, h.ibHnpm);
                    var b = new HNDataBlock
                    {
                        Index = index,
                        Buffer = buf,
                        rgibAlloc = Map.MapArray<UInt16>(buf, h.ibHnpm + Marshal.SizeOf(pm), pm.cAlloc + 1),  //+1 to get the dummy entry that gives us the size of the last one
                    };
                    blocks.Add(b);
                }
                // All other blocks contain a HNPAGEHDR
                else
                {
                    var h = Map.MapType<HNPAGEHDR>(buf, 0);
                    var pm = Map.MapType<HNPAGEMAP>(buf, h.ibHnpm);
                    var b = new HNDataBlock
                    {
                        Index = index,
                        Buffer = buf,
                        rgibAlloc = Map.MapArray<UInt16>(buf, h.ibHnpm + Marshal.SizeOf(pm), pm.cAlloc + 1),  //+1 to get the dummy entry that gives us the size of the last one
                    };
                    blocks.Add(b);
                }
                index++;
            }
            return blocks;
        }

        // Used in reading property contexts and table contexts to get a data value which might be held either on the local heap, or in a sub node
        // The value is returned as a byte array: the caller should convert it to a specific type if required
        private byte[] GetBytesForHNID(FileStream fs, List<HNDataBlock> blocks, BTree<Node> subNodeTree, HNID hnid)
        {
            byte[] buf = null;

            if (hnid.hidType == EnidType.HID)
            {
                if (hnid.hidIndex != 0)
                {
                    buf = MapArray<byte>(blocks, hnid.HID, HidSize(blocks, hnid.HID));
                }
            }
            else if (hnid.nidType == EnidType.LTP)
            {
                buf = ndb.ReadSubNodeDataBlock(fs, subNodeTree, hnid.NID);
            }
            else
                throw new Exception("Data storage style not implemented");

            return buf;
        }

        // Dereference the supplied HID in the supplied heap-on-node blocks,
        // and return the size of the resulting data buffer
        private static int HidSize(List<HNDataBlock> blocks, HID hid)
        {
            if (hid.hidIndex == 0) // Check for empty
                return 0;
            var b = blocks[hid.hidBlockIndex];
            return b.rgibAlloc[hid.hidIndex] - b.rgibAlloc[hid.hidIndex - 1];
        }

        // Dereference the supplied HID in the supplied heap-on-node blocks,
        // and map the resulting data buffer onto the specified type T
        private static T MapType<T>(List<HNDataBlock> blocks, HID hid, int offset = 0)
        {
            var b = blocks[hid.hidBlockIndex];
            return Map.MapType<T>(b.Buffer, b.rgibAlloc[hid.hidIndex - 1] + offset);
        }

        // Dereference the supplied HID in the supplied heap-on-node blocks,
        // and map the resulting data buffer onto an array of count occurrences of the specified type T
        private static T[] MapArray<T>(List<HNDataBlock> blocks, HID hid, int count, int offset = 0)
        {
            var b = blocks[hid.hidBlockIndex];
            return Map.MapArray<T>(b.Buffer, b.rgibAlloc[hid.hidIndex - 1] + offset, count);
        }
        #endregion
    }
}
