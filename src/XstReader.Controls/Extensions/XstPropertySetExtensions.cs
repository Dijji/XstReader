using XstReader.ElementProperties;

namespace XstReader.App.Controls
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
