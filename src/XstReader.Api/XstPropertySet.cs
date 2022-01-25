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
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    public class XstPropertySet
    {
        private Dictionary<PropertyCanonicalName, XstProperty> _DicProperties = null;
        private Dictionary<PropertyCanonicalName, XstProperty> DicProperties
            => _DicProperties ?? (_DicProperties = new Dictionary<PropertyCanonicalName, XstProperty>());

        public bool IsLoaded { get; private set; } = false;

        public IEnumerable<XstProperty> Items
        {
            get
            {
                LoadProperties();
                return DicProperties.Values;
            }
        }
        public IEnumerable<XstProperty> ItemsNonBinary => Items.Where(p => p.PropertyType != EpropertyType.PtypBinary &&
                                                                           p.PropertyType != EpropertyType.PtypMultipleBinary);

        private Func<IEnumerable<XstProperty>> PropertiesGetter { get; set; }

        internal XstPropertySet(Func<IEnumerable<XstProperty>> propertiesGetter)
        {
            PropertiesGetter = propertiesGetter;
        }

        public XstProperty this[UInt16 tag] 
            => Get(tag);
        public XstProperty this[PropertyCanonicalName tag] 
            => Get(tag);

        public XstProperty Get(UInt16 tag)
            => Get((PropertyCanonicalName)tag);

        public XstProperty Get(PropertyCanonicalName tag)
        {
            if (!Contains(tag) && !IsLoaded)
                LoadProperties();
            if (DicProperties.ContainsKey(tag))
                return DicProperties[tag];
            return null;
        }

        public IEnumerable<XstProperty> Get(PropertyArea area)
            => Items.Where(p => p.Tag.PropertyArea() == area);

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

        public bool Contains(UInt16 tag)
            => Contains((PropertyCanonicalName)tag);

        public bool Contains(PropertyCanonicalName tag)
            => DicProperties.ContainsKey(tag);

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
