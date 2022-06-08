//
// Property.cs
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using XstReader.ElementProperties;
using XstReader.MsgKit.Enums;
using XstReader.MsgKit.Exceptions;

namespace XstReader.MsgKit.Structures
{
    /// <summary>
    ///     A property inside the MSG file
    /// </summary>
    public class Property
    {
        #region Properties
        /// <summary>
        ///     The id of the property
        /// </summary>
        internal ushort Id { get; }

        /// <summary>
        ///     Returns the Property as a readable string
        /// </summary>
        /// <returns></returns>
        public string Name => PropertyTags.SubStorageStreamPrefix + GetPropertyId(Id, Type, MultiValueIndex);

        /// <summary>
        ///     Returns the Property as a readable string without the streamprefix and type
        /// </summary>
        /// <returns></returns>
        public string ShortName => Id.ToString("X4");

        /// <summary>
        ///     The <see cref="PropertyType" />
        /// </summary>
        internal PropertyType Type { get; }

        /// <summary>
        ///     The <see cref="PropertyFlags">property flags</see> that have been set
        ///     in its <see cref="uint" /> raw form
        /// </summary>
        internal uint Flags { get; }

        /// <summary>
        ///     The index position of the multi value property
        /// </summary>
        private int MultiValueIndex { get; }

        /// <summary>
        ///     returns true if this represents a single data entry, belonging to a multi valu property
        /// </summary>
        internal bool IsMultiValueData => MultiValueIndex >= 0;

        /// <summary>
        ///     The <see cref="PropertyFlags">property flags</see> that have been set
        ///     as a readonly collection
        /// </summary>
        internal ReadOnlyCollection<PropertyFlags> FlagsCollection
        {
            get
            {
                var result = new List<PropertyFlags>();

                if ((Flags & Convert.ToUInt32(PropertyFlags.PROPATTR_MANDATORY)) != 0)
                    result.Add(PropertyFlags.PROPATTR_MANDATORY);

                if ((Flags & Convert.ToUInt32(PropertyFlags.PROPATTR_READABLE)) != 0)
                    result.Add(PropertyFlags.PROPATTR_READABLE);

                if ((Flags & Convert.ToUInt32(PropertyFlags.PROPATTR_WRITABLE)) != 0)
                    result.Add(PropertyFlags.PROPATTR_WRITABLE);

                return result.AsReadOnly();
            }
        }

        /// <summary>
        ///     The property data
        /// </summary>
        internal byte[] Data { get; }

        /// <summary>
        ///     Returns <see cref="Data" /> as an integer when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_SHORT" />,
        ///     <see cref="PropertyType.PT_LONG" /> or <see cref="PropertyType.PT_ERROR" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not <see cref="PropertyType.PT_SHORT"/> or
        /// <see cref="PropertyType.PT_LONG"/></exception>
        internal int ToInt
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_SHORT:
                        return BitConverter.ToInt16(Data, 0);

                    case PropertyType.PT_LONG:
                        return BitConverter.ToInt32(Data, 0);

