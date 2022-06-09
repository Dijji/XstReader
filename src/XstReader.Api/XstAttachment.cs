// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021,2022 iluvadev, and released under Ms-PL License.
// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using XstReader.Common.BTrees;
using XstReader.ElementProperties;

namespace XstReader
{
    /// <summary>
    /// Attachment of a Message
    /// </summary>
    public class XstAttachment : XstElement
    {
        /// <summary>
        /// The container Message
        /// </summary>
        [DisplayName("Message")]
        [Category("General")]
        [Description("The Container Message")]
        public XstMessage Message { get; internal set; }
        /// <summary>
        /// The Container Folder
        /// </summary>
        [DisplayName("Folder")]
        [Category("General")]
        [Description("The Container Folder")]
        public XstFolder Folder => Message.ParentFolder;

        /// <summary>
        /// The Parents of this Element
        /// </summary>
        [Browsable(false)]
        public override XstElement Parent => Message;

        internal BTree<Node> SubNodeTreeProperties { get; set; } = null; // Used when handling attachments which are themselves messages

        /// <summary>
        /// The Name of the Element
        /// </summary>
        [DisplayName("Display Name")]
        [Category(@"Mapi Common")]
        [Description(@"Contains the display name of the folder.")]
        public override string DisplayName
        {
            get => base.DisplayName ?? LongFileName;
            protected set => base.DisplayName = value;
        }

        /// <summary>
        /// The FileName of the Attachment
        /// </summary>
        [DisplayName("Attach Filename")]
        [Category(@"Message Attachment Properties")]
        [Description(@"Contains the 8.3 name of the PidTagAttachLongFilename property (section 2.595).")]
        public string FileName => Properties[PropertyCanonicalName.PidTagAttachFilename]?.ValueAsStringSanitized;
        /// <summary>
        /// The Description of the Attachment (DisplayName or FileName)
        /// </summary>
        [DisplayName("Description")]
        [Category("General")]
        [Description("The Description of the Attachment (DisplayName or FileName)")]
        public string Description => DisplayName ?? FileName;

        private string _LongFileName = null;
        /// <summary>
        /// The LongFileName of the Attachment
        /// </summary>
        [DisplayName("Attach Long Filename")]
        [Category(@"Message Attachment Properties")]
        [Description(@"Contains the full filename and extension of the Attachment object.")]
        public string LongFileName => _LongFileName ?? (_LongFileName = Properties[PropertyCanonicalName.PidTagAttachLongFilename]?.ValueAsStringSanitized);
        private int? _Size = null;
        /// <summary>
        /// The Size (in bytes) of the Attachement file
        /// </summary>
        [DisplayName("Attach Size")]
        [Category(@"Message Attachment Properties")]
        [Description(@"Contains the size, in bytes, consumed by the Attachment object on the server.")]
        public int Size => _Size ?? (int)(Properties[PropertyCanonicalName.PidTagAttachSize]?.Value ?? 0);

        private AttachMethod? _AttachMethod = null;
        internal AttachMethod AttachMethod => _AttachMethod ?? (AttachMethod)(Properties[PropertyCanonicalName.PidTagAttachMethod]?.Value ?? 0);
        private AttachFlags? _Flags = null;
        internal AttachFlags Flags => _Flags ?? (AttachFlags)(Properties[PropertyCanonicalName.PidTagAttachFlags]?.Value ?? 0);
        private string _ContentId = null;
        /// <summary>
        /// The ContentId of the Attachment
        /// </summary>
        [DisplayName("Attach Content Id")]
        [Category(@"Message Attachment Properties")]
        [Description(@"Contains a content identifier unique to the Message object that matches a corresponding ""cid:"" URI schema reference in the HTML body of the Message object.")]
        public string ContentId => _ContentId ?? (_ContentId = Properties[PropertyCanonicalName.PidTagAttachContentId]?.ValueAsStringSanitized);
        /// <summary>
        /// Indicates if the Attachment is Hidden
        /// </summary>
        [DisplayName("Attachment Hidden")]
        [Category(@"Message Attachment Properties")]
        [Description(@"Indicates whether an Attachment object is hidden from the end user.")]
        public bool IsHidden => Properties[PropertyCanonicalName.PidTagAttachmentHidden]?.Value ?? false;
        /// <summary>
        /// The FileName used for Savig to disc
        /// </summary>
        [DisplayName("Filename for saving")]
        [Category(@"General")]
        [Description(@"The FileName used for Savig to disc")]
        public string FileNameForSaving => LongFileName ?? FileName;

        private dynamic _Content = null;
        internal dynamic Content => _Content ?? Properties[PropertyCanonicalName.PidTagAttachDataBinary]?.Value;
        /// <summary>
        /// Indicates if the Attachement is a File
        /// </summary>
        [DisplayName("Is File")]
        [Category(@"General")]
        [Description(@"Indicates if the Attachement is a File")]
        public bool IsFile => AttachMethod == AttachMethod.afByValue;
        /// <summary>
        /// Indicates if the Attachment is an Email
        /// </summary>
        [DisplayName("Is Email")]
        [Category(@"General")]
        [Description(@"Indicates if the Attachement is an Email")]
        public bool IsEmail => AttachMethod == AttachMethod.afEmbeddedMessage;
        //public bool IsEmail { get { return /*AttachMethod == AttachMethods.afStorage ||*/ AttachMethod == AttachMethod.afEmbeddedMessage; } }

