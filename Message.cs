// Copyright (c) 2016,2019, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace XstReader
{
    // Part of the view of the xst (.ost and .pst) file rendered by XAML
    // The data layer is effectively provided by the xst file itself

    class Message : INotifyPropertyChanged
    {
        private bool isSelected = false;
        private string exportFileName = null;

        public Folder Folder { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string FromTo { get { return Folder.Name.StartsWith("Sent") ? To : From; } }
        public string Subject { get; set; }
        public MessageFlags Flags { get; set; }
        public DateTime? Received { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Modified { get; set; }  // When any attachment was last modified
        public DateTime? Date { get { return Received ?? Submitted; } }
        public string DisplayDate { get { return Date != null ? ((DateTime)Date).ToString("g") : "<unknown>"; } }
        public NID Nid { get; set; }
        public BodyType NativeBody { get; set; }
        public string Body { get; set; }
        public string BodyHtml { get; set; }
        public byte[] Html { get; set; }
        public byte[] RtfCompressed { get; set; }
        public List<Attachment> Attachments { get; private set; } = new List<Attachment>();
        public List<Recipient> Recipients { get; private set; } = new List<Recipient>();
        public List<Property> Properties { get; private set; } = new List<Property>();
        public bool MayHaveInlineAttachment { get { return (Attachments.FirstOrDefault(a => a.HasContentId) != null); } }

        // The following properties are used in XAML bindings to control the UI
        public bool HasAttachment { get { return (Flags & MessageFlags.mfHasAttach) == MessageFlags.mfHasAttach; } }
        public bool HasFileAttachment { get { return (Attachments.FirstOrDefault(a => a.IsFile) != null); } }
        public bool HasVisibleFileAttachment { get { return (Attachments.FirstOrDefault(a => a.IsFile && !a.Hide) != null); } }
        public bool HasEmailAttachment { get { return (Attachments.FirstOrDefault(a => a.IsEmail) != null); } }
        public bool ShowText { get { return NativeBody == BodyType.PlainText || (NativeBody == BodyType.Undefined && Body != null && Body.Length > 0); } }
        public bool ShowHtml
        {
            get
            {
                return NativeBody == BodyType.HTML || (NativeBody == BodyType.Undefined &&
                       ((BodyHtml != null && BodyHtml.Length > 0) || (Html != null && Html.Length > 0)));
            }
        }
        public bool ShowRtf { get { return NativeBody == BodyType.RTF || (NativeBody == BodyType.Undefined && RtfCompressed != null && RtfCompressed.Length > 0); } }
        public bool HasToDisplayList { get { return ToDisplayList.Length > 0; } }
        public string ToDisplayList
        {
            get
            {
                return String.Join("; ", Recipients.Where(r => r.RecipientType == RecipientType.To)
                    .Select(r => r.DisplayName));
            }
        }
        public bool HasCcDisplayList { get { return CcDisplayList.Length > 0; } }
        public string CcDisplayList
        {
            get
            {
                return String.Join("; ", Recipients.Where(r => r.RecipientType == RecipientType.Cc)
                    .Select(r => r.DisplayName));
            }
        }
        public bool HasBccDisplayList { get { return BccDisplayList.Length > 0; } }
        public string BccDisplayList
        {
            get
            {
                return String.Join("; ", Recipients.Where(r => r.RecipientType == RecipientType.Bcc)
                    .Select(r => r.DisplayName));
            }
        }
        public string FileAttachmentDisplayList
        {
            get
            {
                return String.Join("; ", Attachments.Where(a => a.IsFile && !a.Hide)
                    .Select(a => a.FileName));
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public string ExportFileName
        {
            get
            {
                if (exportFileName == null)
                {
                    var fileName = String.Format("{0:yyyy-MM-dd HHmm} {1}", Date, Subject).Truncate(150);
                    string regex = String.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars())));
                    Regex removeInvalidChars = new Regex(regex, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
                    exportFileName = removeInvalidChars.Replace(fileName, " ");
                }

                return exportFileName;
            }
        }

        public string ExportFileExtension
        {
            get
            {
                if (ShowHtml)
                    return "html";
                else if (ShowRtf)
                    return "rtf";
                else
                    return "txt";
            }
        }

        public void ClearContents()
        {
            // Clear out any previous content   
            Body = null;
            BodyHtml = null;
            Html = null;
            Attachments.Clear();
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
            if (ShowHtml)
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
            else if (ShowRtf)
            {
                var doc = GetBodyAsFlowDocument();
                EmbedRtfPrintHeader(doc);
                TextRange content = new TextRange(doc.ContentStart, doc.ContentEnd);
                using (var stream = new FileStream(fullFileName, FileMode.Create))
                {
                    content.Save(stream, DataFormats.Rtf);
                }
                if (Date != null)
                    File.SetCreationTime(fullFileName, (DateTime)Date);
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

        private void AddRtfTableRow(Table table, string c0, string c1)
        {
            var currentRow = new TableRow();
            table.RowGroups[0].Rows.Add(currentRow);

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(c0))
            { FontFamily = new FontFamily("Arial"), FontSize = 12, FontWeight = FontWeights.Bold }));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(c1))
            { FontFamily = new FontFamily("Arial"), FontSize = 12 }));
        }

        public string EmbedAttachments(string body, XstFile xst)
        {
            if (body == null)
                return null;

            var dict = new Dictionary<string, Attachment>();
            foreach (var a in Attachments.Where(x => x.HasContentId))
                dict.Add(a.ContentId, a);

            return Regex.Replace(body, @"(="")cid:(.*?)("")", match =>
            {
                Attachment a;

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

        public void SortAndSaveAttachments(List<Attachment> atts = null)
        {
            // If no attachments are supplied, sort the list we already have
            if (atts == null)
                atts = new List<Attachment>(Attachments);

            atts.Sort((a, b) =>
            {
                if (a == null)
                    return -1;
                else if (b == null)
                    return 1;
                else if (a.Hide != b.Hide)
                    return a.Hide ? 1 : -1;
                else
                    return 0;
            });

            Attachments.Clear();
            foreach (var a in atts)
                Attachments.Add(a);
            OnPropertyChanged(nameof(Attachments));
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

}
