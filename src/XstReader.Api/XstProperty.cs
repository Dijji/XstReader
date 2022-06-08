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
    public class XstProperty : XstPropertyBase
    {
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
        public XstProperty(PropertyCanonicalName tag, PropertyType propertyType, Func<dynamic> valueGetter) 
            : base(tag, propertyType)
        {
            ValueGetter = valueGetter;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="propertyType"></param>
        /// <param name="value"></param>
        public XstProperty(PropertyCanonicalName tag, PropertyType propertyType, dynamic value)
            : this(tag, propertyType, () => value)
        {
            Value = value;
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

        /// <summary>
        /// Clear all contents and memory used
        /// </summary>
        public void ClearContents()
        {
            _Value = null;
        }
    }
}
