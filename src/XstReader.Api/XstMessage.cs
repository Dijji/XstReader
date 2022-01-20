﻿// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XstReader.Common;
using XstReader.Common.BTrees;
using XstReader.ElementProperties;


namespace XstReader
{
    // Holds information about a single message, extracted from the xst tables

    public partial class XstMessage : XstElement
    {
        private static RtfDecompressor RtfDecompressor = new RtfDecompressor();

        public XstFolder ParentFolder { get; private set; }
        public XstAttachment ParentAttachment { get; private set; }
        internal protected override XstFile XstFile => ParentFolder.XstFile;


        private IEnumerable<XstRecipient> _Recipients = null;
        public IEnumerable<XstRecipient> Recipients => GetRecipients();

        public bool HasToDisplayList => Recipients.To().Any();
        public bool HasCcDisplayList => Recipients.Cc().Any();
        public bool HasBccDisplayList => Recipients.Bcc().Any();

        public string Subject => XstPropertySet[PropertyCanonicalName.PidTagSubject]?.Value;
        public string Cc => XstPropertySet[PropertyCanonicalName.PidTagDisplayCc]?.Value;
        public string To => XstPropertySet[PropertyCanonicalName.PidTagDisplayTo]?.Value;
        public string From => XstPropertySet[PropertyCanonicalName.PidTagSenderName]?.Value;
        public string FromAddress => XstPropertySet[PropertyCanonicalName.PidTagSenderSmtpAddress]?.Value ??
                                     XstPropertySet[PropertyCanonicalName.PidTagSmtpAddress]?.Value ??
                                     XstPropertySet[PropertyCanonicalName.PidTagSenderEmailAddress]?.Value ??
                                     XstPropertySet[PropertyCanonicalName.PidTagEmailAddress]?.Value;
        public string FromRepresenting => XstPropertySet[PropertyCanonicalName.PidTagSentRepresentingName]?.Value;
        public string FromRepresentingAddress => XstPropertySet[PropertyCanonicalName.PidTagSentRepresentingSmtpAddress]?.Value ??
                                                 XstPropertySet[PropertyCanonicalName.PidTagSentRepresentingEmailAddress]?.Value;

        public bool IsRepresentingOther => From != FromRepresenting || FromAddress != FromRepresentingAddress;

        private MessageFlags? _Flags = null;
        public MessageFlags? Flags
        {
            get => _Flags ?? (MessageFlags?)XstPropertySet[PropertyCanonicalName.PidTagMessageFlags]?.Value;
            private set => _Flags = value;
        }
        public DateTime? Submitted => XstPropertySet[PropertyCanonicalName.PidTagClientSubmitTime]?.Value;
        public DateTime? Received => XstPropertySet[PropertyCanonicalName.PidTagMessageDeliveryTime]?.Value;

        public DateTime? Date => Received ?? Submitted;

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
        public Encoding Encoding => _Encoding ?? (_Encoding = GetEncoding());
        private BodyType? _NativeBody = null;
        internal BodyType NativeBody
        {
            get => _NativeBody ?? XstPropertySet[PropertyCanonicalName.PidTagNativeBody]?.Value as BodyType? ?? BodyType.Undefined;
            private set => _NativeBody = value;
        }
        private XstMessageBody _Body = null;
        public XstMessageBody Body => GetBody();

        public bool IsRead => (Flags & MessageFlags.mfRead) == MessageFlags.mfRead;

        private IEnumerable<XstAttachment> _Attachments = null;
        public IEnumerable<XstAttachment> Attachments => GetAttachments();
        public IEnumerable<XstAttachment> AttachmentsFiles => Attachments.Where(a => a.IsFile);
        public IEnumerable<XstAttachment> AttachmentsVisibleFiles => AttachmentsFiles.Where(a => !a.Hide);
        public bool HasAttachments => (Flags & MessageFlags.mfHasAttach) == MessageFlags.mfHasAttach;
        public bool MayHaveAttachmentsInline => Attachments.Any(a => a.HasContentId);
        public bool HasAttachmentsFiles => AttachmentsFiles.Any();
        public bool HasAttachmentsVisibleFiles => HasAttachments && AttachmentsVisibleFiles.Any();


