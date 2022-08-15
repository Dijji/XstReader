// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XstReader.ElementProperties;

namespace XstReader.App.Controls
{
    //https://www.codeproject.com/Articles/9280/Add-Remove-Items-to-from-PropertyGrid-at-Runtime

    public class CustomXstProperty
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? HtmlDescription { get; set; }
        public string? Category { get; set; }
        public object Value { get; set; }
        public bool IsVisible { get; set; }

        public CustomXstProperty(string name, string description, object value)
        {
            Name = name;
            Description = description;
            Value = value;
        }

        //public CustomXstProperty(string name, string? description, string? htmlDescription, string? category, object value, bool isVisible)
        //{

        //}
        public CustomXstProperty(XstProperty xstProperty)
        {
            Name = xstProperty.FriendlyName;
            Description = xstProperty.Description;
            HtmlDescription = xstProperty.HtmlDescription;
            Category = xstProperty.Area;
            Value = xstProperty.Value;
        }

        public CustomXstProperty(KnownCanonicalNameProperty canonicalProperty, object value)
        {
            Name = canonicalProperty.FriendlyName;
            Description = canonicalProperty.Description;
            HtmlDescription = canonicalProperty.HtmlDescription;
            Category = canonicalProperty.Category;
            Value = value;
        }

    }
}
