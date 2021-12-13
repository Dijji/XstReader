// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using XstReader.Properties;
using XstReader.Common.BTrees;
#if !NETCOREAPP
using System.Windows.Documents;
using System.Windows.Media;
using XstReader.Common;
#endif

namespace XstReader
{
    // Holds information about a single message, extracted from the xst tables

    public class XstMessage
    {
        public XstFolder Folder { get; private set; }
        public XstAttachment ParentAttachment { get; private set; }
        public XstFile XstFile => Folder.XstFile;
        internal LTP Ltp => XstFile.Ltp;
        internal NDB Ndb => XstFile.Ndb;

        private List<XstRecipient> _Recipients = null;
        public List<XstRecipient> Recipients => GetRecipients();

        public string From { get; set; }

        public bool HasToDisplayList => ToDisplayList.Length > 0;
        public string ToDisplayList => String.Join("; ", Recipients.Where(r => r.RecipientType == RecipientTypes.To).Select(r => r.DisplayName));
        public string To { get; set; }

        public bool HasCcDisplayList => CcDisplayList.Length > 0;
        public string CcDisplayList => String.Join("; ", Recipients.Where(r => r.RecipientType == RecipientTypes.Cc).Select(r => r.DisplayName));
        public string Cc { get; set; }

        public bool HasBccDisplayList => BccDisplayList.Length > 0;
        public string BccDisplayList => String.Join("; ", Recipients.Where(r => r.RecipientType == RecipientTypes.Bcc).Select(r => r.DisplayName));

        public string Subject { get; set; }

        internal MessageFlags Flags { get; set; }

