// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System;
using System.Collections.Generic;
using System.Linq;
using XstReader.ItemProperties;

namespace XstReader
{
    public class XstProperty
    {
        public PropertyCanonicalName Tag { get; set; }
        public dynamic Value { get; set; }

        // Standard properties have a Tag value less than 0x8000,
        // and identify a particular property
        //
        // Tag values from 0x8000 to 0x8fff are named properties,
        // where the Tag Is the key into a per .ost or .pst dictionary of properties
        // identified by a GUID (identifying a Property Set) and a name (identifying a property Within that set), 
        // which can be a string or a 32-bit value

        private string _PropertySetGuid = null;
        // String representation of hex GUID
        public string PropertySetGuid
        {
            get => _PropertySetGuid ?? Tag.PropertySet()?.Guid()?.ToString();
            internal set => _PropertySetGuid = value;
        }

        private string _PropertySetName = null;
        // Equivalent name, where known e.g. PSETID_Common
        public string PropertySetName
        {
            get => _PropertySetName ?? Tag.PropertySet()?.ToString();
            internal set => _PropertySetName = value;
        }

        private string _Name = null;
        // String name of property, when we know it
        public string Name
        {
            get
            {
                string name = _Name ?? Tag.CanonicalName();
                if (string.IsNullOrEmpty(name))
                    name = Tag.ToString();
                return name;
            }
            internal set => _Name = value;
        }

        public bool IsNamed => (UInt16)Tag >= 0x8000 && (UInt16)Tag <= 0x8fff;


        public string Id => Tag.Id0x();
        public string CanonicalName => Tag.CanonicalName();
        public string FriendlyName => Tag.FriendlyName();
        public string Area => Tag.PropertyArea()?.FriendlyName();
        public string Description => Tag.Description();

        public string DisplayValue
        {
            get
            {
                if (Value is byte[] valBytes)
                    return BitConverter.ToString(valBytes);
                else if (Value is Int32[])
                    return String.Join(", ", Value);
                else if (Value is string[])
                    return String.Join(",\r\n", Value);
                else if (Value is List<byte[]> list)
                    return String.Join(",\r\n", list.Select(v => BitConverter.ToString(v)));
                else if (Value is DateTime dateTime)
                    return dateTime.ToUniversalTime().ToString("u");
                else if (Value == null)
                    return null;
                else
                    return Value.ToString();
            }
        }
    }
}
