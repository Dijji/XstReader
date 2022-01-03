using System;
using System.Collections.Generic;
using XstReader.ItemProperties;

namespace XstReader
{
    public class XstPropertySet
    {
        private Dictionary<EpropertyTag, XstProperty> DicProperties { get; } = new Dictionary<EpropertyTag, XstProperty>();

        public IEnumerable<XstProperty> Values => DicProperties.Values;

        public XstProperty this[UInt16 tag] => Get(tag);
        public XstProperty this[EpropertyTag tag] => Get(tag);

        public XstProperty Get(UInt16 tag)
            => Get((EpropertyTag)tag);

        public XstProperty Get(EpropertyTag tag)
        {
            if (DicProperties.ContainsKey(tag))
                return DicProperties[tag];
            return null;
        }

        public bool Contains(UInt16 tag)
            => Contains((EpropertyTag)tag);

        public bool Contains(EpropertyTag tag)
            => DicProperties.ContainsKey(tag);

        public void Add(XstProperty property)
        {
            if (property != null)
                DicProperties[property.Tag] = property;
        }

        public void Add(IEnumerable<XstProperty> properties)
        {
            foreach (var property in properties)
                Add(property);
        }

    }
}
