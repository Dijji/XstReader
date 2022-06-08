//
// Entry.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com> and Travis Semple
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using XstReader.MsgKit.Enums;
using XstReader.MsgKit.Helpers;

namespace XstReader.MsgKit.Streams
{
    /// <summary>
    ///     The entry stream MUST be named "__substg1.0_00030102" and consist of 8-byte entries, one for each
    ///     named property being stored. The properties are assigned unique numeric IDs (distinct from any property
    ///     ID assignment) starting from a base of 0x8000. The IDs MUST be numbered consecutively, like an array.
    ///     In this stream, there MUST be exactly one entry for each named property of the Message object or any of
    ///     its subobjects. The index of the entry for a particular ID is calculated by subtracting 0x8000 from it.
    ///     For example, if the ID is 0x8005, the index for the corresponding 8-byte entry would be 0x8005 – 0x8000 = 5.
    ///     The index can then be multiplied by 8 to get the actual byte offset into the stream from where to start
    ///     reading the corresponding entry.
    /// </summary>
    /// <remarks>
    ///     See https://msdn.microsoft.com/en-us/library/ee159689(v=exchg.80).aspx
    /// </remarks>
    internal sealed class EntryStream : List<EntryStreamItem>
    {
        #region Constructors
        /// <summary>
        ///     Creates this object
        /// </summary>
        internal EntryStream()
        {

        }

        /// <summary>
        ///     Creates this object and reads all the <see cref="EntryStreamItem" /> objects from 
        ///     the given <paramref name="storage"/>
        /// </summary>
        /// <param name="storage">The <see cref="CFStorage"/> that containts the <see cref="PropertyTags.EntryStream"/></param>
        internal EntryStream(CFStorage storage)
        {
            if (!storage.TryGetStream(PropertyTags.EntryStream, out var stream))
                stream = storage.AddStream(PropertyTags.EntryStream);

            using (var memoryStream = new MemoryStream(stream.GetData()))
            using (var binaryReader = new BinaryReader(memoryStream))
                while (!binaryReader.Eos())
                {
                    var entryStreamItem = new EntryStreamItem(binaryReader);
                    Add(entryStreamItem);
                }
        }
        #endregion

        #region Write
        /// <summary>
        ///     Writes all the <see cref="EntryStreamItem"/>'s as a <see cref="CFStream" /> to the
        ///     given <paramref name="storage" />
        /// </summary>
        /// <param name="storage">The <see cref="CFStorage" /></param>
        internal void Write(CFStorage storage)
        {
            var stream = storage.GetStream(PropertyTags.EntryStream);
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                foreach (var entryStreamItem in this)
                    entryStreamItem.Write(binaryWriter);

                stream.SetData(memoryStream.ToArray());
            }
        }
        internal void Write(CFStorage storage, string streamName)
        {
            if (!storage.TryGetStream(streamName, out var stream))
                stream = storage.AddStream(streamName);

            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                foreach (var entryStreamItem in this)
                    entryStreamItem.Write(binaryWriter);

                stream.SetData(memoryStream.ToArray());
            }
        }

        #endregion
    }

}