// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using XstReader.Common;
using XstReader.Common.BTrees;
using XstReader.ItemProperties;


namespace XstReader
{
    // Holds information about a single message, extracted from the xst tables

    public partial class XstMessage
    {
        private static RtfDecompressor RtfDecompressor = new RtfDecompressor();

        public XstFolder ParentFolder { get; private set; }
        public XstAttachment ParentAttachment { get; private set; }
        private XstFile XstFile => ParentFolder.XstFile;
        private LTP Ltp => XstFile.Ltp;
        private NDB Ndb => XstFile.Ndb;
        internal NID Nid { get; set; }

        private IEnumerable<XstRecipient> _Recipients = null;
        public IEnumerable<XstRecipient> Recipients => GetRecipients();

        public bool HasToDisplayList => Recipients.To().Any();
        public bool HasCcDisplayList => Recipients.Cc().Any();
        public bool HasBccDisplayList => Recipients.Bcc().Any();

        public string Subject => PropertySet[EpropertyTag.PidTagSubjectW]?.Value;
        public string Cc => PropertySet[EpropertyTag.PidTagDisplayCcW]?.Value;
        public string To => PropertySet[EpropertyTag.PidTagDisplayToW]?.Value;
        public string From => PropertySet[EpropertyTag.PidTagSentRepresentingNameW]?.Value ??
                              PropertySet[EpropertyTag.PidTagSentRepresentingEmailAddress]?.Value ??
                              PropertySet[EpropertyTag.PidTagSenderName]?.Value;

        private MessageFlags? _Flags = null;
        public MessageFlags? Flags
        {
            get => _Flags ?? (MessageFlags?)PropertySet[EpropertyTag.PidTagMessageFlags].Value;
            private set => _Flags = value;
        }
        public DateTime? Submitted => PropertySet[EpropertyTag.PidTagClientSubmitTime]?.Value;
        public DateTime? Received => PropertySet[EpropertyTag.PidTagMessageDeliveryTime]?.Value;
        public DateTime? Modified => PropertySet[EpropertyTag.PidTagLastModificationTime]?.Value;

        public DateTime? Date => Received ?? Submitted;

        private bool _IsBodyLoaded = false;
        private Func<BTree<Node>> _BodyLoader = null;
        internal Func<BTree<Node>> BodyLoader
        {
            get => _BodyLoader;
            set
            {
                ClearContents();
                _BodyLoader = value;
            }
        }

        private Encoding _Encoding = null;
        public Encoding Encoding => _Encoding ?? (_Encoding = GetEncoding());
        private BodyType? _NativeBody = null;
        internal BodyType NativeBody
        {
            get => _NativeBody ?? PropertySet[EpropertyTag.PidTagNativeBody]?.Value as BodyType? ?? BodyType.Undefined;
            private set => _NativeBody = value;
        }
        private XstMessageBodyFormat BodyFormat => IsBodyHtml ? XstMessageBodyFormat.Html
                                                   : IsBodyRtf ? XstMessageBodyFormat.Rtf
                                                   : IsBodyPlainText ? XstMessageBodyFormat.PlainText
                                                   : XstMessageBodyFormat.Unknown;
        private XstMessageBody _Body = null;
        public XstMessageBody Body => _Body ?? (_Body = new XstMessageBody(this, GetBodyText(), BodyFormat));

        public string BodyPlainText
        {
            get
            {
                if (!_IsBodyLoaded) LoadBody();
                return PropertySet[EpropertyTag.PidTagBody]?.Value;
            }
        }
        private bool IsBodyPlainText => NativeBody == BodyType.PlainText ||
                                       (NativeBody == BodyType.Undefined && BodyPlainText?.Length > 0);

        private string _BodyHtml = null;
        public string BodyHtml
        {
            get
            {
                if (!_IsBodyLoaded) LoadBody();
                return _BodyHtml ?? PropertySet[EpropertyTag.PidTagHtml]?.Value as string;
            }
            private set => _BodyHtml = value;
        }
        internal byte[] Html
        {
            get
            {
                if (!_IsBodyLoaded) LoadBody();
                return PropertySet[EpropertyTag.PidTagHtml]?.Value as byte[];
            }
        }
        private bool IsBodyHtml => NativeBody == BodyType.HTML ||
                                  (NativeBody == BodyType.Undefined && (BodyHtml?.Length > 0 || Html?.Length > 0));

        internal byte[] BodyRtfCompressed
        {
            get
            {
                if (!_IsBodyLoaded) LoadBody();
                return PropertySet[EpropertyTag.PidTagRtfCompressed]?.Value;
            }
        }
        private bool IsBodyRtf => NativeBody == BodyType.RTF ||
                                 (NativeBody == BodyType.Undefined && BodyRtfCompressed?.Length > 0);

