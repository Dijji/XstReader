//
// PropertyTags.cs
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

using XstReader.ElementProperties;

namespace XstReader.MsgKit
{
    /// <summary>
    ///     A class that holds all the known mapi tags
    /// </summary>
    public static class PropertyTags
    {
        /// <summary>
        ///     The prefix for an Recipient <see cref="OpenMcdf.CFStorage" />
        /// </summary>
        internal const string RecipientStoragePrefix = "__recip_version1.0_#";

        /// <summary>
        ///     The prefix for an Attachment <see cref="OpenMcdf.CFStorage" />
        /// </summary>
        internal const string AttachmentStoragePrefix = "__attach_version1.0_#";

        /// <summary>
        ///     The prefix for a PropertyTag <see cref="OpenMcdf.CFStream" />
        /// </summary>
        internal const string SubStorageStreamPrefix = "__substg1.0_";

        /// <summary>
        ///     The name for the properties stream
        /// </summary>
        internal const string PropertiesStreamName = "__properties_version1.0";

        /// <summary>
        ///     The name id storage (named property mapping storage)
        /// </summary>
        internal const string NameIdStorage = "__nameid_version1.0";

        /// <summary>
        ///     The EntryStream stream
        /// </summary>
        internal const string EntryStream = "__substg1.0_00030102";

        /// <summary>
        ///     The GuidStream stream
        /// </summary>
        internal const string GuidStream = "__substg1.0_00020102";

        /// <summary>
        ///     The StringStream stream
        /// </summary>
        internal const string StringStream = "__substg1.0_00040102";
    }
}