// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021,2022 iluvadev, and released under Ms-PL License.
// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XstReader.Common;
using XstReader.Common.BTrees;
using XstReader.ElementProperties;


namespace XstReader
{
    /// <summary>
    /// Class for a Message stored inside an ost/pst file
    /// Holds information about a single message, extracted from the xst tables
    /// </summary>
    public partial class XstMessage : XstElement
    {
        private static RtfDecompressor RtfDecompressor = new RtfDecompressor();

        /// <summary>
        /// The Folder of this Message
        /// </summary>
        [DisplayName("Parent Folder")]
        [Category("General")]
        [Description("The Folder of this Message")]
        public XstFolder ParentFolder { get; private set; }
        /// <summary>
        /// If is an Attached Message, contains the Attachement
        /// </summary>
        [DisplayName("Parent Attachment")]
        [Category("General")]
        [Description("If is an Attached Message, contains the Attachement")]
        public XstAttachment ParentAttachment { get; private set; }
        /// <summary>
        /// The Container File
        /// </summary>
        [DisplayName("File")]
        [Category("General")]
        [Description("The Container File")]
        public override XstFile XstFile => ParentFolder.XstFile;


        private XstRecipientSet _Recipients = null;
        /// <summary>
        /// The Recipients involved in the Message
        /// </summary>
        [Browsable(false)]
        public XstRecipientSet Recipients => _Recipients ?? (_Recipients = new XstRecipientSet(this));

        /// <summary>
        /// The Subject of the Message
        /// </summary>
        [DisplayName("Subject")]
        [Category(@"General Message Properties")]
        [Description(@"Contains the subject of the email message.")]
        public string Subject => Properties[PropertyCanonicalName.PidTagSubject]?.ValueAsStringSanitized;

        /// <summary>
        /// The Cc Summary of the Message
        /// </summary>
        [DisplayName("Display Cc")]
        [Category(@"Message Properties")]
        [Description(@"Contains a list of carbon copy (Cc) recipient display names.")]
        public string Cc => Properties[PropertyCanonicalName.PidTagDisplayCc]?.ValueAsStringSanitized;

        /// <summary>
        /// The To Summary of the Message
        /// </summary>
        [DisplayName("Display To")]
        [Category(@"Message Properties")]
        [Description(@"Contains a list of the primary recipient display names, separated by semicolons, when an email message has primary recipients .")]
        public string To => Properties[PropertyCanonicalName.PidTagDisplayTo]?.ValueAsStringSanitized;
        /// <summary>
        /// The From Summary of the Message
        /// </summary>
        [DisplayName("Sender Name")]
        [Category(@"Address Properties")]
        [Description(@"Contains the display name of the sending mailbox owner.")]
        public string From => Properties[PropertyCanonicalName.PidTagSenderName]?.ValueAsStringSanitized;

        /// <summary>
        /// Indicates if the Message is sent in representation of other 
        /// </summary>
        [DisplayName("Is Sent Representing Other")]
        [Category("General")]
        [Description(@"Indicates if the Message is sent in representation of other.")]
        public bool IsSentRepresentingOther
        {
            get
            {
                var sender = Recipients[RecipientType.Sender].FirstOrDefault();
                var sentRepresenting = Recipients[RecipientType.SentRepresenting].FirstOrDefault();
                return (sender?.DisplayName != sentRepresenting?.DisplayName || sender?.Address != sentRepresenting?.Address);
            }
        }
        /// <summary>
        /// Indicates if the Messages was received representing other
        /// </summary>
        [DisplayName("Is Received Representing Other")]
        [Category("General")]
        [Description(@"Indicates if the Messages was received representing other.")]
        public bool IsReceivedRepresentingOther
        {
            get
            {
                var receiver = Recipients[RecipientType.ReceivedBy].FirstOrDefault();
                var receivedRepresenting = Recipients[RecipientType.ReceivedRepresenting].FirstOrDefault();
                return (receiver?.DisplayName != receivedRepresenting?.DisplayName || receiver?.Address != receivedRepresenting?.Address);
            }
        }

