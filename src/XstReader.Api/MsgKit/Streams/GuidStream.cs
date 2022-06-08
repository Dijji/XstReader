using System;
using System.Collections.Generic;
using System.IO;
using XstReader.MsgKit.Helpers;
using OpenMcdf;
using XstReader.MsgKit;

namespace XstReader.MsgKit.Streams
{
    /// <summary>
    ///     The GUID stream MUST be named "__substg1.0_00020102". It MUST store the property set GUID
    ///     part of the property name of all named properties in the Message object or any of its subobjects,
    ///     except for those named properties that have PS_MAPI or PS_PUBLIC_STRINGS, as described in [MSOXPROPS]
    ///     section 1.3.2, as their property set GUID.
    ///     The GUIDs are stored in the stream consecutively like an array. If there are multiple named properties
    ///     that have the same property set GUID, then the GUID is stored only once and all the named
    ///     properties will refer to it by its index
    /// </summary>
    internal sealed class GuidStream : List<Guid>
    {
        #region Constructors
        /// <summary>
        ///     Creates this object
        /// </summary>
        internal GuidStream()
        {

        }

        /// <summary>
        ///     Creates this object and reads all the <see cref="Guid" /> objects from 
        ///     the given <paramref name="storage"/>
        /// </summary>
        /// <param name="storage">The <see cref="CFStorage"/> that containts the <see cref="PropertyTags.GuidStream"/></param>
        internal GuidStream(CFStorage storage)
        {
            var stream = storage.GetStream(PropertyTags.GuidStream);

            using (var memoryStream = new MemoryStream(stream.GetData()))
            using (var binaryReader = new BinaryReader(memoryStream))
                while (!binaryReader.Eos())
                {
                    var guid = new Guid(binaryReader.ReadBytes(16));
                    Add(guid);
                }
        }
        #endregion

        #region Write
        /// <summary>
        ///     Writes all the <see cref="Guid"/>'s as a <see cref="CFStream" /> to the
        ///     given <paramref name="storage" />
        /// </summary>
        /// <param name="storage">The <see cref="CFStorage" /></param>
        internal void Write(CFStorage storage)
        {
            var stream = storage.GetStream(PropertyTags.GuidStream);
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                foreach (var guid in this)
                    binaryWriter.Write(guid.ToByteArray());

                stream.SetData(memoryStream.ToArray());
            }
        }
        #endregion
    }
}