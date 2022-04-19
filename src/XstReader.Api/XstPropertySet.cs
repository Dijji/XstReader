// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021,2022 iluvadev, and released under Ms-PL License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// A set of Properties of a pst/ost Element
    /// </summary>
    public class XstPropertySet
    {
        private Dictionary<PropertyCanonicalName, XstProperty> _DicProperties = null;
        private Dictionary<PropertyCanonicalName, XstProperty> DicProperties
            => _DicProperties ?? (_DicProperties = new Dictionary<PropertyCanonicalName, XstProperty>());

        private List<PropertyCanonicalName> _NotPresentTags = new List<PropertyCanonicalName>();

        /// <summary>
        /// Indicates if the Properties are loaded
        /// </summary>
        public bool IsLoaded { get; private set; } = false;

        /// <summary>
        /// The properties
        /// </summary>
        public IEnumerable<XstProperty> Items
        {
            get
            {
                LoadProperties();
                return DicProperties.Values;
            }
        }
        /// <summary>
        /// The Properties that are not Binary values (as attachment binary content)
        /// </summary>
        [Obsolete("This property is Obsolete. Use Items.NonBinary() instead")]
        [Browsable(false)]
        public IEnumerable<XstProperty> ItemsNonBinary => Items.NonBinary();

        /// <summary>
        /// The Tags of the properties
        /// </summary>
        public IEnumerable<PropertyCanonicalName> Tags
        {
            get
            {
                LoadProperties();
                return DicProperties.Keys;
            }
        }

        private Func<IEnumerable<XstProperty>> PropertiesGetter { get; set; }
        private Func<PropertyCanonicalName, XstProperty> PropertyGetter { get; set; }
        private Func<PropertyCanonicalName, bool> PropertyChecker { get; set; }

        internal XstPropertySet(Func<IEnumerable<XstProperty>> propertiesGetter,
                                Func<PropertyCanonicalName, XstProperty> propertyGetter,
                                Func<PropertyCanonicalName, bool> propertyChecker)
        {
            PropertiesGetter = propertiesGetter;
            PropertyGetter = propertyGetter;
            PropertyChecker = propertyChecker;
        }

        /// <summary>
        /// Gets the Property by its Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public XstProperty this[UInt16 tag]
            => Get(tag);
        /// <summary>
        /// Gets the Property by its Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public XstProperty this[PropertyCanonicalName tag]
            => Get(tag);

        /// <summary>
        /// Gets the Property by its Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public XstProperty Get(UInt16 tag)
            => Get((PropertyCanonicalName)tag);

        /// <summary>
        /// Gets the Property by its Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public XstProperty Get(PropertyCanonicalName tag)
        {
            if (!DicProperties.ContainsKey(tag))
            {
                if (_NotPresentTags.Contains(tag)) return null;
                if (IsLoaded)
                {
                    _NotPresentTags.Add(tag);
                    return null;
                }
                var p = PropertyGetter?.Invoke(tag);
                if (p == null)
                {
                    _NotPresentTags.Add(tag);
                    return null;
                }
                DicProperties[tag] = p;
            }
            if (DicProperties.ContainsKey(tag))
                return DicProperties[tag];
            return null;
        }

        /// <summary>
        /// Gets the Properties that belongs to specified PropertyArea
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public IEnumerable<XstProperty> Get(PropertyArea area)
            => Items.Where(p => p.Tag.PropertyArea() == area);

        /// <summary>
        /// Gets the Properties that belongs to specified PropertySet
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public IEnumerable<XstProperty> Get(PropertySet set)
            => Items.Where(p => p.Tag.PropertySet() == set);

        private void LoadProperties()
        {
            if (IsLoaded)
                return;
            if (PropertiesGetter == null)
                return;

            Add(PropertiesGetter());
            IsLoaded = true;
        }

        /// <summary>
        /// Returns if there is a Property with specified Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool Contains(UInt16 tag)
            => Contains((PropertyCanonicalName)tag);

        /// <summary>
        /// Returns if there is a Property with specified Tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool Contains(PropertyCanonicalName tag)
        {
            if (_NotPresentTags.Contains(tag))
                return false;
            if (DicProperties.ContainsKey(tag) || (PropertyChecker?.Invoke(tag) ?? false))
                return true;

            _NotPresentTags.Add(tag);
            return false;
        }

        internal void Add(XstProperty property)
        {
            if (property != null)
                DicProperties[property.Tag] = property;
        }

        internal void Add(IEnumerable<XstProperty> properties)
        {
            foreach (var property in properties)
                Add(property);
        }

        /// <summary>
        /// Clear contents and memory
        /// </summary>
        public void ClearContents()
        {
            if (_DicProperties != null)
            {
                foreach (var prop in _DicProperties.Values)
                    prop.ClearContents();
                _DicProperties.Clear();
            }
            _DicProperties = null;
            IsLoaded = false;
        }
    }
}
