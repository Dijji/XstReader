// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XstReader.ElementProperties;

namespace XstReader
{
    internal partial class XstMessageFormatter
    {
        public XstMessage Message { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public XstMessageFormatter()
        {
        }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        public XstMessageFormatter(XstMessage message)
        {
            Message = message;
        }

        public string ExportFileExtension
            => Message?.Body.Format == XstMessageBodyFormat.Html ? "html" 
               : Message?.Body.Format == XstMessageBodyFormat.Rtf ? "rtf" 
               : "txt";

        private string _ExportFileName = null;
        public string ExportFileName => _ExportFileName ?? (_ExportFileName = String.Format("{0:yyyy-MM-dd HHmm} {1}", Message?.Date, Message?.Subject).Truncate(150).ReplaceInvalidFileNameChars(" "));

        private XstRecipient OriginatorRecipient => Message.Recipients[RecipientType.Originator].FirstOrDefault();
        private IEnumerable<XstRecipient> ToRecipients => Message.Recipients[RecipientType.To];
        private IEnumerable<XstRecipient> CcRecipients => Message.Recipients[RecipientType.Cc];
        private IEnumerable<XstRecipient> BccRecipients => Message.Recipients[RecipientType.Bcc];
        private XstRecipient OriginalSentRepresentingRecipient => Message.Recipients[RecipientType.OriginalSentRepresenting].FirstOrDefault();
        private XstRecipient SentRepresentingRecipient => Message.Recipients[RecipientType.SentRepresenting].FirstOrDefault();
        private XstRecipient ReceivedRepresentingRecipient => Message.Recipients[RecipientType.ReceivedRepresenting].FirstOrDefault();
        private XstRecipient SenderRecipient => Message.Recipients[RecipientType.Sender].FirstOrDefault();
        private XstRecipient ReceivedByRecipient => Message.Recipients[RecipientType.ReceivedBy].FirstOrDefault();

        public string SenderFormatted => XstFormatter.Format(SenderRecipient);
        public string SentRepresentingFormatted => XstFormatter.Format(SentRepresentingRecipient);
        public string ReceivedRepresentingFormatted => XstFormatter.Format(ReceivedRepresentingRecipient);
        public string ToFormatted => XstFormatter.Format(ToRecipients);
        public string CcFormatted => XstFormatter.Format(CcRecipients);
        public string BccFormatted => XstFormatter.Format(BccRecipients);

        public string ReceivedByFormatted => XstFormatter.Format(ReceivedByRecipient);
        public string DateFormatted => XstFormatter.Format(Message.Date);
        public string ReceivedFormatted => XstFormatter.Format(Message.ReceivedTime);
        public string SubmittedFormatted => XstFormatter.Format(Message.SubmittedTime);

        public string AttachmentsVisibleFilesFormatted => XstFormatter.Format(Message.AttachmentsVisibleFiles);

        private string HtmlHeader
            => "<p class=\"MsoNormal\">" +
                    $"<b>From:</b> {SenderFormatted.AppendNewLine().TextToHtml()}" +
                    (Message.IsSentRepresentingOther ? $"<b>Representing:</b> {SentRepresentingFormatted.AppendNewLine().TextToHtml()}" : "") +
                    (Message.SubmittedTime != null ? $"<b>Sent:</b> {SubmittedFormatted.AppendNewLine().TextToHtml()}" : "") +
                    $"<b>To:</b> {ToFormatted.AppendNewLine().TextToHtml()}" +
                    (CcRecipients.Any() ? $"<b>Cc:</b> {CcFormatted.AppendNewLine().TextToHtml()}" : "") +
                    (BccRecipients.Any() ? $"<b>Bcc:</b> {BccFormatted.AppendNewLine().TextToHtml()}" : "") +
                    (Message.ReceivedTime != null ? $"<b>Received:</b> {ReceivedFormatted.AppendNewLine().TextToHtml()}" : "") +
                    (ReceivedByRecipient != null ? $"<b>Received by:</b> {ReceivedByFormatted.AppendNewLine().TextToHtml()}" : "") +
                    (Message.IsReceivedRepresentingOther ? $"<b>Received representing:</b> {ReceivedRepresentingFormatted.AppendNewLine().TextToHtml()}" : "") +
                    $"<b>Subject:</b> {Message.Subject.AppendNewLine().TextToHtml()}" +
                    (Message.HasAttachmentsVisibleFiles ? $"<b>Attachments:</b> {AttachmentsVisibleFilesFormatted.AppendNewLine().TextToHtml()}" : "") +
                "</p><p/><p/>";
        private string TxtHeader
            => $"From: {SenderFormatted.AppendNewLine()}" +
                (Message.IsSentRepresentingOther ? $"Representing: {SentRepresentingFormatted.AppendNewLine()}" : "") +
                (Message.SubmittedTime != null ? $"Sent: {SubmittedFormatted.AppendNewLine()}" : "") +
                $"To: {ToFormatted.AppendNewLine()}" +
                (CcRecipients.Any() ? $"Cc: {CcFormatted.AppendNewLine()}" : "") +
                (BccRecipients.Any() ? $"Bcc: {BccFormatted.AppendNewLine()}" : "") +
                (Message.ReceivedTime != null ? $"Received: {ReceivedFormatted.AppendNewLine()}" : "") +
                (ReceivedByRecipient != null ? $"Received by: {ReceivedByFormatted.AppendNewLine()}" : "") +
                (Message.IsReceivedRepresentingOther ? $"Received representing: {ReceivedRepresentingFormatted.AppendNewLine()}" : "") +
                $"Subject: {Message.Subject.AppendNewLine()}" +
                (Message.HasAttachmentsVisibleFiles ? $"Attachments: {AttachmentsVisibleFilesFormatted.AppendNewLine()}" : "")
                .AppendNewLine().AppendNewLine().AppendNewLine();

        public string EmbedHtmlHeader(string body)
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

        public string EmbedTextHeader(string body)
            => TxtHeader + body ?? "";

        public void SaveMessage(string fullFileName, bool includeVisibleAttachments = true)
        {
            if (Message.Body.Format == XstMessageBodyFormat.Html)
            {
                string body = Message.Body.Text;

                if (body != null)
                {
                    body = EmbedHtmlHeader(body);
                    using (var stream = new FileStream(fullFileName, FileMode.Create))
                    {
                        var bytes = Encoding.UTF8.GetBytes(body);
                        stream.Write(bytes, 0, bytes.Count());
                    }
                    if (Message.Date != null)
                        File.SetCreationTime(fullFileName, (DateTime)Message.Date);
                }
            }
            else if (Message.Body.Format == XstMessageBodyFormat.Rtf)
            {
#if !NETCOREAPP
                SaveMessageRft(fullFileName);
#else
                throw new XstException("Emails with body in RTF format not supported on this platform");
#endif
            }
            else
            {
                var body = EmbedTextHeader(Message.Body.Text);
                using (var stream = new FileStream(fullFileName, FileMode.Create))
                {
                    var bytes = Encoding.UTF8.GetBytes(body);
                    stream.Write(bytes, 0, bytes.Count());
                }
                if (Message.Date != null)
                    File.SetCreationTime(fullFileName, (DateTime)Message.Date);
            }
            if (includeVisibleAttachments)
                SaveVisibleAttachmentsToAssociatedFolder(Message, fullFileName);
        }


        private void SaveVisibleAttachmentsToAssociatedFolder(XstMessage message, string fullFileName)
        {
            if (message.HasAttachmentsVisibleFiles)
            {
                var targetFolder = Path.Combine(Path.GetDirectoryName(fullFileName),
                    Path.GetFileNameWithoutExtension(fullFileName) + " Attachments");
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                    if (message.Date != null)
                        Directory.SetCreationTime(targetFolder, (DateTime)message.Date);
                }
                message.Attachments.Where(a => a.IsFile && !a.Hide).SaveToFolder(targetFolder, message.Date);
            }
        }
    }
}