        public bool IsRead => (Flags & MessageFlags.mfRead) == MessageFlags.mfRead;

        private IEnumerable<XstAttachment> _Attachments = null;
        public IEnumerable<XstAttachment> Attachments => GetAttachments();
        public IEnumerable<XstAttachment> AttachmentsFiles => Attachments.Where(a => a.IsFile);
        public IEnumerable<XstAttachment> AttachmentsVisibleFiles => AttachmentsFiles.Where(a => !a.Hide);
        public bool HasAttachments => (Flags & MessageFlags.mfHasAttach) == MessageFlags.mfHasAttach;
        public bool MayHaveAttachmentsInline => Attachments.Any(a => a.HasContentId);
        public bool HasAttachmentsFiles => AttachmentsFiles.Any();
        public bool HasAttachmentsVisibleFiles => HasAttachments && AttachmentsVisibleFiles.Any();

        internal XstPropertySet PropertySet = new XstPropertySet();
        private IEnumerable<XstProperty> _Properties = null;
        public IEnumerable<XstProperty> Properties => GetProperties();
        public bool IsEncryptedOrSigned => BodyHtml == null && Html == null && BodyPlainText == null &&
                                           Attachments.First().FileName == "smime.p7m" && Attachments.Count() == 1;

        internal BTree<Node> SubNodeTreeProperties = null;
        internal BTree<Node> SubNodeTreeParentAttachment = null;
        public bool IsAttached => SubNodeTreeParentAttachment != null;

        #region Content Exclusions
        private static readonly HashSet<EpropertyTag> ContentExclusions = new HashSet<EpropertyTag>
        {
            EpropertyTag.PidTagNativeBody,
            EpropertyTag.PidTagBody,
            EpropertyTag.PidTagHtml,
            EpropertyTag.PidTagRtfCompressed,
        };

        #endregion Content Exclusions

        /// <summary>
        /// Initialization for Messages in a Folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        internal XstMessage Initialize(XstFolder folder)
        {
            ParentFolder = folder;

            // Read the contents properties
            //BodyLoader = () => Ltp.ReadProperties<XstMessage>(Nid, PropertyGetters.MessageContentProperties, this);
            BodyLoader = () => Ltp.ReadProperties(Nid, PropertySet);

            return this;
        }

        public static XstMessage GetAttachedMessage(XstAttachment attachment)
        {
            BTree<Node> subNodeTreeMessage = attachment.SubNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                attachment.Ndb.LookupNodeAndReadItsSubNodeBtree(attachment.Message.Nid, out subNodeTreeMessage);

            var subNodeTreeAttachment = attachment.Ltp.ReadProperties<XstAttachment>(subNodeTreeMessage, attachment.Nid, PropertyGetters.AttachmentContentProperties, attachment);
            if (attachment.Content.GetType() == typeof(PtypObjectValue))
            {
                XstMessage m = new XstMessage
                {
                    Nid = new NID(((PtypObjectValue)attachment.Content).Nid),
                    ParentFolder = attachment.Folder,
                    ParentAttachment = attachment,
                    SubNodeTreeParentAttachment = subNodeTreeAttachment,
                };

                // Read the basic and contents properties
                //m.BodyLoader = () => attachment.Ltp.ReadProperties<XstMessage>(subNodeTreeAttachment, m.Nid, PropertyGetters.MessageAttachmentProperties, m, true);
                m.BodyLoader = () => attachment.Ltp.ReadProperties(subNodeTreeAttachment, m.Nid, m.PropertySet, true);

                return m;
            }
            else
                throw new XstException("Unexpected data type for attached message");
        }

        #region Properties
        public IEnumerable<XstProperty> GetProperties()
        {
            if (_Properties == null)
            {
                if (SubNodeTreeParentAttachment != null)
                    _Properties = Ltp.ReadAllProperties(SubNodeTreeParentAttachment, Nid, ContentExclusions, true);
                else
                    _Properties = Ltp.ReadAllProperties(Nid, ContentExclusions);
            }
            return _Properties;
        }

        private void ClearProperties()
        {
            _Properties = null;
        }
        #endregion Properties

        #region Attachments
        public IEnumerable<XstAttachment> GetAttachments()
            => _Attachments ?? (_Attachments = GetAttachmentsInternal());

