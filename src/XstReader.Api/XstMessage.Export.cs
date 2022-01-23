using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XstReader.ElementProperties;

namespace XstReader
{
    public partial class XstMessage
    {
        public XstExportOptions ExportOptions { get; set; } = new XstExportOptions();

        private XstRecipient FromRecipient => Recipients.Filter(RecipientType.Originator).FirstOrDefault() ??
                                              new XstRecipient(From, FromAddress, RecipientType.Originator);
        private XstRecipient FromRepresentingRecipient
            => (FromRepresenting ?? FromRepresentingAddress) != null
               ? new XstRecipient(FromRepresenting, FromRepresentingAddress, RecipientType.Representing)
               : null;

        public string FromFormatted => ExportOptions.Format(FromRecipient);
        public string FromRepresentingFormatted => ExportOptions.Format(FromRepresentingRecipient);
        public string ToFormatted => ExportOptions.Format(Recipients.To());
        public string CcFormatted => ExportOptions.Format(Recipients.Cc());
        public string BccFormatted => ExportOptions.Format(Recipients.Bcc());

        public string DateFormatted => ExportOptions.Format(Date);

        public string AttachmentsVisibleFilesFormatted => ExportOptions.Format(AttachmentsVisibleFiles);

        private string HtmlHeader
            => "<p class=\"MsoNormal\">" +
                    $"<b>From:</b> {FromFormatted.AppendNewLine().TextToHtml()}" +
                    (IsSentRepresentingOther ? $"<b>Representing:</b> {FromRepresentingFormatted.AppendNewLine().TextToHtml()}" : "") +
                    $"<b>Sent:</b> {DateFormatted.AppendNewLine().TextToHtml()}" +
                    $"<b>To:</b> {ToFormatted.AppendNewLine().TextToHtml()}" +
                    (HasCcDisplayList ? $"<b>Cc:</b> {CcFormatted.AppendNewLine().TextToHtml()}" : "") +
                    (HasBccDisplayList ? $"<b>Bcc:</b> {BccFormatted.AppendNewLine().TextToHtml()}" : "") +
                    $"<b>Subject:</b> {Subject.AppendNewLine().TextToHtml()}" +
                    (HasAttachmentsVisibleFiles ? $"<b>Attachments:</b> {AttachmentsVisibleFilesFormatted.AppendNewLine().TextToHtml()}" : "") +
                "</p><p/><p/>";

        private string _ExportFileName = null;
        public string ExportFileName => _ExportFileName ?? (_ExportFileName = String.Format("{0:yyyy-MM-dd HHmm} {1}", Date, Subject).Truncate(150).ReplaceInvalidFileNameChars(" "));

        public string ExportFileExtension => IsBodyHtml ? "html" : IsBodyRtf ? "rtf" : "txt";


        public void SaveToFile(string fullFileName, bool includeVisibleAttachments = true)
        {
            if (IsBodyHtml)
            {
                string body = GetBodyAsHtmlString();

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
                //TODO: Rtf Support
                //#if !NETCOREAPP
                //                var doc = GetBodyAsFlowDocument();
                //                EmbedRtfPrintHeader(doc);
                //                TextRange content = new TextRange(doc.ContentStart, doc.ContentEnd);
                //                using (var stream = new FileStream(fullFileName, FileMode.Create))
                //                {
                //                    content.Save(stream, DataFormats.Rtf);
                //                }
                //                if (Date != null)
                //                    File.SetCreationTime(fullFileName, (DateTime)Date);
                //#else
                //                throw new XstException("Emails with body in RTF format not supported on this platform");
                //#endif
            }
            else
            {
                var body = EmbedTextPrintHeader(BodyPlainText);
                using (var stream = new FileStream(fullFileName, FileMode.Create))
                {
                    var bytes = Encoding.UTF8.GetBytes(body);
                    stream.Write(bytes, 0, bytes.Count());
                }
                if (Date != null)
                    File.SetCreationTime(fullFileName, (DateTime)Date);
            }
            if (includeVisibleAttachments)
                SaveVisibleAttachmentsToAssociatedFolder(fullFileName);
        }

        private void SaveVisibleAttachmentsToAssociatedFolder(string fullFileName)
        {
            if (HasAttachmentsVisibleFiles)
            {
                var targetFolder = Path.Combine(Path.GetDirectoryName(fullFileName),
                    Path.GetFileNameWithoutExtension(fullFileName) + " Attachments");
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                    if (Date != null)
                        Directory.SetCreationTime(targetFolder, (DateTime)Date);
                }
                Attachments.Where(a => a.IsFile && !a.Hide).SaveToFolder(targetFolder, Date);
            }
        }

