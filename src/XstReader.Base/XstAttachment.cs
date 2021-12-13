// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System;
using System.Collections.Generic;
using XstReader.Common.BTrees;
using XstReader.Properties;

namespace XstReader
{
    public class XstAttachment
    {
        private List<XstProperty> properties = null;

        public XstMessage Message { get; set; }
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
        internal AttachMethod AttachMethod { get; set; }
        internal dynamic Content { get; set; }
        public bool IsFile { get { return AttachMethod == AttachMethod.afByValue; } }
        public bool IsEmail { get { return /*AttachMethod == AttachMethods.afStorage ||*/ AttachMethod == AttachMethod.afEmbeddedMessage; } }
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

        public List<XstProperty> Properties
        {
            get
            {
                // We read the full set of attachment property values only on demand
                if (properties == null)
                {
                    properties = new List<XstProperty>();
                    if (!WasLoadedFromMime)
                    {
                        properties.AddRange(Message.Folder.XstFile.ReadAttachmentProperties(this));
                    }
                }
                return properties;
            }
        }

        public XstAttachment()
        {

        }

        public XstAttachment(string fileName, byte[] content)
        {
            LongFileName = fileName;
            AttachMethod = AttachMethod.afByValue;
            Size = content.Length;
            this.Content = content;
            WasLoadedFromMime = true;
        }

        public XstAttachment(string fileName, string contentId, Byte[] content)
            : this(fileName, content)
        {
            ContentId = contentId;
            Flags = AttachFlags.attRenderedInBody;
        }
    }

}
