// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XstReader
{
    // Enums and classes used in property handling
    // Enum names are taken from <MS-PST>

    public enum EpropertyType : UInt16
    {
        PtypBinary = 0x0102,
        PtypBoolean = 0x000b,
        PtypFloating64 = 0x0005,
        PtypGuid = 0x0048,
        PtypInteger16 = 0x0002,
        PtypInteger32 = 0x0003,
        PtypInteger64 = 0x0014,
        PtypMultipleInteger32 = 0x1003,
        PtypObject = 0x000d,
        PtypString = 0x001f,
        PtypString8 = 0x001e,
        PtypMultipleString = 0x101F,
        PtypMultipleBinary = 0x1102,
        PtypTime = 0x0040,
    }

    public enum EpropertyTag : UInt16
    {
        PidTagDisplayName = 0x3001,

        // Root folder
        PidTagRecordKey = 0x0FF9,
        PidTagIpmSubTreeEntryId = 0x35E0,

        // Folder
        PidTagContentCount = 0x3602,
        PidTagSubfolders = 0x360A,

        // Message List
        PidTagSubjectW = 0x0037,
        PidTagDisplayCcW = 0x0E03,
        PidTagDisplayToW = 0x0E04,
        PidTagMessageFlags = 0x0E07,
        PidTagMessageDeliveryTime = 0x0E06,
        PidTagReceivedByName = 0x0040,
        PidTagSentRepresentingNameW = 0x0042,
        PidTagSentRepresentingEmailAddress = 0x0065,
        PidTagSenderName = 0x0C1A,
        PidTagClientSubmitTime = 0x0039,
        PidTagLastModificationTime = 0x3008,

        // Message body
        PidTagNativeBody = 0x1016,
        PidTagBody = 0x1000,
        PidTagInternetCodepage = 0x3FDE,
        PidTagHtml = 0x1013,
        PidTagRtfCompressed = 0x1009,

        // Recipient
        PidTagRecipientType = 0x0c15,
        PidTagEmailAddress = 0x3003,

        // Attachment
        PidTagAttachFilenameW = 0x3704,
        PidTagAttachLongFilename = 0x3707,
        PidTagAttachmentSize = 0x0E20,
        PidTagAttachMethod = 0x3705,
        PidTagAttachMimeTag = 0x370e,
        PidTagAttachContentId = 0x3712,
        PidTagAttachFlags = 0x3714,
        PidTagAttachPayloadClass = 0x371a,
        PidTagAttachDataBinary = 0x3701,
        PidTagAttachmentHidden = 0x7ffe,
        //PidTagAttachDataObject = 0x3701,

        // Named properties
        PidTagNameidStreamGuid = 0x0002,
        PidTagNameidStreamEntry = 0x0003,
        PidTagNameidStreamString = 0x0004,
    }

    // Values of the PidTagNativeBody property
    public enum BodyType : Int32
    {
        Undefined = 0x00000000,
        PlainText = 0x00000001,
        RTF = 0x00000002,
        HTML = 0x00000003,
        ClearSigned = 0x00000004,
    }

    // Values of the PidTagMessageFlags property
    [Flags]
    public enum MessageFlags : Int32
    {
        mfHasAttach = 0x00000010,
    }

    // Values of the PidTagRecipientType property
    [Flags]
    public enum RecipientType : Int32
    {
        Originator = 0x00000000,
        To = 0x00000001,
        Cc = 0x00000002,
        Bcc = 0x00000003,
    }

    // Values of the PidTagAttachMethod property
    public enum AttachMethods : Int32
    {
        afByValue = 0x00000001,
        afEmbeddedMessage = 0x00000005,
        afStorage = 0x00000006,
    }

    // Values of the PidTagAttachFlags property
    public enum AttachFlags : UInt32
    {
        attInvisibleInHtml = 0x00000001,
        attRenderedInBody = 0x00000004,
    }

    // Property getters are used to specify which properties should be retrieved from a property context
    // or table context, and where they should be stored.
    // T is the target object, Action arguments are target object, column value 
    class PropertyGetters<T> : Dictionary<EpropertyTag, Action<T, dynamic>>
    {
    }


    public class Recipient
    {
        public RecipientType RecipientType { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public List<Property> Properties { get; private set; } = new List<Property>();
    }

    public class Property
    {

        public EpropertyTag Tag { get; set; }
        public dynamic Value { get; set; }

        // Standard properties have a Tag value less than 0x8000,
        // and identify a particular property
        //
        // Tag values from 0x8000 to 0x8fff are named properties,
        // where the Tag Is the key into a per .ost or .pst dictionary of properties
        // identified by a GUID (identifying a Property Set) and a name (identifying a property Within that set), 
        // which can be a string or a 32-bit value
        //
        public string Guid { get; set; }        // String representation of hex GUID
        public string GuidName { get; set; }    // Equivalent name, where known e.g. PSETID_Common 
        public UInt32? Lid { get; set; }        // Property identifier, when we know it
        public string Name { get; set; }        // String name of property, when we know it

        public bool IsNamed { get { return (UInt16)Tag >= 0x8000 && (UInt16)Tag <= 0x8fff; } }

        public string DisplayId
        {
            get
            {
                return String.Format("0x{0:x4}", (UInt16)Tag);
            }
        }

        public string Description
        {
            get
            {
                string description;
                if (IsNamed)
                {
                    return String.Format("Guid: {0}\r\nName: {1}",
                        GuidName != null ? GuidName : Guid,
                        Name != null ? Name : String.Format("0x{0:x8}", Lid));
                }
                else if (StandardProperties.TagToDescription.TryGetValue(Tag, out description))
                    return description;
                else
                    return null;
            }
        }

        public string CsvId
        {
            get
            {
                if (IsNamed)
                    // Prefix with 80 In order to ensure they collate last
                    return String.Format("80{0}{1:x8}", Guid, Lid);
                else
                    return String.Format("{0:x4}", (UInt16)Tag);
            }
        }


        public string CsvDescription
        {
            get
            {
                string description;
                if (IsNamed)
                {
                    return String.Format("{0}: {1}",
                        GuidName != null ? GuidName : Guid,
                        Name != null ? Name : String.Format("0x{0:x8}", Lid));
                }
                else if (StandardProperties.TagToDescription.TryGetValue(Tag, out description))
                {
                    if (description.StartsWith("undocumented"))
                        return "undocumented " + DisplayId;
                    else
                        return description;
                }
                else
                    return DisplayId;
            }
        }

        public string DisplayValue
        {
            get
            {
                if (Value is byte[] valBytes)
                    return BitConverter.ToString(valBytes);
                else if (Value is Int32[])
                    return String.Join(", ", Value);
                else if (Value is string[])
                    return String.Join(",\r\n", Value);
                else if (Value is List<byte[]>)
                    return String.Join(",\r\n", ((List<byte[]>)Value).Select(v => BitConverter.ToString(v)));
                else if (Value == null)
                    return null;
                else
                    return Value.ToString();
            }
        }
    }

    public class Attachment
    {
        private List<Property> properties = null;

        public Message Message { get; set; }
        internal BTree<Node> subNodeTreeProperties { get; set; } = null; // Used when handling attachments which are themselves messages
        public string DisplayName { get; set; }
        public string FileNameW { get; set; }
        public string LongFileName { get; set; }
        internal AttachFlags Flags { get; set; }
        public string MimeTag { get; set; }
        public string ContentId { get; set; }
        public bool Hidden { get; set; }
        public string FileName { get { return LongFileName ?? FileNameW; } }
        public int Size { get; set; }
        internal NID Nid { get; set; }
        internal AttachMethods AttachMethod { get; set; }
        internal dynamic Content { get; set; }
        public bool IsFile { get { return AttachMethod == AttachMethods.afByValue; } }
        public bool IsEmail { get { return /*AttachMethod == AttachMethods.afStorage ||*/ AttachMethod == AttachMethods.afEmbeddedMessage; } }
        public bool WasRenderedInline { get; set; } = false;
        public bool WasLoadedFromMime { get; set; } = false;

        public string Type
        {
            get
            {
                if (IsFile)
                    return "File";
                else if (IsEmail)
                    return "Email";
                else
                    return "Other";
            }
        }

        public string Description
        {
            get
            {
                if (IsFile)
                    return FileName;
                else
                    return DisplayName;
            }
        }

        public bool Hide { get { return (Hidden || IsInlineAttachment); } }
        //public FontWeight Weight { get { return Hide ? FontWeights.ExtraLight: FontWeights.SemiBold; } }
        public bool HasContentId { get { return (ContentId != null && ContentId.Length > 0); } }

        // To do: case where ContentLocation property is used instead of ContentId
        public bool IsInlineAttachment
        {
            get
            {
                // It is an in-line attachment either if the flags say it is, or the content ID
                // matched a reference in the body and it was rendered inline
                return ((Flags & AttachFlags.attRenderedInBody) == AttachFlags.attRenderedInBody ||
                        WasRenderedInline) &&
                       HasContentId;
            }
        }

        public List<Property> Properties
        {
            get
            {
                // We read the full set of attachment property values only on demand
                if (properties == null)
                {
                    properties = new List<Property>();
                    if (!WasLoadedFromMime)
                    {
                        properties.AddRange(Message.Folder.XstFile.ReadAttachmentProperties(this));
                    }
                }
                return properties;
            }
        }

        public Attachment()
        {

        }

        public Attachment(string fileName, byte[] content)
        {
            LongFileName = fileName;
            AttachMethod = AttachMethods.afByValue;
            Size = content.Length;
            this.Content = content;
            WasLoadedFromMime = true;
        }

        public Attachment(string fileName, string contentId, Byte[] content)
            : this(fileName, content)
        {
            ContentId = contentId;
            Flags = AttachFlags.attRenderedInBody;
        }
    }

    public class Folder
    {
        public XstFile XstFile { get; set; }
        public string Name { get; set; }
        public uint ContentCount { get; set; } = 0;
        public bool HasSubFolders { get; set; } = false;
        internal NID Nid { get; set; }  // Where folder data is held
        public Folder ParentFolder { get; set; }
        public List<Folder> Folders { get; private set; } = new List<Folder>();
        public List<Message> Messages { get; private set; } = new List<Message>();

        private string _Path = null;
        public string Path => _Path ?? (_Path = (string.IsNullOrEmpty(ParentFolder?.Name)) ? Name : $"{ParentFolder.Path}\\{Name}");
        
        public Message AddMessage(Message m)
        {
            m.Folder = this;
            Messages.Add(m);
            return m;
        }
    }
}
