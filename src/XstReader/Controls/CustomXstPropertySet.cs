// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System.Collections;
using System.ComponentModel;

namespace XstReader.App.Controls
{
    public class CustomXstPropertySet : CollectionBase, ICustomTypeDescriptor
    {
        public CustomXstPropertySet()
        {
        }

        public CustomXstPropertySet(IEnumerable<CustomXstProperty> values)
        {
            foreach (var value in values)
                List.Add(value);
        }

        /// <summary>
        /// Add CustomProperty to Collectionbase List
        /// </summary>
        /// <param name="value"></param>
        public void Add(CustomXstProperty value) => List.Add(value);

        /// <summary>
        /// Remove item from List
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            foreach (CustomXstProperty prop in base.List)
            {
                if (prop.Name == name)
                {
                    base.List.Remove(prop);
                    return;
                }
            }
        }

        /// <summary>
        /// Indexer
        /// </summary>
        public CustomXstProperty? this[int index]
        {
            get => List[index] as CustomXstProperty;
            set => List[index] = value;
        }


        #region "TypeDescriptor Implementation"
        /// <summary>
        /// Get Class Name
        /// </summary>
        /// <returns>String</returns>
        public string? GetClassName() => TypeDescriptor.GetClassName(this, true);


        /// <summary>
        /// GetAttributes
        /// </summary>
        /// <returns>AttributeCollection</returns>
        public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);

        /// <summary>
        /// GetComponentName
        /// </summary>
        /// <returns>String</returns>
        public string? GetComponentName() => TypeDescriptor.GetComponentName(this, true);


        /// <summary>
        /// GetConverter
        /// </summary>
        /// <returns>TypeConverter</returns>
        public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);

        /// <summary>
        /// GetDefaultEvent
        /// </summary>
        /// <returns>EventDescriptor</returns>
        public EventDescriptor? GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);

        /// <summary>
        /// GetDefaultProperty
        /// </summary>
        /// <returns>PropertyDescriptor</returns>
        public PropertyDescriptor? GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(this, true);

        /// <summary>
        /// GetEditor
        /// </summary>
        /// <param name="editorBaseType">editorBaseType</param>
        /// <returns>object</returns>
        public object? GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);

        public EventDescriptorCollection GetEvents(Attribute[]? attributes) => TypeDescriptor.GetEvents(this, attributes, true);

        public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(this, true);

        public PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
        {
            var newProps = new List<PropertyDescriptor>();
            foreach (var item in List)
                if (item is CustomXstProperty prop)
                    newProps.Add(new CustomXstPropertyDescriptor(ref prop, attributes));

            return new PropertyDescriptorCollection(newProps.ToArray());
        }

        public PropertyDescriptorCollection GetProperties() => TypeDescriptor.GetProperties(this, true);

        public object GetPropertyOwner(PropertyDescriptor? pd) => this;
        #endregion


    }
}
