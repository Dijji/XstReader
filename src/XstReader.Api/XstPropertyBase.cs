using System;
using System.Collections.Generic;
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// A definition of a Property of a pst/ost element
    /// </summary>
    public class XstPropertyBase
    {
        /// <summary>
        /// Canonical Name of the property 
        /// (from <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/../ms-oxcfold/c0f31b95-c07f-486c-98d9-535ed9705fbf">
        /// [MS-OXCFOLD]</see>)
        /// </summary>
        public PropertyCanonicalName Tag { get; internal set; }

        /// <summary>
        /// The Type of the Property
        /// </summary>
        public PropertyType PropertyType { get; internal set; }

        private XstPropertyBase NonDocumentedNamedProperty => NonDocumentedNamedProperties.FirstOrDefault(t => t.Tag == Tag);

        // Standard properties have a Tag value less than 0x8000,
        // and identify a particular property
        //
        // Tag values from 0x8000 to 0x8fff are named properties,
        // where the Tag Is the key into a per .ost or .pst dictionary of properties
        // identified by a GUID (identifying a Property Set) and a name (identifying a property Within that set), 
        // which can be a string or a 32-bit value

        private PropertySet? _PropertySet = null;
        /// <summary>
        /// The PropertySet 
        /// </summary>
        public PropertySet? PropertySet
        {
            get => _PropertySet ?? (_PropertySet = Tag.PropertySet() ?? NonDocumentedNamedProperty?.PropertySet);
            set => _PropertySet = value;
        }

        /// <summary>
        /// String representation of hex GUID of its PropertySet
        /// </summary>
        [Obsolete("This property is obsolete. Use PropertySet?.Guid()?.ToString() instead")]
        public string PropertySetGuid => PropertySet?.Guid()?.ToString();

        /// <summary>
        /// The name of its PropertySet, e.g. PSETID_Common 
        /// </summary>
        [Obsolete("This property is obsolete. Use PropertySet?.ToString() instead")]
        public string PropertySetName => PropertySet?.ToString();


        private string _Name = null;
        /// <summary>
        /// The Name of the Property, when we know it
        /// </summary>
        public string Name
        {
            get => _Name ?? (_Name = Tag.CanonicalName() ?? NonDocumentedNamedProperty?.Name ?? Tag.ToString());
            private set => _Name = value;
        }

        internal bool IsNamed => (UInt16)Tag >= 0x8000 && (UInt16)Tag <= 0x8fff;
        /// <summary>
        /// The string Id of the Property
        /// </summary>
        public string Id => Tag.Id0x();
        /// <summary>
        /// The Canonical Name of the Property, when we know it 
        /// </summary>
        public string CanonicalName => Tag.CanonicalName();
        /// <summary>
        /// The "Frienly Name" of the Property, when we know it
        /// </summary>
        public string FriendlyName => NonDocumentedNamedProperty?.Name ?? Tag.FriendlyName();

        /// <summary>
        /// The Area of the Property, when we know it
        /// </summary>
        public string Area => Tag.PropertyArea()?.FriendlyName();
        /// <summary>
        /// The Description of the Property, when we know it
        /// </summary>
        public string Description => Tag.Description();

        /// <summary>
        /// The Description of the Property, when we know it, in HTML format
        /// </summary>
        public string HtmlDescription => Tag.HtmlDescription();


        /// <summary>
        /// Ctor
        /// </summary>
        public XstPropertyBase()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="propertyType"></param>
        public XstPropertyBase(PropertyCanonicalName tag, PropertyType propertyType)
        {
            Tag = tag;
            PropertyType = propertyType;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="name"></param>
        /// <param name="propertySet"></param>
        /// <param name="propertyType"></param>
        private XstPropertyBase(UInt16 tagId, string name, PropertySet propertySet, PropertyType propertyType)
            : this((PropertyCanonicalName)tagId, propertyType)
        {
            Name = name;
            PropertySet = propertySet;
            PropertyType = propertyType;
        }

        private static List<XstPropertyBase> NonDocumentedNamedProperties = new List<XstPropertyBase>
        {
            new XstPropertyBase(0x000B, "PidLidSingleInvite", ElementProperties.PropertySet.PSETID_Meeting, PropertyType.PT_BOOLEAN),
            new XstPropertyBase(0x001D, "PidLidAllAttendeesList", ElementProperties.PropertySet.PSETID_Meeting, PropertyType.PT_UNICODE),

            new XstPropertyBase(0x8027, "PidLidEmailList", ElementProperties.PropertySet.PSETID_Address, PropertyType.PT_MV_LONG),
            new XstPropertyBase(0x8086, "PidLidEmail1RichTextFormat", ElementProperties.PropertySet.PSETID_Address, PropertyType.PT_BOOLEAN),
            new XstPropertyBase(0x8091, "PidLidEmail2EntryId", ElementProperties.PropertySet.PSETID_Address, PropertyType.PT_BINARY),
            new XstPropertyBase(0x8096, "PidLidEmail2RichTextFormat", ElementProperties.PropertySet.PSETID_Address, PropertyType.PT_BOOLEAN),
            new XstPropertyBase(0x80A1, "PidLidEmail3EntryId", ElementProperties.PropertySet.PSETID_Address, PropertyType.PT_BINARY),
            new XstPropertyBase(0x80A6, "PidLidEmail3RichTextFormat", ElementProperties.PropertySet.PSETID_Address, PropertyType.PT_BOOLEAN),

            new XstPropertyBase(0x823E, "PidLidTrustRecipientHighlights", ElementProperties.PropertySet.PSETID_Task, PropertyType.PT_BOOLEAN),

            new XstPropertyBase(0x8200, "PidLidSendMeetingAsIcal", ElementProperties.PropertySet.PSETID_Appointment, PropertyType.PT_BOOLEAN),

            new XstPropertyBase(0x8A21, "PidLidSharingExtensionXml", ElementProperties.PropertySet.PSETID_Sharing, PropertyType.PT_UNICODE),

            new XstPropertyBase(0x8F05, "PidLidRemoteTransferSize", ElementProperties.PropertySet.Unknown2, PropertyType.PT_LONG),
            new XstPropertyBase(0x8F07, "PidLidRemoteAttachment", ElementProperties.PropertySet.Unknown2, PropertyType.PT_BOOLEAN),

            new XstPropertyBase(0x0000, "PidLitOther", ElementProperties.PropertySet.PS_PUBLIC_STRINGS, PropertyType.PT_UNICODE),
        };

    }
}
