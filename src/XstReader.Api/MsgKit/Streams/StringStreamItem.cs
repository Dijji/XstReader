using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XstReader.MsgKit.Streams
{
    /// <summary>
    ///     Represents one item in the <see cref="StringStream"/> stream
    /// </summary>
    internal sealed class StringStreamItem
    {
        #region Properties
        /// <summary>
        ///     The length of the following <see cref="Name"/> field in bytes.
        /// </summary>
        public uint Length { get; }

        /// <summary>
        ///     A Unicode string that is the name of the property. A new entry MUST always start
        ///     on a 4 byte boundary; therefore, if the size of the Name field is not an exact multiple of 4, and
        ///     another Name field entry occurs after it, null characters MUST be appended to the stream after it
        ///     until the 4-byte boundary is reached.The Name Length field for the next entry will then start at
        ///     the beginning of the next 4-byte boundary
        /// </summary>
        public string Name { get; }
        #endregion

        #region Constructors
        /// <summary>
        ///     Creates this object and reads all the properties from the given <paramref name="binaryReader" />
        /// </summary>
        /// <param name="binaryReader">The <see cref="BinaryReader"/></param>
        internal StringStreamItem(BinaryReader binaryReader)
        {
            Length = binaryReader.ReadUInt32();
            Name = Encoding.Unicode.GetString(binaryReader.ReadBytes((int)Length)).Trim('\0');
            var boundry = Get4BytesBoundry(Length);
            binaryReader.ReadBytes((int)boundry);
        }

        /// <summary>
        ///     Creates this object and sets all it's needed properties
        /// </summary>
        /// <param name="name"><see cref="Name"/></param>
        internal StringStreamItem(string name)
        {
            Length = (uint)name.Length;
            Name = name;
        }
        #endregion

        #region Write
        /// <summary>
        ///     Writes all the internal properties to the given <paramref name="binaryWriter" />
        /// </summary>
        /// <param name="binaryWriter"></param>
        internal void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Length);
            binaryWriter.Write(Name);
            var boundry = Get4BytesBoundry(Length);
            var bytes = new byte[boundry];
            binaryWriter.Write(bytes);
        }
        #endregion

        #region Get4BytesBoundry
        /// <summary>
        ///     Extract 4 from the given <paramref name="length"/> until the result is smaller
        ///     then 4 and then returns this result;
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static uint Get4BytesBoundry(uint length)
        {
            if (length == 0) return 4;

            while (length >= 4)
                length -= 4;

            return length;
        }
        #endregion
    }

}
