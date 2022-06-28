using XstReader.App.Common;
using XstReader.ElementProperties;

namespace XstReader.Razor.Templates
{
    public static class RenderOptions
    {
        public static XstExportOptions ExportOptions { get; set; } = new XstExportOptions();

        private static Dictionary<PropertyCanonicalName, KnownCanonicalNameProperty>? _UsedProperties = null;
        internal static Dictionary<PropertyCanonicalName, KnownCanonicalNameProperty> UsedProperties 
            => _UsedProperties ??= new Dictionary<PropertyCanonicalName, KnownCanonicalNameProperty>();
        
        internal static void AddUsedProperty(PropertyCanonicalName canonicalName)
        {
            if(!UsedProperties.ContainsKey(canonicalName))
            {
                var prop = KnownCanonicalNames.KnonwnProperties.FirstOrDefault(p => p.CanonicalName == canonicalName.CanonicalName());
                if (prop != null)
                    UsedProperties.Add(canonicalName, prop);
            }
        }

        public static void Initialize()
        {
            _UsedProperties = null;
        }
    }
}
