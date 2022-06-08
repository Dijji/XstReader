//
// PropertyFlags.cs
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

using System;

namespace XstReader.MsgKit.Enums
{
    /// <summary>
    ///     Flags used to set on a <see cref="Structures.Property" />
    /// </summary>
    /// <remarks>
    ///     See https://msdn.microsoft.com/en-us/library/ee158556(v=exchg.80).aspx
    /// </remarks>
    [Flags]
    public enum PropertyFlags : uint
    {
        /// <summary>
        ///     If this flag is set for a property, that property MUST NOT be deleted from the .msg file
        ///     (irrespective of which storage it is contained in) and implementations MUST return an error
        ///     if any attempt is made to do so. This flag is set in circumstances where the implementation
        ///     depends on that property always being present in the .msg file once it is written there.
        /// </summary>
        PROPATTR_MANDATORY = 0x00000001,

        /// <summary>
        ///     If this flag is not set on a property, that property MUST NOT be read from the .msg file
        ///     and implementations MUST return an error if any attempt is made to read it. This flag is
        ///     set on all properties unless there is an implementation-specific reason to prevent a property
        ///     from being read from the .msg file.
        /// </summary>
        PROPATTR_READABLE = 0x00000002,

        /// <summary>
        ///     If this flag is not set on a property, that property MUST NOT be modified or deleted and
        ///     implementations MUST return an error if any attempt is made to do so. This flag is set in
        ///     circumstances where the implementation depends on the properties being writable.
        /// </summary>
        PROPATTR_WRITABLE = 0x00000004
    }
}