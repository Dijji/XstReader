using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XstReader.MsgKit.Enums;

namespace XstReader.MsgKit.Streams
{
    /// <summary>
    ///     Represents one item in the <see cref="EntryStream"/> stream
    /// </summary>
    internal sealed class EntryStreamItem
    {
        #region Properties
        /// <summary>
        ///     Name Identifier/String Offset (4 bytes): If this property is a numerical named property (as specified by
        ///     the Property Kind subfield of the Index and Kind Information field), this value is the LID part of the
        ///     PropertyName structure, as specified in [MS-OXCDATA] section 2.6.1. If this property is a string named
        ///     property, this value is the offset in bytes into the strings stream where the value of the Name field of
        ///     the PropertyName structure is located.
        /// </summary>
        public uint NameIdentifierOrStringOffset { get; }

        /// <summary>
        /// 
        /// </summary>
        public string NameIdentifierOrStringOffsetHex { get; }

        /// <summary>
        ///     The following structure specifies the stream indexes and whether the property is a numerical named
        ///     property or a string named property
        /// </summary>
        public IndexAndKindInformation IndexAndKindInformation { get; }
        #endregion

        #region Constructors
        /// <summary>
        ///     Creates this object and reads all the properties from the given <paramref name="binaryReader" />
        /// </summary>
        /// <param name="binaryReader">The <see cref="BinaryReader"/></param>
        internal EntryStreamItem(BinaryReader binaryReader)
        {
            NameIdentifierOrStringOffset = binaryReader.ReadUInt16();
            NameIdentifierOrStringOffsetHex = $"{NameIdentifierOrStringOffset:X}";
            IndexAndKindInformation = new IndexAndKindInformation(binaryReader);
        }

        /// <summary>
        ///     Creates this object and sets all it's needed properties
        /// </summary>
        /// <param name="nameIdentifierOrStringOffset"><see cref="NameIdentifierOrStringOffset"/></param>
        /// <param name="indexAndKindInformation"><see cref="IndexAndKindInformation"/></param>
        internal EntryStreamItem(ushort nameIdentifierOrStringOffset,
                                 IndexAndKindInformation indexAndKindInformation)
        {
            NameIdentifierOrStringOffset = nameIdentifierOrStringOffset;
            NameIdentifierOrStringOffsetHex = $"{nameIdentifierOrStringOffset:X}";
            IndexAndKindInformation = indexAndKindInformation;
        }
        #endregion

        #region Write
        /// <summary>
        ///     Writes all the internal properties to the given <paramref name="binaryWriter" />
        /// </summary>
        /// <param name="binaryWriter"></param>
        internal void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(NameIdentifierOrStringOffset);
            binaryWriter.Write((ushort)((IndexAndKindInformation.GuidIndex << 1) | (ushort)0x00));
            binaryWriter.Write(IndexAndKindInformation.PropertyIndex); //Doesn't seem to be the case in the spec. 
            // Fortunately section 3.2 clears this up. 
        }
        #endregion
    }

    /// <summary>
    ///     2.2.3.1.2.1 Index and Kind Information
    ///     The following structure specifies the stream indexes and whether the property is a numerical named
    ///     property or a string named property.
    /// </summary>
    internal sealed class IndexAndKindInformation
    {
        #region Properties
        /// <summary>
        ///     Sequentially increasing, zero-based index. This MUST be 0 for the first
        ///     named property, 1 for the second, and so on.
        /// </summary>
        public UInt16 PropertyIndex { get; }

        /// <summary>
        ///     Index into the GUID stream. The possible values are shown in the following table.<br/>
        ///     - 1 Always use the PS_MAPI property set, as specified in [MS-OXPROPS] section 1.3.2. No GUID is stored in<br/>
        ///         the GUID stream.<br/>
        ///     - 2 Always use the PS_PUBLIC_STRINGS property set, as specified in [MS-OXPROPS]<br/>
        ///         section 1.3.2. No GUID is stored in the GUID stream.<br/>
        ///     - >= 3 Use Value minus 3 as the index of the GUID into the GUID stream.For example, if the GUID index is 5,<br/>
        ///         the third GUID(5 minus 3, resulting in a zero-based index of 2) is used as the GUID for the name<br/>
        ///         property being derived.
        /// </summary>
        public UInt16 GuidIndex { get; }

        #endregion

        #region GetUIntFromBitArray
        /// <summary>
        /// Converts the given <paramref name="bitArray"/> to an unsigned integer
        /// </summary>
        /// <param name="bitArray"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static uint GetUIntFromBitArray(BitArray bitArray, int offset, int count)
        {
            uint value = 0;

            for (var i = offset; i < count + offset; i++)
            {
                if (bitArray[i])
                    value += Convert.ToUInt16(Math.Pow(2, i));
            }

            return value;
        }
        #endregion

        #region Constructors
        /// <summary>
        ///     Creates this object and reads all the properties from the given <paramref name="binaryReader" />
        /// </summary>
        /// <param name="binaryReader">The <see cref="BinaryReader"/></param>
        internal IndexAndKindInformation(BinaryReader binaryReader)
        {
            PropertyIndex = binaryReader.ReadUInt16();
            var bits = new BitArray(binaryReader.ReadBytes(2));
            GuidIndex = (ushort)GetUIntFromBitArray(bits, 1, 15);
        }

        /// <summary>
        ///     Creates this object and sets all it's needed properties
        /// </summary>
        /// <param name="propertyIndex"><see cref="PropertyIndex"/></param>
        /// <param name="guidIndex"><see cref="GuidIndex"/></param>
        internal IndexAndKindInformation(ushort propertyIndex,
                                         ushort guidIndex)
        {
            PropertyIndex = propertyIndex;
            GuidIndex = guidIndex;
        }
        #endregion

        #region Write
        /// <summary>
        ///     Writes all the internal properties to the given <paramref name="binaryWriter" />
        /// </summary>
        /// <param name="binaryWriter"></param>
        internal void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(PropertyIndex);
            binaryWriter.Write(GuidIndex + (uint)0x00);
        }
        #endregion
    }
}