        public DateTime? Received { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Modified { get; set; }  // When any attachment was last modified
        public DateTime? Date => Received ?? Submitted;
        public string DisplayDate => Date != null ? ((DateTime)Date).ToString("g") : "<unknown>";

        internal NID Nid { get; set; }

        private bool _IsContentLoaded = false;
        private Func<BTree<Node>> _ContentLoader = null;

        internal BodyType NativeBody { get; set; }
        private string _Body = null;
        public string Body
        {
            get
            {
                if (!_IsContentLoaded) LoadContents();
                return _Body;
            }
            internal set => _Body = value;
        }
        public bool IsBodyText => NativeBody == BodyType.PlainText ||
                                  (NativeBody == BodyType.Undefined && Body != null && Body.Length > 0);

        private string _BodyHtml = null;
        public string BodyHtml
        {
            get
            {
                if (!_IsContentLoaded) LoadContents();
                return _BodyHtml;
            }
            internal set => _BodyHtml = value;
        }
        private byte[] _Html = null;
        public byte[] Html
        {
            get
            {
                if (!_IsContentLoaded) LoadContents();
                return _Html;
            }
            internal set => _Html = value;
        }

        public bool IsBodyHtml => NativeBody == BodyType.HTML ||
                                  (NativeBody == BodyType.Undefined &&
                                      ((BodyHtml != null && BodyHtml.Length > 0) || (Html != null && Html.Length > 0)));

        private byte[] _RtfCompressed;
        public byte[] RtfCompressed
        {
            get
            {
                if (!_IsContentLoaded) LoadContents();
                return _RtfCompressed;
            }
            internal set => _RtfCompressed = value;
        }

        public bool IsBodyRtf => NativeBody == BodyType.RTF || (NativeBody == BodyType.Undefined && RtfCompressed != null && RtfCompressed.Length > 0);

        private List<XstAttachment> _Attachments = null;
        public List<XstAttachment> Attachments => GetAttachments();
        public bool HasAttachment => (Flags & MessageFlags.mfHasAttach) == MessageFlags.mfHasAttach;
        public bool MayHaveInlineAttachment => Attachments.FirstOrDefault(a => a.HasContentId) != null;
        public bool HasFileAttachment => Attachments.FirstOrDefault(a => a.IsFile) != null;
        public bool HasVisibleFileAttachment => Attachments.FirstOrDefault(a => a.IsFile && !a.Hide) != null;

        private List<XstProperty> _Properties = null;
        public List<XstProperty> Properties => GetProperties();
        public bool IsEncryptedOrSigned => GetBodyAsHtmlString() == null && Attachments.Count() == 1 && Attachments[0].FileName == "smime.p7m";

        public string FileAttachmentDisplayList => String.Join("; ", Attachments.Where(a => a.IsFile && !a.Hide).Select(a => a.FileName));

        private string _ExportFileName = null;
        public string ExportFileName => _ExportFileName ?? (_ExportFileName = String.Format("{0:yyyy-MM-dd HHmm} {1}", Date, Subject).Truncate(150).ReplaceInvalidFileNameChars(" "));

        public string ExportFileExtension => IsBodyHtml ? "html" : IsBodyRtf ? "rtf" : "txt";

        internal BTree<Node> SubNodeTreeProperties = null;
        internal BTree<Node> SubNodeTreeParentAttachment = null;
        public bool IsAttached => SubNodeTreeParentAttachment != null;

        #region PropertyGetters
        // We use sets of PropertyGetters to define the equivalent of queries when reading property sets and tables

        internal static readonly HashSet<EpropertyTag> contentExclusions = new HashSet<EpropertyTag>
        {
            EpropertyTag.PidTagNativeBody,
            EpropertyTag.PidTagBody,
            EpropertyTag.PidTagHtml,
            EpropertyTag.PidTagRtfCompressed,
        };

        #endregion PropertyGetters

        /// <summary>
        /// Initialization for Messages in a Folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        internal XstMessage Initialize(XstFolder folder)
        {
            Folder = folder;

            ClearContents();

            // Read the contents properties
            _ContentLoader = () => Ltp.ReadProperties<XstMessage>(Nid, PropertiesGetter.pgMessageContent, this);

            return this;
        }

        private void LoadContents()
        {
            SubNodeTreeProperties = _ContentLoader?.Invoke();
            _IsContentLoaded = SubNodeTreeProperties != null;
        }

        internal static XstMessage GetAttachedMessage(XstAttachment attachment)
        {
            BTree<Node> subNodeTreeMessage = attachment.SubNodeTreeProperties;

            if (subNodeTreeMessage == null)
                // No subNodeTree given: assume we can look it up in the main tree
                attachment.Ndb.LookupNodeAndReadItsSubNodeBtree(attachment.Message.Nid, out subNodeTreeMessage);

            var subNodeTreeAttachment = attachment.Ltp.ReadProperties<XstAttachment>(subNodeTreeMessage, attachment.Nid, PropertiesGetter.pgAttachmentContent, attachment);
            if (attachment.Content.GetType() == typeof(PtypObjectValue))
            {
                XstMessage m = new XstMessage
                {
                    Nid = new NID(((PtypObjectValue)attachment.Content).Nid),
                    Folder = attachment.Folder,
                    ParentAttachment = attachment,
                    SubNodeTreeParentAttachment = subNodeTreeAttachment,
                };

                // Read the basic and contents properties
                m.ClearContents();
                m._ContentLoader = () => attachment.Ltp.ReadProperties<XstMessage>(subNodeTreeAttachment, m.Nid, PropertiesGetter.pgMessageAttachment, m, true);

                return m;
            }
            else
                throw new XstException("Unexpected data type for attached message");
        }

        #region Properties
        public List<XstProperty> GetProperties()
        {
            if (_Properties == null)
            {
                if (SubNodeTreeParentAttachment != null)
                    _Properties = Ltp.ReadAllProperties(SubNodeTreeParentAttachment, Nid, contentExclusions, true).ToList();
                else
                    _Properties = Ltp.ReadAllProperties(Nid, contentExclusions).ToList();
            }
            return _Properties;
        }

        public void UnloadProperties()
        {
            _Properties = null;
        }
        #endregion Properties

        #region Attachments
        public List<XstAttachment> GetAttachments()
        {
            if (_Attachments == null)
            {
                _Attachments = new List<XstAttachment>();

                // Read any attachments
                var attachmentsNid = new NID(EnidSpecial.NID_ATTACHMENT_TABLE);
                if (HasAttachment)
                {
                    if (!Ltp.IsTablePresent(SubNodeTreeProperties, attachmentsNid))
                        throw new XstException("Could not find expected Attachment table");

                    // Read the attachment table, which is held in the subnode of the message
                    var atts = Ltp.ReadTable<XstAttachment>(SubNodeTreeProperties, attachmentsNid, PropertiesGetter.pgAttachmentList, (a, id) => a.Nid = new NID(id)).ToList();
                    foreach (var a in atts)
                    {
                        a.Message = this; // For lazy reading of the complete properties: a.Message.Folder.XstFile

                        // If the long name wasn't in the attachment table, go look for it in the attachment properties
                        if (a.LongFileName == null)
                            Ltp.ReadProperties<XstAttachment>(SubNodeTreeProperties, a.Nid, PropertiesGetter.pgAttachmentName, a);

                        // Read properties relating to HTML images presented as attachments
                        Ltp.ReadProperties<XstAttachment>(SubNodeTreeProperties, a.Nid, PropertiesGetter.pgAttachedHtmlImages, a);

                        // If this is an embedded email, tell the attachment where to look for its properties
                        // This is needed because the email node is not in the main node tree
                        if (IsAttached)
                            a.SubNodeTreeProperties = SubNodeTreeProperties;

                        _Attachments.Add(a);
                    }
                }
            }
            return _Attachments;
        }

        public void UnloadAttachments()
        {
            _Attachments = null;
        }
        #endregion Attachments

        #region Recipients
        public List<XstRecipient> GetRecipients()
        {
            if (_Recipients == null)
            {
                _Recipients = new List<XstRecipient>();

                // Read the recipient table for the message
                var recipientsNid = new NID(EnidSpecial.NID_RECIPIENT_TABLE);
                if (Ltp.IsTablePresent(SubNodeTreeProperties, recipientsNid))
                {
                    var rs = Ltp.ReadTable<XstRecipient>(SubNodeTreeProperties, recipientsNid, PropertiesGetter.pgMessageRecipient, null, (r, p) => r.Properties.Add(p));
                    foreach (var r in rs)
                    {
                        // Sort the properties
                        List<XstProperty> lp = new List<XstProperty>(r.Properties);
                        lp.Sort((a, b) => a.Tag.CompareTo(b.Tag));
                        r.Properties.Clear();
                        foreach (var p in lp)
                            r.Properties.Add(p);

                        _Recipients.Add(r);
                    }
                }
            }
            return _Recipients;
        }

        public void UnloadRecipients()
        {
            _Recipients = null;
        }
        #endregion Recipients

        public void ClearContents()
        {
            UnloadBody();
            UnloadAttachments();
            UnloadProperties();
            UnloadRecipients();
        }
        public void UnloadBody()
        {
            _Body = null;
            _BodyHtml = null;
            _Html = null;
            _RtfCompressed = null;
            _IsContentLoaded = false;
        }

        public string GetBodyAsHtmlString()
        {
            if (BodyHtml != null)
                return BodyHtml; // This will be plain ASCII
            else if (Html != null)
            {
                var e = GetEncoding();
                if (e != null)
                {
                    return EscapeUnicodeCharacters(new String(e.GetChars(Html)));
                }
            }
            else if (Body != null) // Not really expecting this as a source of HTML
                return EscapeUnicodeCharacters(Body);

            return null;
        }

        public void ExportToFile(string fullFileName, XstFile xstFile)
        {
            if (IsBodyHtml)
            {
                string body = GetBodyAsHtmlString();
                if (MayHaveInlineAttachment)
                    body = EmbedAttachments(body, xstFile);  // Returns null if this is not appropriate

                if (body != null)
                {
                    body = EmbedHtmlPrintHeader(body);
                    using (var stream = new FileStream(fullFileName, FileMode.Create))
                    {
                        var bytes = Encoding.UTF8.GetBytes(body);
                        stream.Write(bytes, 0, bytes.Count());
                    }
                    if (Date != null)
                        File.SetCreationTime(fullFileName, (DateTime)Date);
                }
            }
            else if (IsBodyRtf)
            {
#if !NETCOREAPP

                var doc = GetBodyAsFlowDocument();
                EmbedRtfPrintHeader(doc);
                TextRange content = new TextRange(doc.ContentStart, doc.ContentEnd);
                using (var stream = new FileStream(fullFileName, FileMode.Create))
                {
                    content.Save(stream, DataFormats.Rtf);
                }
                if (Date != null)
                    File.SetCreationTime(fullFileName, (DateTime)Date);
#else
                throw new XstException("Emails with body in RTF format not supported on this platform");
#endif
            }
            else
            {
                var body = EmbedTextPrintHeader(Body);
                using (var stream = new FileStream(fullFileName, FileMode.Create))
                {
                    var bytes = Encoding.UTF8.GetBytes(body);
                    stream.Write(bytes, 0, bytes.Count());
                }
                if (Date != null)
                    File.SetCreationTime(fullFileName, (DateTime)Date);
            }
        }

#if !NETCOREAPP
        public FlowDocument GetBodyAsFlowDocument()
        {
            FlowDocument doc = new FlowDocument();

            var decomp = new RtfDecompressor();

            using (System.IO.MemoryStream ms = decomp.Decompress(RtfCompressed, true))
            {
                ms.Position = 0;
                TextRange selection = new TextRange(doc.ContentStart, doc.ContentEnd);
                selection.Load(ms, DataFormats.Rtf);
            }
            // For debug, a way to look at the document
            //var infoString = System.Windows.Markup.XamlWriter.Save(doc);
            return doc;
        }
#endif
        public string EmbedTextPrintHeader(string body, bool forDisplay = false, bool showEmailType = false)
        {
            string row = forDisplay ? "{0,-15}\t{1}\r\n" : "{0,-15}{1}\r\n";
            StringBuilder header = new StringBuilder();
            header.AppendFormat(row, "Sent:", String.Format("{0:dd MMMM yyyy HHmm}", Date));
            header.AppendFormat(row, showEmailType ? "Text From:" : "From:", From);
            header.AppendFormat(row, "To:", ToDisplayList);
            if (HasCcDisplayList)
                header.AppendFormat(row, "Cc:", CcDisplayList);
            if (HasBccDisplayList)
                header.AppendFormat(row, "Bcc:", BccDisplayList);
            header.AppendFormat(row, "Subject:", Subject);
            if (HasFileAttachment)
                header.AppendFormat(row, "Attachments:", FileAttachmentDisplayList);
            header.Append("\r\n\r\n");

            return header.ToString() + body ?? "";
        }

        public string EmbedHtmlPrintHeader(string body, bool showEmailType = false)
        {
            if (body == null)
                return null;

            int insertAt;

            // look for an insertion point after a variety of tags in descending priority order
            if (!LookForInsertionPoint(body, "body", out insertAt))
                if (!LookForInsertionPoint(body, "meta", out insertAt))
                    if (!LookForInsertionPoint(body, "html", out insertAt))
                        //throw new Exception("Cannot locate insertion point in HTML email contents");
                        insertAt = 0; // Just insert at the beginning

            const string row = "<tr style=\"font-family:Arial,Helvetica,sans-serif;font-size:12px;\">" +
                "<td style=\"width:175px;vertical-align:top\"><b>{0}<b></td><td>{1}</td></tr>";
            StringBuilder header = new StringBuilder();
            //omit MyName and the line under it for now, as we have no reliable source for it
            //header.AppendFormat("<h3>{0}</h3><hr/><table><tbody>", MyName);
            header.Append("<table><tbody style=\"font-family:serif;font-size:12px;\">");
            header.AppendFormat(row, showEmailType ? "HTML From:" : "From:", From);
            header.AppendFormat(row, "Sent:", String.Format("{0:dd MMMM yyyy HH:mm}", Date));
            header.AppendFormat(row, "To:", ToDisplayList);
            if (HasCcDisplayList)
                header.AppendFormat(row, "Cc:", CcDisplayList);
            if (HasBccDisplayList)
                header.AppendFormat(row, "Bcc:", BccDisplayList);
            header.AppendFormat(row, "Subject:", Subject);
            if (HasFileAttachment)
                header.AppendFormat(row, "Attachments:", FileAttachmentDisplayList);
            header.Append("</tbody></table><p/><p/>");

            return body.Insert(insertAt, header.ToString());
        }

        private bool LookForInsertionPoint(string body, string tag, out int insertAt)
        {
            string rex = String.Format(@"<\s*(?i){0}[^<]*>", tag);
            Match m = Regex.Match(body, rex);
            if (m.Success)
            {
                insertAt = m.Index + m.Length;
                return true;
            }
            else
            {
                insertAt = -1;
                return false;
            }
        }

#if !NETCOREAPP

        public void EmbedRtfPrintHeader(FlowDocument doc, bool showEmailType = false)
        {
            if (doc == null)
                return;

            //omit MyName and the line under it for now, as we have no reliable source for it
            //Paragraph p = new Paragraph(new Run(MyName));
            //p.FontSize = 14;
            //p.FontWeight = FontWeights.Bold;
            //p.TextDecorations = TextDecorations.Underline;

            // Create the Table...
            var table1 = new Table();

            table1.Columns.Add(new TableColumn { Width = new GridLength(150) });
            table1.Columns.Add(new TableColumn { Width = new GridLength(500) });
            table1.RowGroups.Add(new TableRowGroup());

            AddRtfTableRow(table1, showEmailType ? "RTF From:" : "From:", From);

            AddRtfTableRow(table1, "Sent:", String.Format("{0:dd MMMM yyyy HH:mm}", Date));
            AddRtfTableRow(table1, "To:", ToDisplayList);
            if (HasCcDisplayList)
                AddRtfTableRow(table1, "Cc:", CcDisplayList);
            if (HasBccDisplayList)
                AddRtfTableRow(table1, "Bcc:", BccDisplayList);
            AddRtfTableRow(table1, "Subject:", Subject);
            if (HasFileAttachment)
                AddRtfTableRow(table1, "Attachments:", FileAttachmentDisplayList);

            // Cope with the empty document case
            if (doc.Blocks.Count == 0)
                doc.Blocks.Add(new Paragraph(new Run("")));
            else
                doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, new Paragraph(new Run("")));
            doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, new Paragraph(new Run("")));
            doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, table1);