        /// <summary>
        /// Indicates if the Attachment was rendered inside the body
        /// </summary>
        [DisplayName("Was Rendered Inline")]
        [Category(@"General")]
        [Description(@"Indicates if the Attachment was rendered inside the body")]
        public bool WasRenderedInline { get; internal set; } = false;
        /// <summary>
        /// Indicates if the Attachment was loaded from Mime
        /// </summary>
        [DisplayName("Was Loaded from Mime")]
        [Category(@"General")]
        [Description(@"Indicates if the Attachment was loaded from Mime")]
        public bool WasLoadedFromMime { get; internal set; } = false;

        /// <summary>
        /// Indicates the Type of the Attachment
        /// </summary>
        [DisplayName("Type")]
        [Category(@"General")]
        [Description(@"Indicates the Type of the Attachment")]
        public XstAttachmentType Type
            => IsFile ? XstAttachmentType.File
               : IsEmail ? XstAttachmentType.Email
               : XstAttachmentType.Other;

        /// <summary>
        /// Indicates if the Attachment should be Hidden (Is Hidden or rendered in the body) 
        /// </summary>
        [DisplayName("Hide")]
        [Category(@"General")]
        [Description(@"Indicates if the Attachment should be Hidden (Is Hidden or rendered in the body)")]
        public bool Hide => IsHidden || IsInlineAttachment;
        //public FontWeight Weight { get { return Hide ? FontWeights.ExtraLight: FontWeights.SemiBold; } }

        /// <summary>
        /// Indicates if the Attachment has ContentId
        /// </summary>
        [DisplayName("Has Content Id")]
        [Category(@"General")]
        [Description(@"Indicates if the Attachment has ContentId")]
        public bool HasContentId => ContentId != null && ContentId.Length > 0;

        // To do: case where ContentLocation property is used instead of ContentId
        /// <summary>
        /// Indicates if the Attachment is an Inline Attachement
        /// </summary>
        [DisplayName("Is Inline Attachment")]
        [Category(@"General")]
        [Description(@"Indicates if the Attachment is an Inline Attachement")]
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
        /// <summary>
        /// If the Attachment is an email, contains the Message
        /// </summary>
        [DisplayName("Attached Email Message")]
        [Category(@"General")]
        [Description(@"If the Attachment is an email, contains the Message")]
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
                _AttachedEmailMessage.BodyLoader = () => Ltp.ReadProperties(subNodeTreeAttachment, _AttachedEmailMessage.Nid, _AttachedEmailMessage.Properties, true);
            }
            else
                throw new XstException("Unexpected data type for attached message");

            return _AttachedEmailMessage;
        }
        private protected override bool CheckProperty(PropertyCanonicalName tag)
        {
            if (WasLoadedFromMime)
                return false;

            BTree<Node> subNodeTreeMessage = SubNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                Ndb.LookupNodeAndReadItsSubNodeBtree(Message.Nid, out subNodeTreeMessage);

            // Read property
            return Ltp.ContainsProperty(subNodeTreeMessage, Nid, tag, true);
        }

        private protected override XstProperty LoadProperty(PropertyCanonicalName tag)
        {
            if (WasLoadedFromMime)
                return null;

            BTree<Node> subNodeTreeMessage = SubNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                Ndb.LookupNodeAndReadItsSubNodeBtree(Message.Nid, out subNodeTreeMessage);

            // Read property
            return Ltp.ReadProperty(subNodeTreeMessage, Nid, tag, true);
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

        /// <summary>
        /// Ctor
        /// </summary>
        public XstAttachment() : base(XstElementType.Attachment)
        {

        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public XstAttachment(string fileName, byte[] content) : this()
        {
            _LongFileName = fileName;
            _AttachMethod = AttachMethod.afByValue;
            _Size = content.Length;
            _Content = content;
            WasLoadedFromMime = true;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contentId"></param>
        /// <param name="content"></param>
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
        /// <summary>
        /// Save the Attachement to a folder
        /// </summary>
        /// <param name="folderpath"></param>
        /// <param name="creationTime"></param>
        public void SaveToFolder(string folderpath, DateTime? creationTime = null)
        {
            var fullFileName = System.IO.Path.Combine(folderpath, FileNameForSaving);

            // If the result is too long, truncate the attachment name as required
            if (fullFileName.Length >= MaxPath)
            {
                var ext = System.IO.Path.GetExtension(FileNameForSaving);
                var att = System.IO.Path.GetFileNameWithoutExtension(FileNameForSaving).Truncate(MaxPath - folderpath.Length - ext.Length - 5) + ext;
                fullFileName = System.IO.Path.Combine(folderpath, att);
            }
            SaveToFile(fullFileName, creationTime);
        }
        /// <summary>
        /// Save the attachment to a file
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="creationTime"></param>
        public void SaveToFile(string fullFileName, DateTime? creationTime = null)
        {
            using (var afs = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
            {
                SaveToStream(afs);
            }
            if (creationTime == null)
                creationTime = LastModificationTime;

            if (creationTime != null)
                System.IO.File.SetCreationTime(fullFileName, (DateTime)creationTime);
        }
        /// <summary>
        /// Save the Attachment to a Stream
        /// </summary>
        /// <param name="s"></param>
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

        private void ClearAttachedEmailMessage()
        {
            //if (_AttachedEmailMessage != null)
            //    _AttachedEmailMessage.ClearContentsInternal();
            _AttachedEmailMessage = null;
        }
        private void ClearAttachmentContent()
        {
            _Content = null;
        }

        internal override void ClearContentsInternal()
        {
            base.ClearContentsInternal();
            ClearAttachedEmailMessage();
            ClearAttachmentContent();
        }

        /// <summary>
        /// Gets the String representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => LongFileName ?? FileName ?? base.ToString();
    }

}
