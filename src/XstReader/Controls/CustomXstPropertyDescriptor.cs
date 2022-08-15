// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System.ComponentModel;

namespace XstReader.App.Controls
{
    public class CustomXstPropertyDescriptor : PropertyDescriptor
    {
        private CustomXstProperty Property { get; init; }
        public CustomXstPropertyDescriptor(ref CustomXstProperty myProperty, Attribute[]? attrs)
            : base(myProperty.Name, attrs)
        {
            Property = myProperty;
        }

        public override Type ComponentType => typeof(object);

        public override bool IsReadOnly => true;

        public override Type? PropertyType => Property.Value?.GetType();

        public override bool CanResetValue(object component) => false;

        public override object? GetValue(object? component) => Property.Value;

        public override void ResetValue(object component) { }

        public override void SetValue(object? component, object? value) { }

        public override bool ShouldSerializeValue(object component) => false;
        public override string Category => Property.Category ?? "";
        public override string Description => Property.Description ?? "";

        public string HtmlDescription => $"<b><big>{Property.Name}</big></b><br>{Property.HtmlDescription??"<em>Undocumented property</em>"}";
    }
}