        public string GetBodyAsHtmlString(bool embedInlineAttachments = true)
        {
            string body = GetBodyAsHtmlStringBase();

            if (embedInlineAttachments && MayHaveAttachmentsInline)
                body = EmbedAttachments(body);  // Returns null if this is not appropriate

            return body;
        }
        private string GetBodyAsHtmlStringBase()
        {
            if (BodyHtml != null)
                return BodyHtml; // This will be plain ASCII
            else if (Html != null)
            {
                if (Encoding != null)
                {
                    return EscapeUnicodeCharacters(new String(Encoding.GetChars(Html)));
                }
            }
            else if (BodyPlainText != null) // Not really expecting this as a source of HTML
                return EscapeUnicodeCharacters(BodyPlainText);

            return null;
        }

        ////TODO: Rtf Support
        //#if !NETCOREAPP
        //        public FlowDocument GetBodyAsFlowDocument()
        //        {
        //            FlowDocument doc = new FlowDocument();

        //            var decomp = new RtfDecompressor();

        //            using (System.IO.MemoryStream ms = decomp.Decompress(BodyRtfCompressed, true))
        //            {
        //                ms.Position = 0;
        //                TextRange selection = new TextRange(doc.ContentStart, doc.ContentEnd);
        //                selection.Load(ms, DataFormats.Rtf);
        //            }
        //            // For debug, a way to look at the document
        //            //var infoString = System.Windows.Markup.XamlWriter.Save(doc);
        //            return doc;
        //        }
        //#endif
        public string EmbedTextPrintHeader(string body, bool forDisplay = false, bool showEmailType = false)
        {
            string row = forDisplay ? "{0,-15}\t{1}\r\n" : "{0,-15}{1}\r\n";
            StringBuilder header = new StringBuilder();
            header.AppendFormat(row, "Sent:", String.Format("{0:dd MMMM yyyy HHmm}", Date));
            header.AppendFormat(row, showEmailType ? "Text From:" : "From:", From);
            header.AppendFormat(row, "To:", ToFormatted);
            if (HasCcDisplayList)
                header.AppendFormat(row, "Cc:", CcFormatted);
            if (HasBccDisplayList)
                header.AppendFormat(row, "Bcc:", BccFormatted);
            header.AppendFormat(row, "Subject:", Subject);
            if (HasAttachmentsFiles)
                header.AppendFormat(row, "Attachments:", AttachmentsVisibleFilesFormatted);
            header.Append("\r\n\r\n");

            return header.ToString() + body ?? "";
        }

        private string GetHtmlHeaderOld(bool showEmailType = false)
        {
            //const string row = "<tr style=\"font-family:Arial,Helvetica,sans-serif;font-size:12px;\">" +
            //    "<td style=\"width:175px;vertical-align:top\"><b>{0}<b></td><td>{1}</td></tr>";
            const string row = "<tr><td style=\"width:175px;vertical-align:top\"><strong>{0}</strong></td><td>{1}</td></tr>";
            StringBuilder header = new StringBuilder();
            //omit MyName and the line under it for now, as we have no reliable source for it
            //header.AppendFormat("<h3>{0}</h3><hr/><table><tbody>", MyName);
            header.Append("<table><tbody>");
            header.AppendFormat(row, showEmailType ? "HTML From:" : "From:", From);
            header.AppendFormat(row, "Sent:", String.Format("{0:dd MMMM yyyy HH:mm}", Date));
            header.AppendFormat(row, "To:", ToFormatted);
            if (HasCcDisplayList)
                header.AppendFormat(row, "Cc:", CcFormatted);
            if (HasBccDisplayList)
                header.AppendFormat(row, "Bcc:", BccFormatted);
            header.AppendFormat(row, "Subject:", Subject);
            if (HasAttachmentsFiles)
                header.AppendFormat(row, "Attachments:", AttachmentsVisibleFilesFormatted);
            header.Append("</tbody></table><p/><p/>");

            return header.ToString();
        }

        private string GetHtmlHeader1(bool showEmailType = false)
        {
            const string row = "<b>{0}</b> {1}<br>";
            StringBuilder header = new StringBuilder();
            //omit MyName and the line under it for now, as we have no reliable source for it
            //header.AppendFormat("<h3>{0}</h3><hr/><table><tbody>", MyName);
            header.Append("<p class=\"MsoNormal\">");
            header.AppendFormat(row, showEmailType ? "HTML From:" : "From:", FromFormatted.TextToHtml());
            header.AppendFormat(row, "Sent:", DateFormatted.TextToHtml());
            header.AppendFormat(row, "To:", ToFormatted.TextToHtml());
            if (HasCcDisplayList)
                header.AppendFormat(row, "Cc:", CcFormatted.TextToHtml());
            if (HasBccDisplayList)
                header.AppendFormat(row, "Bcc:", BccFormatted.TextToHtml());
            header.AppendFormat(row, "Subject:", Subject.TextToHtml());
            if (HasAttachmentsVisibleFiles)
                header.AppendFormat(row, "Attachments:", AttachmentsVisibleFilesFormatted.TextToHtml());
            header.Append("</p><p/><p/>");

            return header.ToString();
        }






