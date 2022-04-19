// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.
// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 


using System;
using System.Collections.Generic;
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// A Property of a pst/ost element
    /// </summary>
    public class XstProperty
    {
        /// <summary>
        /// Canonical Name of the property 
        /// (from <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/../ms-oxcfold/c0f31b95-c07f-486c-98d9-535ed9705fbf">
        /// [MS-OXCFOLD]</see>)
        /// </summary>
        public PropertyCanonicalName Tag { get; internal set; }

        /// <summary>
        /// The Type of the Property
        /// </summary>
        public EpropertyType PropertyType { get; internal set; }

        internal Func<dynamic> ValueGetter { get; set; } = null;
        private dynamic _Value;
        /// <summary>
        /// The value of the property for the element
        /// </summary>
        public dynamic Value
        {
            get => _Value ?? (_Value = ValueGetter?.Invoke());
            set => _Value = value;
        }

        internal string ValueAsStringSanitized => (Value as string).SanitizeControlChars();

        /// <summary>
        /// Ctor
        /// </summary>
        public XstProperty()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="propertyType"></param>
        /// <param name="valueGetter"></param>
        public XstProperty(PropertyCanonicalName tag, EpropertyType propertyType, Func<dynamic> valueGetter)
        {
            Tag = tag;
            PropertyType = propertyType;
            ValueGetter = valueGetter;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="newTag"></param>
        internal XstProperty CopyToNew(PropertyCanonicalName newTag)
            => new XstProperty
            {
                Tag = newTag,
                PropertyType = this.PropertyType,
                ValueGetter = this.ValueGetter,
                _Value = this._Value,
            };

        // Standard properties have a Tag value less than 0x8000,
        // and identify a particular property
        //
        // Tag values from 0x8000 to 0x8fff are named properties,
        // where the Tag Is the key into a per .ost or .pst dictionary of properties
        // identified by a GUID (identifying a Property Set) and a name (identifying a property Within that set), 
        // which can be a string or a 32-bit value

        private string _PropertySetGuid = null;
        /// <summary>
        /// String representation of hex GUID of its PropertySet
        /// </summary>
        public string PropertySetGuid
        {
            get => _PropertySetGuid ?? Tag.PropertySet()?.Guid()?.ToString();
            internal set => _PropertySetGuid = value;
        }

        private string _PropertySetName = null;
        /// <summary>
        /// The name of its PropertySet, e.g. PSETID_Common 
        /// </summary>
        public string PropertySetName
        {
            get => _PropertySetName ?? Tag.PropertySet()?.ToString();
            internal set => _PropertySetName = value;
        }

        private string _Name = null;
        /// <summary>
        /// The Name of the Property, when we know it
        /// </summary>
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

        internal bool IsNamed => (UInt16)Tag >= 0x8000 && (UInt16)Tag <= 0x8fff;
        /// <summary>
        /// The string Id of the Property
        /// </summary>
        public string Id => Tag.Id0x();
        /// <summary>
        /// The Canonical Name of the Property, when we know it 
        /// </summary>
        public string CanonicalName => Tag.CanonicalName();
        /// <summary>
        /// The "Frienly Name" of the Property, when we know it
        /// </summary>
        public string FriendlyName => Tag.FriendlyName();
        /// <summary>
        /// The Area of the Property, when we know it
        /// </summary>
        public string Area => Tag.PropertyArea()?.FriendlyName();
        /// <summary>
        /// The Description of the Property, when we know it
        /// </summary>
        public string Description => Tag.Description();

        /// <summary>
        /// The Description of the Property, when we know it, in HTML format
        /// </summary>
        public string HtmlDescription => Tag.HtmlDescription();

        /// <summary>
        /// The String representation of the Value's Property
        /// </summary>
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
                //else if (Value == null)
                //    return null;
                else
                    return Value?.ToString();
            }
        }

        /// <summary>
        /// Clear all contents and memory used
        /// </summary>
        public void ClearContents()
        {
            _Value = null;
        }
    }
}