        private MessageFlags? _Flags = null;
        /// <summary>
        /// The Flags of the Message
        /// </summary>
        [DisplayName("Message Flags")]
        [Category(@"General Message Properties")]
        [Description(@"Specifies the status of the Message object.")]
        public MessageFlags? Flags
        {
            get => _Flags ?? (MessageFlags?)Properties[PropertyCanonicalName.PidTagMessageFlags]?.Value;
            private set => _Flags = value;
        }

        /// <summary>
        /// The Status of the Message
        /// </summary>
        [DisplayName("Message Status")]
        [Category(@"General Message Properties")]
        [Description(@"Specifies the status of a message in a contents table.")]
        public MessageStatus? Status => (MessageStatus?)Properties[PropertyCanonicalName.PidTagMessageStatus]?.Value;

        /// <summary>
        /// DateTime when Message was submitted
        /// </summary>
        [DisplayName("Client Submit Time")]
        [Category(@"Message Time Properties")]
        [Description(@"Contains the current time, in UTC, when the email message is submitted.")]
        public DateTime? SubmittedTime => Properties[PropertyCanonicalName.PidTagClientSubmitTime]?.Value;
        /// <summary>
        /// DateTime when Message was received
        /// </summary>
        [DisplayName("Message Delivery Time")]
        [Category(@"Message Time Properties")]
        [Description(@"Specifies the time (in UTC) when the server received the message.")]
        public DateTime? ReceivedTime => Properties[PropertyCanonicalName.PidTagMessageDeliveryTime]?.Value;
        /// <summary>
        /// DateTime of the Message (Received or Submitted)
        /// </summary>
        [DisplayName("Date")]
        [Category("General")]
        [Description(@"DateTime of the Message (Received or Submitted)")]
        public DateTime? Date => ReceivedTime ?? SubmittedTime;

        /// <summary>
        /// The Priority of the message
        /// </summary>
        [DisplayName("Priority")]
        [Category(@"Email")]
        [Description(@"Indicates the client's request for the priority with which the message is to be sent by the messaging system.")]
        public MessagePriority? Priority => (MessagePriority?)Properties[PropertyCanonicalName.PidTagPriority]?.Value;

        /// <summary>
        /// The Importance of the Message
        /// </summary>
        [DisplayName("Importance")]
        [Category(@"General Message Properties")]
        [Description(@"Indicates the level of importance assigned by the end user to the Message object.")]
        public MessageImportance? Importance => (MessageImportance?)Properties[PropertyCanonicalName.PidTagImportance]?.Value;

        /// <summary>
        /// The Sensitivity of the Message
        /// </summary>
        [DisplayName("Sensitivity")]
        [Category(@"General Message Properties")]
        [Description(@"Indicates the sender's assessment of the sensitivity of the Message object.")]
        public MessageSensitivity? Sensitivity => (MessageSensitivity?)Properties[PropertyCanonicalName.PidTagSensitivity]?.Value;

        /// <summary>
        /// The Internet Message Id
        /// </summary>
        [DisplayName("Internet Message Id")]
        [Category(@"MIME Properties")]
        [Description(@"Corresponds to the message-id field.")]
        public string InternetMessageId => Properties[PropertyCanonicalName.PidTagInternetMessageId]?.Value;

        private Func<BTree<Node>> _BodyLoader = null;
        internal Func<BTree<Node>> BodyLoader
        {
            get => _BodyLoader;
            set
            {
                if (_BodyLoader != null)
                    ClearContents();
                _BodyLoader = value;
            }
        }

        private Encoding _Encoding = null;
        /// <summary>
        /// The Message Encoding
        /// </summary>
        [DisplayName("Encoding")]
        [Category("General")]
        [Description(@"The Message Encoding")]
        public Encoding Encoding => _Encoding ?? (_Encoding = GetEncoding());
        private BodyType? _NativeBody = null;
        internal BodyType NativeBody
        {
            get => _NativeBody ?? Properties[PropertyCanonicalName.PidTagNativeBody]?.Value as BodyType? ?? BodyType.Undefined;
            private set => _NativeBody = value;
        }
        private XstMessageBody _Body = null;
        /// <summary>
        /// The Body of the Message
        /// </summary>
        [Browsable(false)]
        public XstMessageBody Body => GetBody();
        /// <summary>
        /// Indicates if the Message was been read
        /// </summary>
        [DisplayName("Is Read")]
        [Category("General")]
        [Description(@"Indicates if the Message was been read")]
        public bool IsRead => Flags?.HasFlag(MessageFlags.mfRead) ?? true;