            //omit MyName and the line under it for now, as we have no reliable source for it
            //doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, p);
        }
#endif
#if !NETCOREAPP

        private void AddRtfTableRow(Table table, string c0, string c1)
        {
            var currentRow = new TableRow();
            table.RowGroups[0].Rows.Add(currentRow);

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(c0))
            { FontFamily = new FontFamily("Arial"), FontSize = 12, FontWeight = FontWeights.Bold }));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(c1))
            { FontFamily = new FontFamily("Arial"), FontSize = 12 }));
        }
#endif
        public string EmbedAttachments(string body, XstFile xst)
        {
            if (body == null)
                return null;

            var dict = Attachments.Where(a => a.HasContentId)
                                  .GroupBy(a => a.ContentId)
                                  .Select(g => g.First())
                                  .ToDictionary(a => a.ContentId);

            return Regex.Replace(body, @"(="")cid:(.*?)("")", match =>
            {
                XstAttachment a;

                if (dict.TryGetValue(match.Groups[2].Value, out a))
                {
                    // There are limits to what we can push into an inline data image, 
                    // but we don't know exactly what
                    // Todo handle limit when known
                    a.WasRenderedInline = true;
                    var s = new MemoryStream();
                    xst.SaveAttachment(s, a);
                    s.Seek(0, SeekOrigin.Begin);
                    var cooked = match.Groups[1] + @"data:image/jpg;base64," + EscapeString(Convert.ToBase64String(s.ToArray())) + match.Groups[3];
                    return cooked;
                }

                return match.Value;
            }, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        private static string EscapeUnicodeCharacters(string source)
        {
            int length = source.Length;
            var escaped = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                char ch = source[i];

                if (ch >= '\x00a0')
                {
                    escaped.AppendFormat("&#x{0};", ((int)ch).ToString("X4"));
                }
                else
                {
                    escaped.Append(ch);
                }
            }

            return escaped.ToString();
        }

        private string EscapeString(string s)
        {
            var sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length;)
            {
                int len = Math.Min(s.Length - i, 32766);
                sb.Append(Uri.EscapeDataString(s.Substring(i, len)));
                i += len;
            }
            return sb.ToString();
        }

        private Encoding GetEncoding()
        {
            var p = Properties.FirstOrDefault(x => x.Guid == "00020386-0000-0000-c000-000000000046" && x.Name == "content-type");
            if (p != null)
            {

                Match m = Regex.Match((string)p.Value, @".*charset=""(.*?)""");
                if (m.Success)
                    return Encoding.GetEncoding(m.Groups[1].Value);
            }

            p = Properties.FirstOrDefault(x => x.Tag == EpropertyTag.PidTagInternetCodepage);
            if (p != null)
            {
                return Encoding.GetEncoding((int)p.Value);
            }

            return null;
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
            Attachments.RemoveAt(0);
            // if no attachments left unset the has attachments flag
            if (Attachments.Count == 0)
            {
                Flags ^= MessageFlags.mfHasAttach;
            }
        }

        //parse mime message into a given message object adds alll attachments and inserts inline content to message body
        public void ParseMimeMessage(String mimeText)
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
                    Attachments.Add(new XstAttachment(filename, content));
                }
                //inline images
                else if (partHeaders.Keys.Contains("content-id"))
                {
                    string fileName = Regex.Match(partHeaders["content-type"], @".*name=""(.*)""", RegexOptions.IgnoreCase).Groups[1].Value;
                    string contentId = Regex.Match(partHeaders["content-id"], @"<(.*)>", RegexOptions.IgnoreCase).Groups[1].Value;
                    byte[] content = Convert.FromBase64String(partHeaders["mimeBody"]);
                    Attachments.Add(new XstAttachment(fileName, contentId, content));
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

            string line = string.Empty;
            string lastHeader = string.Empty;
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
    }

}