        public string EmbedHtmlPrintHeader(string body)
        {
            if (body == null)
                return null;

            // look for an insertion point after a variety of tags in descending priority order
            if (!LookForInsertionPoint(body, "body", out int insertAt) &&
                !LookForInsertionPoint(body, "meta", out insertAt) &&
                !LookForInsertionPoint(body, "html", out insertAt))
                //throw new Exception("Cannot locate insertion point in HTML email contents");
                insertAt = 0; // Just insert at the beginning

            return body.Insert(insertAt, HtmlHeader);
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

        //TODO: Rtf Support
        //#if !NETCOREAPP
        //        public void EmbedRtfPrintHeader(FlowDocument doc, bool showEmailType = false)
        //        {
        //            if (doc == null)
        //                return;

        //            //omit MyName and the line under it for now, as we have no reliable source for it
        //            //Paragraph p = new Paragraph(new Run(MyName));
        //            //p.FontSize = 14;
        //            //p.FontWeight = FontWeights.Bold;
        //            //p.TextDecorations = TextDecorations.Underline;

        //            // Create the Table...
        //            var table1 = new Table();

        //            table1.Columns.Add(new TableColumn { Width = new GridLength(150) });
        //            table1.Columns.Add(new TableColumn { Width = new GridLength(500) });
        //            table1.RowGroups.Add(new TableRowGroup());

        //            AddRtfTableRow(table1, showEmailType ? "RTF From:" : "From:", From);

        //            AddRtfTableRow(table1, "Sent:", String.Format("{0:dd MMMM yyyy HH:mm}", Date));
        //            AddRtfTableRow(table1, "To:", ToFormatted);
        //            if (HasCcDisplayList)
        //                AddRtfTableRow(table1, "Cc:", CcFormatted);
        //            if (HasBccDisplayList)
        //                AddRtfTableRow(table1, "Bcc:", BccFormatted);
        //            AddRtfTableRow(table1, "Subject:", Subject);
        //            if (HasAttachmentsFiles)
        //                AddRtfTableRow(table1, "Attachments:", AttachmentsVisibleFilesFormatted);

        //            // Cope with the empty document case
        //            if (doc.Blocks.Count == 0)
        //                doc.Blocks.Add(new Paragraph(new Run("")));
        //            else
        //                doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, new Paragraph(new Run("")));
        //            doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, new Paragraph(new Run("")));
        //            doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, table1);

        //            //omit MyName and the line under it for now, as we have no reliable source for it
        //            //doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, p);
        //        }
        //#endif
        //#if !NETCOREAPP
        //        private void AddRtfTableRow(Table table, string c0, string c1)
        //        {
        //            var currentRow = new TableRow();
        //            table.RowGroups[0].Rows.Add(currentRow);

        //            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(c0))
        //            { FontFamily = new FontFamily("Arial"), FontSize = 12, FontWeight = FontWeights.Bold }));
        //            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(c1))
        //            { FontFamily = new FontFamily("Arial"), FontSize = 12 }));
        //        }
        //#endif
        private string EmbedAttachments(string body)
        {
            if (body == null)
                return null;

            if (!MayHaveAttachmentsInline)
                return body;

            var dict = Attachments.Where(a => a.HasContentId)
                                  .GroupBy(a => a.ContentId)
                                  .Select(g => g.First())
                                  .ToDictionary(a => a.ContentId);

            return Regex.Replace(body, @"(="")cid:(.*?)("")", match =>
            {
                if (dict.TryGetValue(match.Groups[2].Value, out XstAttachment a))
                {
                    // There are limits to what we can push into an inline data image, 
                    // but we don't know exactly what
                    // Todo handle limit when known
                    a.WasRenderedInline = true;
                    string cooked = null;
                    using (var s = new MemoryStream())
                    {
                        a.SaveToStream(s);
                        s.Seek(0, SeekOrigin.Begin);
                        cooked = match.Groups[1] + @"data:image/jpg;base64," + EscapeString(Convert.ToBase64String(s.ToArray())) + match.Groups[3];
                    }
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
            var p = Properties.FirstOrDefault(x => x.PropertySetGuid == "00020386-0000-0000-c000-000000000046" && x.Name == "content-type");
            if (p != null)
            {

                Match m = Regex.Match((string)p.Value, @".*charset=""(.*?)""");
                if (m.Success)
                    return Encoding.GetEncoding(m.Groups[1].Value);
            }

            p = Properties.FirstOrDefault(x => x.Tag == PropertyCanonicalName.PidTagInternetCodepage);
            if (p != null)
            {
                return Encoding.GetEncoding((int)p.Value);
            }

            return null;
        }
    }
}
