// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System;
using System.Collections.Generic;
using System.IO;
using XstReader.Common.BTrees;
using XstReader.ElementProperties;

namespace XstReader
{
    public class XstAttachment : XstElement
    {
        public XstMessage Message { get; internal set; }
        public XstFolder Folder => Message.ParentFolder;
        protected internal override XstFile XstFile => Message.XstFile;

        internal BTree<Node> SubNodeTreeProperties { get; set; } = null; // Used when handling attachments which are themselves messages
        public string DisplayName => XstPropertySet[PropertyCanonicalName.PidTagDisplayName]?.Value;
        public string FileName => XstPropertySet[PropertyCanonicalName.PidTagAttachFilename]?.Value;

        private string _LongFileName = null;
        public string LongFileName => _LongFileName ?? XstPropertySet[PropertyCanonicalName.PidTagAttachLongFilename]?.Value;
        private int? _Size = null;
        public int Size => _Size ?? (int)(XstPropertySet[PropertyCanonicalName.PidTagAttachSize]?.Value ?? 0);

        private AttachMethod? _AttachMethod = null;
        internal AttachMethod AttachMethod => _AttachMethod ?? (AttachMethod)(XstPropertySet[PropertyCanonicalName.PidTagAttachMethod]?.Value ?? 0);
        private AttachFlags? _Flags = null;
        internal AttachFlags Flags => _Flags ?? (AttachFlags)(XstPropertySet[PropertyCanonicalName.PidTagAttachFlags]?.Value ?? 0);
        public string MimeTag => XstPropertySet[PropertyCanonicalName.PidTagAttachMimeTag]?.Value;
        private string _ContentId = null;
        public string ContentId => _ContentId ?? XstPropertySet[PropertyCanonicalName.PidTagAttachContentId]?.Value;
        public bool IsHidden => XstPropertySet[PropertyCanonicalName.PidTagAttachmentHidden]?.Value ?? false;

        public string FileNameForSaving => LongFileName ?? FileName;

        private dynamic _Content = null;
        internal dynamic Content => _Content ?? XstPropertySet[PropertyCanonicalName.PidTagAttachDataBinary]?.Value;
        public bool IsFile => AttachMethod == AttachMethod.afByValue;
        //public bool IsEmail { get { return /*AttachMethod == AttachMethods.afStorage ||*/ AttachMethod == AttachMethod.afEmbeddedMessage; } }
        public bool IsEmail => AttachMethod == AttachMethod.afEmbeddedMessage;
        public bool WasRenderedInline { get; set; } = false;
        public bool WasLoadedFromMime { get; set; } = false;

        public XstAttachmentType Type => IsFile ? XstAttachmentType.File : IsEmail ? XstAttachmentType.Email : XstAttachmentType.Other;

        public string Description => IsFile ? FileNameForSaving : DisplayName;

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
        private XstMessage _AttachedEmailMessage = null;
        public XstMessage AttachedEmailMessage => GetAttachedEmailMessage();
        private XstMessage GetAttachedEmailMessage()
        {
            if (_AttachedEmailMessage != null)
                return _AttachedEmailMessage;
            if (!IsEmail)
                return null;

            BTree<Node> subNodeTreeMessage = SubNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                Ndb.LookupNodeAndReadItsSubNodeBtree(Message.Nid, out subNodeTreeMessage);

            var subNodeTreeAttachment = Ltp.ReadProperties(subNodeTreeMessage, Nid, this);
            if (Content.GetType() == typeof(PtypObjectValue))
            {
                _AttachedEmailMessage = new XstMessage(parentFolder: Folder, parentAttachment: this)
                {
                    Nid = new NID(((PtypObjectValue)Content).Nid),
                    SubNodeTreeParentAttachment = subNodeTreeAttachment,
                };

                // Read the basic and contents properties
                _AttachedEmailMessage.BodyLoader = () => Ltp.ReadProperties(subNodeTreeAttachment, _AttachedEmailMessage.Nid, _AttachedEmailMessage.XstPropertySet, true);
            }
            else
                throw new XstException("Unexpected data type for attached message");

            return _AttachedEmailMessage;
        }

        private protected override IEnumerable<XstProperty> LoadProperties()
        {
            if (WasLoadedFromMime)
                return null;

            BTree<Node> subNodeTreeMessage = SubNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                Ndb.LookupNodeAndReadItsSubNodeBtree(Message.Nid, out subNodeTreeMessage);

            // Read all non-content properties
            return Ltp.ReadAllProperties(subNodeTreeMessage, Nid, null, true);
        }

        //private static readonly HashSet<PropertyCanonicalName> attachmentContentExclusions = new HashSet<PropertyCanonicalName>
        //{
        //    PropertyCanonicalName.PidTagAttachDataBinary,
        //};


        public XstAttachment()
        {

        }

        public XstAttachment(string fileName, byte[] content)
        {
            _LongFileName = fileName;
            _AttachMethod = AttachMethod.afByValue;
            _Size = content.Length;
            _Content = content;
            WasLoadedFromMime = true;
        }

        public XstAttachment(string fileName, string contentId, Byte[] content)
            : this(fileName, content)
        {
            _ContentId = contentId;
            _Flags = AttachFlags.attRenderedInBody;
        }

        internal void Initialize(XstMessage message, bool isAttached = false)
        {
            Message = message;

            //// If the long name wasn't in the attachment table, go look for it in the attachment properties
            //if (!XstPropertySet.Contains(PropertyCanonicalName.PidTagAttachLongFilename))
            //    Ltp.ReadProperties(message.SubNodeTreeProperties, Nid, this);

            //// Read properties relating to HTML images presented as attachments
            //Ltp.ReadProperties(message.SubNodeTreeProperties, Nid, this);

            // If this is an embedded email, tell the attachment where to look for its properties
            // This is needed because the email node is not in the main node tree
            if (isAttached)
                SubNodeTreeProperties = message.SubNodeTreeProperties;
        }

        const int MaxPath = 260;
        public void SaveToFolder(string folderpath, DateTime? creationTime)
        {
            var fullFileName = Path.Combine(folderpath, FileNameForSaving);

            // If the result is too long, truncate the attachment name as required
            if (fullFileName.Length >= MaxPath)
            {
                var ext = Path.GetExtension(FileNameForSaving);
                var att = Path.GetFileNameWithoutExtension(FileNameForSaving).Truncate(MaxPath - folderpath.Length - ext.Length - 5) + ext;
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

                var subNodeTreeAttachment = Ltp.ReadProperties<XstAttachment>(subNodeTreeMessage, Nid, this);

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
