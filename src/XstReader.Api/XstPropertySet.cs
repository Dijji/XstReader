using System;
using System.Collections.Generic;
using XstReader.ItemProperties;

namespace XstReader
{
    public class XstPropertySet
    {
        private Dictionary<EpropertyTag, XstProperty> DicProperties { get; } = new Dictionary<EpropertyTag, XstProperty>();

        public bool IsLoaded { get; private set; } = false;

        public IEnumerable<XstProperty> Values
        {
            get
            {
                LoadProperties();
                return DicProperties.Values;
            }
        }
        private Func<IEnumerable<XstProperty>> PropertiesGetter { get; set; }

        internal XstPropertySet(Func<IEnumerable<XstProperty>> propertiesGetter)
        {
            PropertiesGetter = propertiesGetter;
        }

        public XstProperty this[UInt16 tag] => Get(tag);
        public XstProperty this[EpropertyTag tag] => Get(tag);

        public XstProperty Get(UInt16 tag)
            => Get((EpropertyTag)tag);

        public XstProperty Get(EpropertyTag tag)
        {
            if (!IsLoaded && !Contains(tag))
                LoadProperties();
            if (DicProperties.ContainsKey(tag))
                return DicProperties[tag];
            return null;
        }
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
            => Contains((EpropertyTag)tag);

        public bool Contains(EpropertyTag tag)
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
            DicProperties.Clear();
            IsLoaded = false;
        }
    }
}