                    default:
                        throw new MKInvalidProperty("Type is not PT_SHORT or PT_LONG");
                }
            }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as a single when <see cref="Type" /> is set to 
        ///     <see cref="PropertyType.PT_FLOAT" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not <see cref="PropertyType.PT_FLOAT"/></exception>
        internal float ToSingle
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_FLOAT:
                        return BitConverter.ToSingle(Data, 0);

                    default:
                        throw new MKInvalidProperty("Type is not PT_FLOAT");
                }
            }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as a single when <see cref="Type" /> is set to 
        ///     <see cref="PropertyType.PT_DOUBLE" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not <see cref="PropertyType.PT_DOUBLE"/></exception>
        internal double ToDouble
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_DOUBLE:
                        return BitConverter.ToDouble(Data, 0);

                    default:
                        throw new MKInvalidProperty("Type is not PT_DOUBLE");
                }
            }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as a decimal when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_FLOAT" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not <see cref="PropertyType.PT_FLOAT"/></exception>
        internal decimal ToDecimal
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_FLOAT:
                        return ByteArrayToDecimal(Data, 0);

                    default:
                        throw new MKInvalidProperty("Type is not PT_FLOAT");
                }
            }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as a datetime when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_APPTIME" />
        ///     or <see cref="PropertyType.PT_SYSTIME" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not set to <see cref="PropertyType.PT_APPTIME"/> or
        /// <see cref="PropertyType.PT_SYSTIME"/></exception>
        internal DateTime ToDateTime
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_APPTIME:
                        var oaDate = BitConverter.ToDouble(Data, 0);
                        return DateTime.FromOADate(oaDate);

                    case PropertyType.PT_SYSTIME:
                        var fileTime = BitConverter.ToInt64(Data, 0);
                        return DateTime.FromFileTime(fileTime);

                    default:
                        throw new MKInvalidProperty("Type is not PT_APPTIME or PT_SYSTIME");
                }
            }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as a boolean when <see cref="Type" /> is set to <see cref="PropertyType.PT_BOOLEAN" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not set to <see cref="PropertyType.PT_BOOLEAN"/></exception>
        internal bool ToBool
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_BOOLEAN:
                        return BitConverter.ToBoolean(Data, 0);

                    default:
                        throw new MKInvalidProperty("Type is not PT_BOOLEAN");
                }
            }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as a boolean when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_LONGLONG" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not set to <see cref="PropertyType.PT_LONGLONG"/></exception>
        internal long ToLong
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_LONG:
                    case PropertyType.PT_LONGLONG:
                        return BitConverter.ToInt64(Data, 0);

                    default:
                        throw new MKInvalidProperty("Type is not PT_LONGLONG");
                }
            }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as a string when <see cref="Type" /> is set to <see cref="PropertyType.PT_UNICODE" />
        ///     or <see cref="PropertyType.PT_STRING8" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not set to <see cref="PropertyType.PT_UNICODE"/> or <see cref="PropertyType.PT_STRING8" /></exception>
        public new string ToString
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_UNICODE:
                    case PropertyType.PT_STRING8:
                        var encoding = Type == PropertyType.PT_STRING8 ? Encoding.Default : Encoding.Unicode;
                        using (var memoryStream = new MemoryStream(Data))
                        using (var streamReader = new StreamReader(memoryStream, encoding))
                        {
                            var streamContent = streamReader.ReadToEnd();
                            return streamContent.TrimEnd('\0');
                        }

                    default:
                        throw new MKInvalidProperty("Type is not PT_UNICODE or PT_STRING8");
                }
            }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as a Guid when <see cref="Type" /> is set to <see cref="PropertyType.PT_CLSID" />
        ///     <see cref="PropertyType.PT_OBJECT" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not set to <see cref="PropertyType.PT_BINARY"/></exception>
        public Guid ToGuid
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_CLSID:
                        return new Guid(Data);

                    default:
                        throw new MKInvalidProperty("Type is not PT_CLSID");
                }
            }
        }

        /*
        /// <summary>
        ///     Variable size; a 16-bit COUNT field followed by a structure as specified in section 2.11.1.4. (PT_SVREID)
        /// </summary>
        PtypServerId = 0x00FB,

        /// <summary>
        ///     Variable size; a byte array representing one or more Restriction structures as specified in section 2.12.
        ///     (PT_SRESTRICT)
        /// </summary>
        PtypRestriction = 0x00FD,

        /// <summary>
        ///     Variable size; a 16-bit COUNT field followed by that many rule (4) action (3) structures, as specified in
        ///     [MS-OXORULE] section 2.2.5. (PT_ACTIONS)
        /// </summary>
        PtypRuleAction = 0x00FE,
        */

        /// <summary>
        ///     Returns <see cref="Data" /> as a byte[] when <see cref="Type" /> is set to <see cref="PropertyType.PT_BINARY" />
        ///     <see cref="PropertyType.PT_OBJECT" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not set to <see cref="PropertyType.PT_BINARY"/></exception>
        public byte[] ToBinary
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.PT_BINARY:
                        return Data;

                    default:
                        throw new MKInvalidProperty("Type is not PT_BINARY");
                }
            }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as an readonly collection of integers when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_MV_SHORT" /> or <see cref="PropertyType.PT_MV_LONG" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not set to <see cref="PropertyType.PT_FLOAT"/></exception>
        internal ReadOnlyCollection<int> ToIntCollection
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Returns <see cref="Data" /> as an readonly collection of floats when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_MV_FLOAT" /> or <see cref="PropertyType.PT_MV_DOUBLE" />
        /// </summary>
        /// <exception cref="MKInvalidProperty">Raised when the <see cref="Type"/> is not set to <see cref="PropertyType.PT_FLOAT"/></exception>
        internal ReadOnlyCollection<float> ToFloatCollection => throw new NotImplementedException();

        /// <summary>
        ///     Returns <see cref="Data" /> as an readonly collection of decimals when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_MV_CURRENCY" />
        /// </summary>
        internal ReadOnlyCollection<decimal> ToDecimalCollection => throw new NotImplementedException();

        /// <summary>
        ///     Returns <see cref="Data" /> as an readonly collection of datetime when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_MV_APPTIME" /> or <see cref="PropertyType.PT_MV_SYSTIME" />
        /// </summary>
        internal ReadOnlyCollection<DateTime> ToDateTimeCollection => throw new NotImplementedException();

        /// <summary>
        ///     Returns <see cref="Data" /> as an readonly collection of datetime when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_MV_LONGLONG" />
        /// </summary>
        internal ReadOnlyCollection<long> ToLongCollection => throw new NotImplementedException();

        /// <summary>
        ///     Returns <see cref="Data" /> as an readonly collection of strings when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_MV_STRING8" />
        /// </summary>
        internal ReadOnlyCollection<long> ToStringCollection => throw new NotImplementedException();

        ///// <summary>
        /////     Returns <see cref="Data" /> as an readonly collection of guids when <see cref="Type" /> is set to
        /////     <see cref="PropertyType.PT_MV_CLSID" />
        ///// </summary>
        //internal ReadOnlyCollection<Guid> ToGuidCollection
        //{
        //    get { throw new NotImplementedException(); }
        //}

        /// <summary>
        ///     Returns <see cref="Data" /> as an readonly collection of byte arrays when <see cref="Type" /> is set to
        ///     <see cref="PropertyType.PT_MV_BINARY" />
        /// </summary>
        internal ReadOnlyCollection<byte[]> ToBinaryCollection => throw new NotImplementedException();

        /*
        /// <summary>
        ///     Any: this property type value matches any type; a server MUST return the actual type in its response. Servers
        ///     MUST NOT return this type in response to a client request other than NspiGetIDsFromNames or the
        ///     RopGetPropertyIdsFromNamesROP request ([MS-OXCROPS] section 2.2.8.1). (PT_UNSPECIFIED)
        /// </summary>
        PtypUnspecified = 0x0000,

        /// <summary>
        ///     None: This property is a placeholder. (PT_NULL)
        /// </summary>
        PtypNull = 0x0001,
        */
        #endregion

        #region GetPropertyId
        /// <summary>
        ///     Returns the property name, excluding the substorage prefix
        /// </summary>
        private static string GetPropertyId(ushort id, PropertyType type, int multiValueIndex = -1) =>
            id.ToString("X4")
            + ((ushort)type).ToString("X4")
            + (multiValueIndex >= 0 ? "-" + ((uint)multiValueIndex).ToString("X8") : "");
        #endregion

        #region ByteArrayToDecimal
        /// <summary>
        ///     Converts a byte array to a decimal
        /// </summary>
        /// <param name="source">The byte array</param>
        /// <param name="offset">The offset to start reading</param>
        /// <returns></returns>
        private static decimal ByteArrayToDecimal(byte[] source, int offset)
        {
            var i1 = BitConverter.ToInt32(source, offset);
            var i2 = BitConverter.ToInt32(source, offset + 4);
            var i3 = BitConverter.ToInt32(source, offset + 8);
            var i4 = BitConverter.ToInt32(source, offset + 12);

            return new decimal(new[] { i1, i2, i3, i4 });
        }
        #endregion

        #region Constructor
        /// <summary>
        ///     Creates this object and sets all its propertues
        /// </summary>
        /// <param name="id">The id of the property</param>
        /// <param name="type">The <see cref="PropertyType" /></param>
        /// <param name="data">The property data</param>
        /// <param name="multiValueIndex">If part of a multivalue property, this is the index of the value</param>
        internal Property(ushort id, PropertyType type, byte[] data, int multiValueIndex = -1)
        {
            Id = id;
            Type = type;
            Data = data;
            MultiValueIndex = multiValueIndex;
        }

        /// <summary>
        ///     Creates this object and sets all its propertues
        /// </summary>
        /// <param name="id">The id of the property</param>
        /// <param name="type">The <see cref="PropertyType" /></param>
        /// <param name="flags">The <see cref="PropertyFlags" /></param>
        /// <param name="data">The property data</param>
        /// <param name="multiValueIndex">If part of a multivalue property, this is the index of the value</param>
        internal Property(ushort id, PropertyType type, PropertyFlags flags, byte[] data, int multiValueIndex = -1)
        {
            Id = id;
            Type = type;
            Flags = Convert.ToUInt32(flags);
            Data = data;
            MultiValueIndex = multiValueIndex;
        }

        /// <summary>
        ///     Creates this object and sets all its propertues
        /// </summary>
        /// <param name="id">The id of the property</param>
        /// <param name="type">The <see cref="PropertyType" /></param>
        /// <param name="flags">The <see cref="PropertyFlags" /></param>
        /// <param name="data">The property data</param>
        /// <param name="multiValueIndex">If part of a multivalue property, this is the index of the value</param>
        internal Property(ushort id, PropertyType type, uint flags, byte[] data, int multiValueIndex = -1)
        {
            Id = id;
            Type = type;
            Flags = flags;
            Data = data;
            MultiValueIndex = multiValueIndex;
        }
        #endregion
    }
}