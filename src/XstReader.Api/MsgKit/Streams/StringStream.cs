using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenMcdf;
using XstReader.MsgKit.Helpers;

namespace XstReader.MsgKit.Streams
{
    /// <summary>
    ///     The string stream MUST be named "__substg1.0_00040102". It MUST consist of one entry for each
    ///     string named property, and all entries MUST be arranged consecutively, like in an array.
    ///     As specified in section 2.2.3.1.2, the offset, in bytes, to use for a particular property is stored in the
    ///     corresponding entry in the entry stream.That is a byte offset into the string stream from where the
    ///     entry for the property can be read.The strings MUST NOT be null-terminated. Implementers can add a
    ///     terminating null character to the string
    /// </summary>
    /// <remarks>
    ///     See https://msdn.microsoft.com/en-us/library/ee124409(v=exchg.80).aspx
    /// </remarks>
    internal sealed class StringStream : List<StringStreamItem>
    {
        #region Constructors
        /// <summary>
        ///     Creates this object
        /// </summary>
        internal StringStream()
        {

        }

        /// <summary>
        ///     Creates this object and reads all the <see cref="StringStreamItem" /> objects 
        ///     from the given <paramref name="storage"/>
        /// </summary>
        /// <param name="storage">The <see cref="CFStorage"/> that contains the <see cref="PropertyTags.EntryStream"/></param>
        internal StringStream(CFStorage storage)
        {
            var stream = storage.GetStream(PropertyTags.StringStream);
            using (var memoryStream = new MemoryStream(stream.GetData()))
            using (var binaryReader = new BinaryReader(memoryStream))
                while (!binaryReader.Eos())
                {
                    var stringStreamItem = new StringStreamItem(binaryReader);
                    Add(stringStreamItem);
                }
        }
        #endregion

        #region Write
        /// <summary>
        ///     Writes all the <see cref="StringStream"/>'s as a <see cref="CFStream" /> to the
        ///     given <paramref name="storage" />
        /// </summary>
        /// <param name="storage">The <see cref="CFStorage" /></param>
        internal void Write(CFStorage storage)
        {
            var stream = storage.GetStream(PropertyTags.StringStream);
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                foreach (var stringStreamItem in this)
                    stringStreamItem.Write(binaryWriter);

                stream.SetData(memoryStream.ToArray());
            }
        }
        #endregion
    }
}