        public bool IsEncryptedOrSigned => BodyHtml == null && Html == null && BodyPlainText == null &&
                                           Attachments.Count() == 1 &&
                                           Attachments.First().FileNameForSaving == "smime.p7m";

        private BTree<Node> _SubNodeTreeProperties = null;
        internal BTree<Node> SubNodeTreeProperties
        {
            get => _SubNodeTreeProperties ?? (_SubNodeTreeProperties = BodyLoader?.Invoke());
            set => _SubNodeTreeProperties = value;
        }
        internal BTree<Node> SubNodeTreeParentAttachment = null;
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
        public XstMessage()
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="parentFolder"></param>
        /// <param name="parentAttachment"></param>
        internal XstMessage(XstFolder parentFolder, XstAttachment parentAttachment)
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
            BodyLoader = () => Ltp.ReadProperties(Nid, XstPropertySet);
        }

        #region Properties

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
        private void ClearAttachments()
        {
            _Attachments = null;
        }
        #endregion Attachments

        #region Recipients
        public IEnumerable<XstRecipient> GetRecipients()
            => _Recipients ?? (_Recipients = GetRecipientsInternal());

        private IEnumerable<XstRecipient> GetRecipientsInternal()
        {
            // Read the recipient table for the message
            var recipientsNid = new NID(EnidSpecial.NID_RECIPIENT_TABLE);
            if (!Ltp.IsTablePresent(SubNodeTreeProperties, recipientsNid))
                return new XstRecipient[0];
            //    throw new XstException("Could not find expected Recipient table");

            return Ltp.ReadTable<XstRecipient>(SubNodeTreeProperties, recipientsNid,
                                               (r, id) => r.Nid = new NID(id),
                                               r => r.Initialize(this));
            // Sort the properties
            //.Select(r => { r.Properties.OrderBy(p => p.Tag).ToList(); return r; });
        }

        private void ClearRecipients()
        {
            _Recipients = null;
        }
        #endregion Recipients

        #region Body
        private string BodyPlainText => XstPropertySet[PropertyCanonicalName.PidTagBody]?.Value;
        private bool IsBodyPlainText => NativeBody == BodyType.PlainText ||
                                       (NativeBody == BodyType.Undefined && BodyPlainText?.Length > 0);

        private string BodyHtml => XstPropertySet[PropertyCanonicalName.PidTagHtml]?.Value as string;

        private byte[] Html => XstPropertySet[PropertyCanonicalName.PidTagHtml]?.Value as byte[];
        private bool IsBodyHtml => NativeBody == BodyType.HTML ||
                                  (NativeBody == BodyType.Undefined && (BodyHtml?.Length > 0 || Html?.Length > 0));

        private byte[] BodyRtfCompressed => XstPropertySet[PropertyCanonicalName.PidTagRtfCompressed]?.Value;
        private bool IsBodyRtf => NativeBody == BodyType.RTF ||
                                 (NativeBody == BodyType.Undefined && BodyRtfCompressed?.Length > 0);


        public XstMessageBody GetBody()
        {
            if(_Body==null)
            {
                var format = GetBodyFormat();
                _Body = new XstMessageBody(this, GetBodyText(format), format);
            }
            return _Body;
        }
        private XstMessageBodyFormat GetBodyFormat()
        {
            if (IsBodyHtml)
                return XstMessageBodyFormat.Html;
            if (IsBodyRtf)
                return XstMessageBodyFormat.Rtf;
            if (IsBodyPlainText)
                return XstMessageBodyFormat.PlainText;

            return XstMessageBodyFormat.Unknown;
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

        private void ClearBody()
        {
            SubNodeTreeProperties = null;
            _NativeBody = null;
            _Body = null;
        }
        #endregion Body

        public override void ClearContents()
        {
            base.ClearContents();

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
    }
}
