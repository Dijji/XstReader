//
// TopLevelProperties.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright (c) 2015-2021 Magic-Sessions. (www.magic-sessions.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using OpenMcdf;
using System;
using System.IO;
using XstReader.MsgKit.Structures;

namespace XstReader.MsgKit.Streams
{
    /// <summary>
    ///     The properties stream contained inside the top level of the .msg file, which represents the Message object itself.
    /// </summary>
    internal sealed class TopLevelProperties : Properties
    {
        #region Properties
        /// <summary>
        ///     The ID to use for naming the next Recipient object storage if one is created inside the .msg file
        /// </summary>
        internal int NextRecipientId { get; set; }

        /// <summary>
        ///     The ID to use for naming the next Attachment object storage if one is created inside the .msg file
        /// </summary>
        internal int NextAttachmentId { get; set; }

        /// <summary>
        ///     The number of Recipient objects
        /// </summary>
        internal int RecipientCount { get; set; }

        /// <summary>
        ///     The number of Attachment objects
        /// </summary>
        internal int AttachmentCount { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        ///     Create this object and reads all the <see cref="Property">properties</see> from 
        ///     the given <see cref="CFStream"/>
        /// </summary>
        /// <param name="stream">The <see cref="CFStream"/></param>
        internal TopLevelProperties(CFStream stream)
        {
            using (var memoryStream = new MemoryStream(stream.GetData()))
            using (var binaryReader = new BinaryReader(memoryStream))
            {
                binaryReader.ReadBytes(8); // Reserved
                NextRecipientId = Convert.ToInt32(binaryReader.ReadUInt32());
                NextAttachmentId = Convert.ToInt32(binaryReader.ReadUInt32());
                RecipientCount = Convert.ToInt32(binaryReader.ReadUInt32());
                AttachmentCount = Convert.ToInt32(binaryReader.ReadUInt32());
                binaryReader.ReadBytes(8); // Reserved
                ReadProperties(binaryReader);
            }
        }

        /// <summary>
        ///     Creates this object
        /// </summary>
        internal TopLevelProperties()
        {
        }
        #endregion

        #region WriteProperties
        /// <summary>
        ///     Writes all <see cref="Property">properties</see> either as a <see cref="CFStream"/> or as a collection in
        ///     a <see cref="PropertyTags.PropertiesStreamName"/> stream to the given <paramref name="storage"/>, this depends 
        ///     on the <see cref="Enums.PropertyType"/>
        /// </summary>
        /// <remarks>
        ///     See the <see cref="Properties"/> class it's <see cref="Properties.WriteProperties"/> method for the logic
        ///     that is used to determine this
        /// </remarks>
        /// <param name="storage">The <see cref="CFStorage"/></param>
        /// <param name="messageSize">Used to calculate the exact size of the <see cref="Message"/></param>
        /// <returns>
        ///     Total size of the written <see cref="Properties"/>
        /// </returns>
        internal long WriteProperties(CFStorage storage, long? messageSize = null)
        {
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                // Reserved(8 bytes): This field MUST be set to zero when writing a .msg file and MUST be ignored 
                // when reading a.msg file. 
                binaryWriter.Write(new byte[8]);
                // Next Recipient ID(4 bytes): The ID to use for naming the next Recipient object storage if one is 
                // created inside the .msg file. The naming convention to be used is specified in section 2.2.1.If 
                // no Recipient object storages are contained in the.msg file, this field MUST be set to 0.
                binaryWriter.Write(Convert.ToUInt32(NextRecipientId));
                // Next Attachment ID (4 bytes): The ID to use for naming the next Attachment object storage if one 
                // is created inside the .msg file. The naming convention to be used is specified in section 2.2.2.
                // If no Attachment object storages are contained in the.msg file, this field MUST be set to 0.
                binaryWriter.Write(Convert.ToUInt32(NextAttachmentId));
                // Recipient Count(4 bytes): The number of Recipient objects.
                binaryWriter.Write(Convert.ToUInt32(RecipientCount));
                // Attachment Count (4 bytes): The number of Attachment objects.
                binaryWriter.Write(Convert.ToUInt32(AttachmentCount));
                // Reserved(8 bytes): This field MUST be set to 0 when writing a msg file and MUST be ignored when 
                // reading a msg file.
                binaryWriter.Write(new byte[8]);
                return WriteProperties(storage, binaryWriter, messageSize);
            }
        }
        #endregion
    }
}