// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System;
using System.Collections.Generic;
using System.Linq;
using XstReader.Properties;

namespace XstReader
{
    public class XstProperty
    {
        public EpropertyTag Tag { get; set; }
        public dynamic Value { get; set; }

        // Standard properties have a Tag value less than 0x8000,
        // and identify a particular property
        //
        // Tag values from 0x8000 to 0x8fff are named properties,
        // where the Tag Is the key into a per .ost or .pst dictionary of properties
        // identified by a GUID (identifying a Property Set) and a name (identifying a property Within that set), 
        // which can be a string or a 32-bit value
        //
        public string Guid { get; set; }        // String representation of hex GUID
        public string GuidName { get; set; }    // Equivalent name, where known e.g. PSETID_Common 
        public UInt32? Lid { get; set; }        // Property identifier, when we know it
        public string Name { get; set; }        // String name of property, when we know it

        public bool IsNamed { get { return (UInt16)Tag >= 0x8000 && (UInt16)Tag <= 0x8fff; } }

        public string DisplayId
        {
            get
            {
                return String.Format("0x{0:x4}", (UInt16)Tag);
            }
        }

        public string Description
        {
            get
            {
                string description;
                if (IsNamed)
                {
                    return String.Format("Guid: {0}\r\nName: {1}",
                        GuidName != null ? GuidName : Guid,
                        Name != null ? Name : String.Format("0x{0:x8}", Lid));
                }
                else if (StandardProperties.TagToDescription.TryGetValue(Tag, out description))
                    return description;
                else
                    return null;
            }
        }

        public string CsvId
        {
            get
            {
                if (IsNamed)
                    // Prefix with 80 In order to ensure they collate last
                    return String.Format("80{0}{1:x8}", Guid, Lid);
                else
                    return String.Format("{0:x4}", (UInt16)Tag);
            }
        }


        public string CsvDescription
        {
            get
            {
                string description;
                if (IsNamed)
                {
                    return String.Format("{0}: {1}",
                        GuidName != null ? GuidName : Guid,
                        Name != null ? Name : String.Format("0x{0:x8}", Lid));
                }
                else if (StandardProperties.TagToDescription.TryGetValue(Tag, out description))
                {
                    if (description.StartsWith("undocumented"))
                        return "undocumented " + DisplayId;
                    else
                        return description;
                }
                else
                    return DisplayId;
            }
        }

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
                else if (Value is List<byte[]>)
                    return String.Join(",\r\n", ((List<byte[]>)Value).Select(v => BitConverter.ToString(v)));
                else if (Value == null)
                    return null;
                else
                    return Value.ToString();
            }
        }
    }
}