        private IEnumerable<XstAttachment> GetAttachmentsInternal()
        {
            if (HasAttachments)
            {
                // Read any attachments
                var attachmentsNid = new NID(EnidSpecial.NID_ATTACHMENT_TABLE);

                if (!Ltp.IsTablePresent(SubNodeTreeProperties, attachmentsNid))
                    throw new XstException("Could not find expected Attachment table");

                // Read the attachment table, which is held in the subnode of the message
                var atts = Ltp.ReadTable<XstAttachment>(SubNodeTreeProperties, attachmentsNid, PropertyGetters.AttachmentListProperties, (a, id) => a.Nid = new NID(id)).ToList();
                foreach (var a in atts)
                {
                    a.Message = this; // For lazy reading of the complete properties: a.Message.Folder.XstFile

                    // If the long name wasn't in the attachment table, go look for it in the attachment properties
                    if (a.LongFileName == null)
                        Ltp.ReadProperties<XstAttachment>(SubNodeTreeProperties, a.Nid, PropertyGetters.AttachmentNameProperties, a);

                    // Read properties relating to HTML images presented as attachments
                    Ltp.ReadProperties<XstAttachment>(SubNodeTreeProperties, a.Nid, PropertyGetters.AttachedHtmlImagesProperties, a);

                    // If this is an embedded email, tell the attachment where to look for its properties
                    // This is needed because the email node is not in the main node tree
                    if (IsAttached)
                        a.SubNodeTreeProperties = SubNodeTreeProperties;

                    yield return a;
                }
            }
            yield break;
        }
        private void ClearAttachments()
        {
            _Attachments = null;
        }
        #endregion Attachments

        #region Recipients
        public IEnumerable<XstRecipient> GetRecipients()
        {
            if (_Recipients == null)
            {
                // Read the recipient table for the message
                var recipientsNid = new NID(EnidSpecial.NID_RECIPIENT_TABLE);
                if (Ltp.IsTablePresent(SubNodeTreeProperties, recipientsNid))
                {
                    _Recipients = Ltp.ReadTable<XstRecipient>(SubNodeTreeProperties, recipientsNid, PropertyGetters.MessageRecipientProperties, null, (r, p) => r.Properties.Add(p))
                                      // Sort the properties
                                      .Select(r => { r.Properties.OrderBy(p => p.Tag).ToList(); return r; });
                }
            }
            return _Recipients;
        }

        private void ClearRecipients()
        {
            _Recipients = null;
        }
        #endregion Recipients

        #region Body
        private string GetBodyText()
        {
            switch (BodyFormat)
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

        private void LoadBody()
        {
            SubNodeTreeProperties = _BodyLoader?.Invoke();
            _IsBodyLoaded = SubNodeTreeProperties != null;
        }

        private void ClearBody()
        {
            SubNodeTreeProperties = null;
            _NativeBody = null;
            _BodyHtml = null;
            _Body = null;

            _IsBodyLoaded = false;
        }
        #endregion Body

        public void ClearContents()
        {
            _Flags = null;
            ClearBody();
            ClearAttachments();
            ClearProperties();
            ClearRecipients();
        }

        // Take encrypted or signed bytes and parse into message object
        public void ReadSignedOrEncryptedMessage(byte[] messageBytes)
        {
            string messageFromBytes = System.Text.Encoding.ASCII.GetString(messageBytes);

            // the message is not encrypted just signed
            if (messageFromBytes.Contains("application/x-pkcs7-signature"))
            {
                ParseMimeMessage(messageFromBytes);
            }
            else
            {
                string decryptedMessage = DecryptMessage(messageBytes);
                string cleartextMessage;

                //Message is only signed
                if (decryptedMessage.Contains("filename=smime.p7m"))
                {
                    cleartextMessage = DecodeSignedMessage(decryptedMessage);
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
                    BodyHtml = DecodeQuotedPrintable(partHeaders["mimeBody"]);
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

        //decrpts mime message bytes with a valid cert in the user cert store
        // returns the decrypted message as a string
        private string DecryptMessage(byte[] encryptedMessageBytes)
        {
            //get cert store and collection of valid certs
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

            //decrypt bytes with EnvelopedCms
#if !NETCOREAPP
            EnvelopedCms ec = new EnvelopedCms();
            ec.Decode(encryptedMessageBytes);
            ec.Decrypt(fcollection);
            byte[] decryptedData = ec.ContentInfo.Content;

            return System.Text.Encoding.ASCII.GetString(decryptedData);
#else
            throw new XstException("CMS decoding not supported on this platform");
#endif
        }

        //Signed messages are base64 endcoded and broken up with \r\n 
        //This extracts the base64 content from signed message that has been wrapped in an encrypted message and decodes it
        // returns the decoded message as a string
        private string DecodeSignedMessage(string s)
        {
            //parse out base64 encoded content in "signed-data"
            string base64Message = s.Split(new string[] { "filename=smime.p7m" }, StringSplitOptions.None)[1];
            string data = base64Message.Replace("\r\n", "");

            // parse out signing data from content
#if !NETCOREAPP
            SignedCms sc = new SignedCms();
            sc.Decode(Convert.FromBase64String(data));

            return System.Text.Encoding.ASCII.GetString(sc.ContentInfo.Content);
#else
            throw new XstException("PKCS decoding not supported on this platform");
#endif
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
    }
}
