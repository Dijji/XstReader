// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using XstReader.App.Controls;
using XstReader.ElementProperties;

namespace XstReader.App
{
    public static class XstPropertySetExtensions
    {
        public static CustomXstPropertySet ToPropertyGridSelectedObject(this XstPropertySet propertySet)
        {
            //Ms defines some PropertyTags with the same value
            //This is to get all PropertyTags with the same value, not only the first
            var tagsInProperties = propertySet.Tags;
            var usedKnownProperties = propertySet.Tags.SelectMany(t => KnownCanonicalNames.GetByEnum(t));
            var usedUnknownTags = tagsInProperties.Where(t => !usedKnownProperties.Any(k => k.Id == (UInt16)t));

            var customizedPropClass = new CustomXstPropertySet();
            foreach (var knownProp in usedKnownProperties)
                customizedPropClass.Add(new CustomXstProperty(knownProp, propertySet[knownProp.Id].Value));

            foreach (var tag in usedUnknownTags)
                customizedPropClass.Add(new CustomXstProperty(propertySet[tag]));

            ////foreach (var prop in propertySet.Items)
            ////    customizedPropClass.Add(new CustomXstProperty(prop));

            return customizedPropClass;
        }
    }
}
