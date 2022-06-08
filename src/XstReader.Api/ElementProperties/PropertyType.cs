// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021,2022 iluvadev, and released under Ms-PL License.
// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 
// 
//
// File from MsgKit (https://github.com/Sicos1977/MsgKit), under MIT license
//
// PropertyType.cs
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

namespace XstReader.ElementProperties
{
    /// <summary>
    ///     The type of a property in the properties stream
    /// </summary>
    public enum PropertyType : ushort
    {
        /// <summary>
        ///     Any: this property type value matches any type; a server MUST return the actual type in its response. Servers
        ///     MUST NOT return this type in response to a client request other than NspiGetIDsFromNames or the
        ///     RopGetPropertyIdsFromNamesROP request ([MS-OXCROPS] section 2.2.8.1). (PT_UNSPECIFIED)
        /// </summary>
        PT_UNSPECIFIED = 0x0000,

        /// <summary>
        ///     None: This property is a placeholder. (PT_NULL)
        /// </summary>
        PT_NULL = 0x0001,

        /// <summary>
        ///     2 bytes; a 16-bit integer (PT_I2, i2, ui2)
        /// </summary>
        PT_SHORT = 0x0002,

        /// <summary>
        ///     4 bytes; a 32-bit integer (PT_LONG, PT_I4, int, ui4)
        /// </summary>
        PT_LONG = 0x0003,

        /// <summary>
        ///     4 bytes; a 32-bit floating point number (PT_FLOAT, PT_R4, float, r4)
        /// </summary>
        PT_FLOAT = 0x0004,

        /// <summary>
        ///     8 bytes; a 64-bit floating point number (PT_DOUBLE, PT_R8, r8)
        /// </summary>
        PT_DOUBLE = 0x0005,

        /// <summary>
        ///     8 bytes; a 64-bit floating point number in which the whole number part represents the number of days since
        ///     December 30, 1899, and the fractional part represents the fraction of a day since midnight (PT_APPTIME)
        /// </summary>
        PT_APPTIME = 0x0007,

        /// <summary>
        ///     4 bytes; a 32-bit integer encoding error information as specified in section 2.4.1. (PT_ERROR)
        /// </summary>
        PT_ERROR = 0x000A,

        /// <summary>
        ///     1 byte; restricted to 1 or 0 (PT_BOOLEAN. bool)
        /// </summary>
        PT_BOOLEAN = 0x000B,

        /// <summary>
        ///     The property value is a Component Object Model (COM) object, as specified in section 2.11.1.5. (PT_OBJECT)
        /// </summary>
        PT_OBJECT = 0x000D,

        /// <summary>
        ///     8 bytes; a 64-bit integer (PT_LONGLONG, PT_I8, i8, ui8)
        /// </summary>
        PT_I8 = 0x0014,

        /// <summary>
        ///     8 bytes; a 64-bit integer (PT_LONGLONG, PT_I8, i8, ui8)
        /// </summary>
        PT_LONGLONG = 0x0014,

        /// <summary>
        ///     Variable size; a string of Unicode characters in UTF-16LE format encoding with terminating null character
        ///     (0x0000). (PT_UNICODE, string)
        /// </summary>
        PT_UNICODE = 0x001F,

        /// <summary>
        ///     Variable size; a string of multibyte characters in externally specified encoding with terminating null
        ///     character (single 0 byte). (PT_STRING8) ... ANSI format
        /// </summary>
        PT_STRING8 = 0x001E,

        /// <summary>
        ///     8 bytes; a 64-bit integer representing the number of 100-nanosecond intervals since January 1, 1601
        ///     (PT_SYSTIME, time, datetime, datetime.tz, datetime.rfc1123, Date, time, time.tz)
        /// </summary>
        PT_SYSTIME = 0x0040,

        /// <summary>
        ///     16 bytes; a GUID with Data1, Data2, and Data3 fields in little-endian format (PT_CLSID, UUID)
        /// </summary>
        PT_CLSID = 0x0048,

        /// <summary>
        ///     Variable size; a 16-bit COUNT field followed by a structure as specified in section 2.11.1.4. (PT_SVREID)
        /// </summary>
        PT_SVREID = 0x00FB,

        /// <summary>
        ///     Variable size; a byte array representing one or more Restriction structures as specified in section 2.12.
        ///     (PT_SRESTRICT)
        /// </summary>
        PT_SRESTRICT = 0x00FD,

        /// <summary>
        ///     Variable size; a 16-bit COUNT field followed by that many rule (4) action (3) structures, as specified in
        ///     [MS-OXORULE] section 2.2.5. (PT_ACTIONS)
        /// </summary>
        PT_ACTIONS = 0x00FE,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many bytes. (PT_BINARY)
        /// </summary>
        PT_BINARY = 0x0102,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_SHORT values. (PT_MV_SHORT, PT_MV_I2, mv.i2)
        /// </summary>
        PT_MV_SHORT = 0x1002,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_LONG values. (PT_MV_LONG, PT_MV_I4, mv.i4)
        /// </summary>
        PT_MV_LONG = 0x1003,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_FLOAT values. (PT_MV_FLOAT, PT_MV_R4, mv.float)
        /// </summary>
        PT_MV_FLOAT = 0x1004,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_DOUBLE values. (PT_MV_DOUBLE, PT_MV_R8)
        /// </summary>
        PT_MV_DOUBLE = 0x1005,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_CURRENCY values. (PT_MV_CURRENCY, mv.fixed.14.4)
        /// </summary>
        PT_MV_CURRENCY = 0x1006,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_APPTIME values. (PT_MV_APPTIME)
        /// </summary>
        PT_MV_APPTIME = 0x1007,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_LONGLONGvalues. (PT_MV_I8, PT_MV_I8)
        /// </summary>
        PT_MV_LONGLONG = 0x1014,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_UNICODE values. (PT_MV_UNICODE)
        /// </summary>
        PT_MV_TSTRING = 0x101F,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_UNICODE values. (PT_MV_UNICODE)
        /// </summary>
        PT_MV_UNICODE = 0x101F,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_STRING8 values. (PT_MV_STRING8, mv.string)
        /// </summary>
        PT_MV_STRING8 = 0x101E,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_SYSTIME values. (PT_MV_SYSTIME)
        /// </summary>
        PT_MV_SYSTIME = 0x1040,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_CLSID values. (PT_MV_CLSID, mv.uuid)
        /// </summary>
        PT_MV_CLSID = 0x1048,

        /// <summary>
        ///     Variable size; a COUNT field followed by that many PT_MV_BINARY values. (PT_MV_BINARY, mv.bin.hex)
        /// </summary>
        PT_MV_BINARY = 0x1102,
    }
}