        /// <summary>
        /// Indicates if the Message is a Draft (unsent message)
        /// </summary>
        [DisplayName("Is Draft")]
        [Category("General")]
        [Description(@"Indicates if the Message is a Draft (unsent message)")]
        public bool IsDraft => Flags?.HasFlag(MessageFlags.mfUnsent) ?? false;


        private IEnumerable<XstAttachment> _Attachments = null;
        /// <summary>
        /// The Attachments of the Message
        /// </summary>
        [Browsable(false)]
        public IEnumerable<XstAttachment> Attachments => GetAttachments();
        /// <summary>
        /// The Files Attached to the Message
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.Files() instead")]
        [Browsable(false)]
        public IEnumerable<XstAttachment> AttachmentsFiles => Attachments.Files();
        /// <summary>
        /// The Visible Files Attached to the Message
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.VisibleFiles() instead")]
        [Browsable(false)]
        public IEnumerable<XstAttachment> AttachmentsVisibleFiles => Attachments.VisibleFiles();
        /// <summary>
        /// Indicates if the Message has Attachments
        /// </summary>
        [DisplayName("Has Attachments")]
        [Category("General")]
        [Description(@"Indicates if the Message has Attachments")]
        public bool HasAttachments => Flags?.HasFlag(MessageFlags.mfHasAttach) ?? false;
        /// <summary>
        /// Indicates if the Message may have Attachments inline, incrusted in the body
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.Inlines().Any() instead")]
        [Browsable(false)]

        public bool MayHaveAttachmentsInline => Attachments.Inlines().Any();
        /// <summary>
        /// Indicates if the Message has any File attached
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.Files().Any() instead")]
        [Browsable(false)]

        public bool HasAttachmentsFiles => Attachments.Files().Any();
        /// <summary>
        /// Indicates if the Message has any Visible File attached
        /// </summary>
        [Obsolete("This property is Obsolete. Use Attachments.VisibleFiles().Any() instead")]
        [Browsable(false)]

        public bool HasAttachmentsVisibleFiles => Attachments.VisibleFiles().Any();

        /// <summary>
        /// Indicates if the Message is Encrypted or signed
        /// </summary>
        [DisplayName("Is Encrypted or Signed")]
        [Category("General")]
        [Description(@"Indicates if the Message is Encrypted or signed")]
        public bool IsEncryptedOrSigned => Attachments.Count() == 1 &&
                                           Attachments.First().FileNameForSaving == "smime.p7m" &&
                                           !Properties.Contains(PropertyCanonicalName.PidTagBody) &&
                                           !Properties.Contains(PropertyCanonicalName.PidTagHtml);

        private BTree<Node> _SubNodeTreeProperties = null;
        internal BTree<Node> SubNodeTreeProperties
        {
            get => _SubNodeTreeProperties ?? (_SubNodeTreeProperties = BodyLoader?.Invoke());
            set => _SubNodeTreeProperties = value;
        }
        internal BTree<Node> SubNodeTreeParentAttachment = null;

        /// <summary>
        /// Indicates if the Message is an attachment of other Message
        /// </summary>
        [DisplayName("Is Attached")]
        [Category("General")]
        [Description(@"Indicates if the Message is an attachment of other Message")]
        public bool IsAttached => SubNodeTreeParentAttachment != null;

        #region Content Exclusions
        private static readonly HashSet<PropertyCanonicalName> ContentExclusions = new HashSet<PropertyCanonicalName>
        {
            PropertyCanonicalName.PidTagNativeBody,
            PropertyCanonicalName.PidTagBody,
            PropertyCanonicalName.PidTagHtml,
            PropertyCanonicalName.PidTagRtfCompressed,
        };

        #endregion Content Exclusions

