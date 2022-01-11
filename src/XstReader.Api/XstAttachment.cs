// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System;
using System.Collections.Generic;
using System.IO;
using XstReader.Common.BTrees;
using XstReader.ElementProperties;

namespace XstReader
{
    public class XstAttachment
    {
        public XstMessage Message { get; internal set; }
        public XstFolder Folder => Message.ParentFolder;
        public XstFile XstFile => Folder.XstFile;
        internal LTP Ltp => XstFile.Ltp;
        internal NDB Ndb => XstFile.Ndb;

        internal BTree<Node> SubNodeTreeProperties { get; set; } = null; // Used when handling attachments which are themselves messages
        public string DisplayName { get; set; }
        public string FileNameW { get; set; }
        public string LongFileName { get; set; }
        internal AttachFlags Flags { get; set; }
        public string MimeTag { get; set; }
        public string ContentId { get; set; }
        public bool IsHidden { get; set; }
        public string FileName => LongFileName ?? FileNameW;
        public int Size { get; set; }
        internal NID Nid { get; set; }
        internal AttachMethod AttachMethod { get; set; }
        internal dynamic Content { get; set; }
        public bool IsFile => AttachMethod == AttachMethod.afByValue;
        //public bool IsEmail { get { return /*AttachMethod == AttachMethods.afStorage ||*/ AttachMethod == AttachMethod.afEmbeddedMessage; } }
        public bool IsEmail => AttachMethod == AttachMethod.afEmbeddedMessage;
        public bool WasRenderedInline { get; set; } = false;
        public bool WasLoadedFromMime { get; set; } = false;

        public XstAttachmentType Type => IsFile ? XstAttachmentType.File : IsEmail ? XstAttachmentType.Email : XstAttachmentType.Other;

        public string Description => IsFile ? FileName : DisplayName;

        public bool Hide => IsHidden || IsInlineAttachment;
        //public FontWeight Weight { get { return Hide ? FontWeights.ExtraLight: FontWeights.SemiBold; } }
        public bool HasContentId => ContentId != null && ContentId.Length > 0;

        // To do: case where ContentLocation property is used instead of ContentId
        public bool IsInlineAttachment
        {
            get
            {
                // It is an in-line attachment either if the flags say it is, or the content ID
                // matched a reference in the body and it was rendered inline
                return ((Flags & AttachFlags.attRenderedInBody) == AttachFlags.attRenderedInBody ||
                        WasRenderedInline) && HasContentId;
            }
        }

        private IEnumerable<XstProperty> _Properties = null;
        public IEnumerable<XstProperty> Properties
        {
            get
            {
                // We read the full set of attachment property values only on demand
                if (_Properties == null)
                {
                    if (!WasLoadedFromMime)
                        _Properties = ReadProperties();
                }
                return _Properties;
            }
        }

        private IEnumerable<XstProperty> ReadProperties()
        {
            BTree<Node> subNodeTreeMessage = SubNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                Ndb.LookupNodeAndReadItsSubNodeBtree(Message.Nid, out subNodeTreeMessage);

            // Read all non-content properties
            return Ltp.ReadAllProperties(subNodeTreeMessage, Nid, XstAttachment.attachmentContentExclusions, true);
        }

        private static readonly HashSet<PropertyCanonicalName> attachmentContentExclusions = new HashSet<PropertyCanonicalName>
        {
            PropertyCanonicalName.PidTagAttachDataBinary,
        };


        public XstAttachment()
        {

        }

        public XstAttachment(string fileName, byte[] content)
        {
            LongFileName = fileName;
            AttachMethod = AttachMethod.afByValue;
            Size = content.Length;
            Content = content;
            WasLoadedFromMime = true;
        }

        public XstAttachment(string fileName, string contentId, Byte[] content)
            : this(fileName, content)
        {
            ContentId = contentId;
            Flags = AttachFlags.attRenderedInBody;
        }

        const int MaxPath = 260;
        public void SaveToFolder(string folderpath, DateTime? creationTime)
        {
            var fullFileName = Path.Combine(folderpath, FileName);

            // If the result is too long, truncate the attachment name as required
            if (fullFileName.Length >= MaxPath)
            {
                var ext = Path.GetExtension(FileName);
                var att = Path.GetFileNameWithoutExtension(FileName).Truncate(MaxPath - folderpath.Length - ext.Length - 5) + ext;
                fullFileName = Path.Combine(folderpath, att);
            }
            SaveToFile(fullFileName, creationTime);
        }

        public void SaveToFile(string fullFileName, DateTime? creationTime)
        {
            using (var afs = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
            {
                SaveToStream(afs);
            }
            if (creationTime != null)
                File.SetCreationTime(fullFileName, (DateTime)creationTime);
        }

        public void SaveToStream(Stream s)
        {
            if (WasLoadedFromMime)
            {
                s.Write(Content, 0, Content.Length);
            }
            else
            {
                BTree<Node> subNodeTreeMessage = SubNodeTreeProperties;

                if (subNodeTreeMessage == null)
                    // No subNodeTree given: assume we can look it up in the main tree
                    Ndb.LookupNodeAndReadItsSubNodeBtree(Message.Nid, out subNodeTreeMessage);

                var subNodeTreeAttachment = Ltp.ReadProperties<XstAttachment>(subNodeTreeMessage, Nid, PropertyGetters.AttachmentContentProperties, this);

                if (Content is object)
                {
                    // If the value is inline, we just write it out
                    if (Content.GetType() == typeof(byte[]))
                    {
                        s.Write(Content, 0, Content.Length);
                    }
                    // Otherwise we need to dereference the node pointing to the data,
                    // using the subnode tree belonging to the attachment
                    else if (Content.GetType() == typeof(NID))
                    {
                        var nb = NDB.LookupSubNode(subNodeTreeAttachment, (NID)Content);

                        // Copy the data to the output file stream without getting it all into memory at once,
                        // as there can be a lot of data
                        Ndb.CopyDataBlocks(s, nb.DataBid);
                    }
                }
            }
        }
    }

}