        /// <summary>
        /// Ctor
        /// </summary>
        public XstMessage() : base(XstElementType.Message)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="parentFolder"></param>
        /// <param name="parentAttachment"></param>
        internal XstMessage(XstFolder parentFolder, XstAttachment parentAttachment) : this()
        {
            ParentFolder = parentFolder;
            ParentAttachment = parentAttachment;
        }

        /// <summary>
        /// Initialization for Messages in a Folder
        /// </summary>
        /// <param name="nid"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        internal void Initialize(NID nid, XstFolder folder)
        {
            Nid = nid;
            ParentFolder = folder;

            // Read the contents properties
            //BodyLoader = () => Ltp.ReadProperties<XstMessage>(Nid, PropertyGetters.MessageContentProperties, this);
            BodyLoader = () => Ltp.ReadProperties(Nid, Properties);
        }

        #region Properties
        private protected override bool CheckProperty(PropertyCanonicalName tag)
        {
            if (SubNodeTreeParentAttachment != null)
                return Ltp.ContainsProperty(SubNodeTreeParentAttachment, Nid, tag, true);

            if (SubNodeTreeProperties != null)
                return Ltp.ContainsProperty(Nid, tag);

            return false;
        }
        private protected override XstProperty LoadProperty(PropertyCanonicalName tag)
        {
            if (SubNodeTreeParentAttachment != null)
                return Ltp.ReadProperty(SubNodeTreeParentAttachment, Nid, tag, true);

            if (SubNodeTreeProperties != null)
                return Ltp.ReadProperty(Nid, tag);

            return null;
        }

        private protected override IEnumerable<XstProperty> LoadProperties()
        {
            if (SubNodeTreeParentAttachment != null)
                return Ltp.ReadAllProperties(SubNodeTreeParentAttachment, Nid, ContentExclusions, true);

            if (SubNodeTreeProperties != null)
                return Ltp.ReadAllProperties(Nid, ContentExclusions);

            return new XstProperty[0];
        }

        #endregion Properties

        #region Attachments
        /// <summary>
        /// Returns the Attachments of this Message
        /// </summary>
        /// <returns></returns>
        public IEnumerable<XstAttachment> GetAttachments()
            => _Attachments ?? (_Attachments = GetAttachmentsInternal());

        private IEnumerable<XstAttachment> GetAttachmentsInternal()
        {
            if (!HasAttachments)
                return new XstAttachment[0];

            // Read any attachments
            var attachmentsNid = new NID(EnidSpecial.NID_ATTACHMENT_TABLE);
            if (!Ltp.IsTablePresent(SubNodeTreeProperties, attachmentsNid))
                throw new XstException("Could not find expected Attachment table");

            // Read the attachment table, which is held in the subnode of the message
            return Ltp.ReadTable<XstAttachment>(SubNodeTreeProperties, attachmentsNid,
                                                (a, id) => a.Nid = new NID(id),
                                                a => a.Initialize(this, IsAttached));
        }
        #endregion Attachments


        #region Body
        private string BodyPlainText => Properties[PropertyCanonicalName.PidTagBody]?.Value;
        private bool IsBodyPlainText => NativeBody == BodyType.PlainText ||
                                       (NativeBody == BodyType.Undefined && BodyPlainText?.Length > 0);

        private string BodyHtml => Properties[PropertyCanonicalName.PidTagHtml]?.Value as string;

        private byte[] Html => Properties[PropertyCanonicalName.PidTagHtml]?.Value as byte[];
        private bool IsBodyHtml => NativeBody == BodyType.HTML ||
                                  (NativeBody == BodyType.Undefined && (BodyHtml?.Length > 0 || Html?.Length > 0));

        private byte[] BodyRtfCompressed => Properties[PropertyCanonicalName.PidTagRtfCompressed]?.Value;
        private bool IsBodyRtf => NativeBody == BodyType.RTF ||
                                 (NativeBody == BodyType.Undefined && BodyRtfCompressed?.Length > 0);


        /// <summary>
        /// Obtains the Body of the Message
        /// </summary>
        /// <returns></returns>
        public XstMessageBody GetBody()
        {
            if (_Body == null)
            {
                var formatList = GetBodyFormats();
                foreach (var format in formatList)
                {
                    _Body = new XstMessageBody(this, GetBodyText(format), format);
                    if (_Body.Text != null)
                        break;
                }
            }
            return _Body;
        }
        private List<XstMessageBodyFormat> GetBodyFormats()
        {
            var formatList = new List<XstMessageBodyFormat>();
            if (IsBodyHtml)
                formatList.Add(XstMessageBodyFormat.Html);
            if (IsBodyRtf)
                formatList.Add(XstMessageBodyFormat.Rtf);
            if (IsBodyPlainText)
                formatList.Add(XstMessageBodyFormat.PlainText);

            return formatList;
        }

        private string GetBodyText(XstMessageBodyFormat format)
        {
            switch (format)
            {
                case XstMessageBodyFormat.Html: return GetBodyHtmlWithImages();
                case XstMessageBodyFormat.Rtf: return GetBodyRtf();
                case XstMessageBodyFormat.PlainText:
                default: return BodyPlainText;
            }
        }
        private string GetBodyHtmlWithImages()
        {
            if (!IsBodyHtml)
                return null;

            string htmlWithImages = null;
            if (BodyHtml != null)
                htmlWithImages = BodyHtml; // This will be plain ASCII
            else if (Html != null)
            {
                if (Encoding != null)
                    htmlWithImages = EscapeUnicodeCharacters(Encoding.GetString(Html));
            }
            htmlWithImages = EmbedAttachments(htmlWithImages);
            return htmlWithImages;
        }
        private string GetBodyRtf()
        {
            if (!IsBodyRtf)
                return null;

            string rtfText = null;
            if (Encoding != null)
                using (MemoryStream ms = RtfDecompressor.Decompress(BodyRtfCompressed, true))
                    rtfText = Encoding.GetString(ms.ToArray());

            return rtfText;
        }
        #endregion Body

        private void ClearAttachments()
        {
            //if (_Attachments != null)
            //    foreach (var attachment in _Attachments)
            //        attachment.ClearContents();
            _Attachments = null;
        }
        private void ClearRecipients()
        {
            if (_Recipients != null)
                _Recipients.ClearContents();
            _Recipients = null;
        }
        private void ClearBody()
        {
            SubNodeTreeProperties = null;
            _NativeBody = null;
            _Body = null;
        }
        internal override void ClearContentsInternal()
        {
            base.ClearContentsInternal();

            _Flags = null;
            ClearBody();
            ClearAttachments();
            ClearRecipients();
        }

        internal void ProcessSignedOrEncrypted()
        {
            //email is signed and/or encrypted and no body was included
            if (!IsEncryptedOrSigned)
                return;
            XstAttachment a = Attachments.FirstOrDefault();
            if (a == null)
                return;

            byte[] attachmentBytes = new byte[0];

            //get attachment bytes
            using (var ms = new MemoryStream())
            {
                a.SaveToStream(ms);
                attachmentBytes = ms.ToArray();
            }
            ReadSignedOrEncryptedMessage(attachmentBytes);
        }

        // Take encrypted or signed bytes and parse into message object
        private void ReadSignedOrEncryptedMessage(byte[] messageBytes)
        {
            string messageFromBytes = System.Text.Encoding.ASCII.GetString(messageBytes);

            // the message is not encrypted just signed
            if (messageFromBytes.Contains("application/x-pkcs7-signature"))
            {
                ParseMimeMessage(messageFromBytes);
            }
            else
            {
                string decryptedMessage = Crypto.DecryptWithCert(messageBytes);
                string cleartextMessage;

                //Message is only signed
                if (decryptedMessage.Contains("filename=smime.p7m"))
                {
                    cleartextMessage = Crypto.DecodeSigned(decryptedMessage);
                }
                // message is only encrypted not signed
                else
                {
                    cleartextMessage = decryptedMessage;
                }
                ParseMimeMessage(cleartextMessage);
            }

            //remove P7M encrypted file from attachments list
            _Attachments = Attachments.Skip(1);
            // if no attachments left unset the has attachments flag
            if (!Attachments.Any())
            {
                Flags ^= MessageFlags.mfHasAttach;
            }
        }

        //parse mime message into a given message object adds all attachments and inserts inline content to message body
        private void ParseMimeMessage(String mimeText)
        {
            string[] messageParts = GetMimeParts(mimeText);

            foreach (string part in messageParts)
            {
                Dictionary<string, string> partHeaders = GetHeaders(new StringReader(part));
                //message body
                if (partHeaders.Keys.Contains("content-type") && partHeaders["content-type"].Trim().Contains("text/html;"))
                {
                    Body.Text = DecodeQuotedPrintable(partHeaders["mimeBody"]);
                    NativeBody = BodyType.HTML;
                }
                //real attachments
                else if (partHeaders.Keys.Contains("content-disposition") && partHeaders["content-disposition"].Trim().Contains("attachment;"))
                {
                    string filename = Regex.Match(partHeaders["content-disposition"], @"filename=""(.*?)""", RegexOptions.IgnoreCase).Groups[1].Value;
                    byte[] content = Convert.FromBase64String(partHeaders["mimeBody"]);
                    _Attachments = Attachments.Union(new XstAttachment[] { new XstAttachment(filename, content) });
                }
                //inline images
                else if (partHeaders.Keys.Contains("content-id"))
                {
                    string fileName = Regex.Match(partHeaders["content-type"], @".*name=""(.*)""", RegexOptions.IgnoreCase).Groups[1].Value;
                    string contentId = Regex.Match(partHeaders["content-id"], @"<(.*)>", RegexOptions.IgnoreCase).Groups[1].Value;
                    byte[] content = Convert.FromBase64String(partHeaders["mimeBody"]);
                    _Attachments = Attachments.Union(new XstAttachment[] { new XstAttachment(fileName, contentId, content) });
                }
            }
        }


        //parse out mime headers from a mime section
        //returns a dictionary with the header type as the key and its value as the value
        private Dictionary<string, string> GetHeaders(StringReader mimeText)
        {
            Dictionary<string, string> Headers = new Dictionary<string, string>();
            string lastHeader = string.Empty;

            string line;
            while ((!string.IsNullOrEmpty(line = mimeText.ReadLine()) && (line.Trim().Length != 0)))
            {

                //If the line starts with a whitespace it is a continuation of the previous line
                if (Regex.IsMatch(line, @"^\s"))
                {
                    Headers[lastHeader] = Headers[lastHeader] + " " + line.TrimStart('\t', ' ');
                }
                else
                {
                    string headerkey = line.Substring(0, line.IndexOf(':')).ToLower();
                    string value = line.Substring(line.IndexOf(':') + 1).TrimStart(' ');
                    if (value.Length > 0)
                        Headers[headerkey] = line.Substring(line.IndexOf(':') + 1).TrimStart(' ');
                    lastHeader = headerkey;
                }
            }

            string mimeBody = "";
            while ((line = mimeText.ReadLine()) != null)
            {
                mimeBody += line + "\r\n";
            }
            Headers["mimeBody"] = mimeBody;
            return Headers;
        }

        // splits a mime message into its individual parts
        // returns a string[] with the parts
        private string[] GetMimeParts(string mimetext)
        {
            String partRegex = @"\r\n------=_NextPart_.*\r\n";
            string[] test = Regex.Split(mimetext, partRegex);

            return test;
        }

        // decodes quoted printable text
        // returns the decoded text
        private string DecodeQuotedPrintable(string input)
        {
            Regex regex = new Regex(@"(\=[0-9A-F][0-9A-F])+|=\r\n", RegexOptions.IgnoreCase);
            string value = regex.Replace(input, new MatchEvaluator(HexDecoderEvaluator));
            return value;
        }

        //converts hex endcoded values to UTF-8
        //returns the string representation of the hex encoded value
        private string HexDecoderEvaluator(Match m)
        {
            if (m.Groups[1].Success)
            {
                byte[] bytes = new byte[m.Value.Length / 3];

                for (int i = 0; i < bytes.Length; i++)
                {
                    string hex = m.Value.Substring(i * 3 + 1, 2);
                    int iHex = Convert.ToInt32(hex, 16);
                    bytes[i] = Convert.ToByte(iHex);
                }
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            return "";
        }

        #region Save


        #endregion Save

        /// <summary>
        /// Gets the String representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Subject?.Trim() ?? base.ToString();
    }
}
