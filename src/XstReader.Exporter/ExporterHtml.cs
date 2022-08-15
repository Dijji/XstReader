// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using System.Text;
using XstReader.App.Common;
using XstReader.ElementProperties;

namespace XstReader.Exporter
{
    public class ExporterHtml
    {
        private Dictionary<PropertyCanonicalName, KnownCanonicalNameProperty>? _UsedProperties = null;
        private Dictionary<PropertyCanonicalName, KnownCanonicalNameProperty> UsedProperties => _UsedProperties ??= new();


        private XstExportOptions? _ExportOptions = null;
        public XstExportOptions ExportOptions
        {
            get => _ExportOptions ??= new();
            init => _ExportOptions = value;
        }



        public ExporterHtml()
        {
        }
        public ExporterHtml(XstExportOptions exportOptions)
        {
            ExportOptions = exportOptions;
        }

        public string Render(XstMessage message)
        {
            _UsedProperties = null;

            StringBuilder sb = new();
            RenderMain(message, sb);
            return sb.ToString();
        }

        private void RenderMain(XstMessage message, StringBuilder sb)
        {
            sb.AppendLine("<style>details {padding-left:1em; padding-top:0.5em;} details>div {padding-left:2em;border-style:none none solid dotted;border-color:lightgray}</style>");
            sb.AppendLine("<body>");
            {
                RenderMessage(message, sb);

                // Properties
                if (ExportOptions.ShowDetails && ExportOptions.ShowPropertiesDescriptions)
                    RenderKnownCanonicalNameProperties(sb);
            }
            sb.AppendLine("</body>");
        }
        private void RenderMessage(XstMessage message, StringBuilder sb)
        {
            //Subject
            sb.AppendLine($"<H1>{message.Subject ?? message.DisplayName}</H1>");

            //Header
            RenderHeaderMessage(message, sb);

            //Body
            if (message.Body?.Format == XstMessageBodyFormat.Rtf)
                sb.AppendLine(RtfPipe.Rtf.ToHtml(message.Body.Text));
            else
                sb.AppendLine(message.Body?.Text ?? "");

            if (ExportOptions.ShowDetails)
            {
                sb.AppendLine("<br><hr>");
                //Details
                sb.AppendLine("<H2>Details</H2>");

                //Message
                RenderMessageDetails(message, sb);

                //Recipients
                RenderRecipientsDetails(message, sb);

                //Attachments
                RenderAttachmentsDetails(message, sb);
            }
        }

        private void RenderHeaderMessage(XstMessage message, StringBuilder sb)
        {
            if (!ExportOptions.ShowHeadersInMessages)
                return;
            sb.Append("<p>");
            RenderRecipientsInHeader(message.Recipients[RecipientType.Sender], "From", sb);
            RenderRecipientsInHeader(message.Recipients[RecipientType.OriginalSentRepresenting], "Representing", sb);
            sb.Append($"<b>Sent:</b> {message.SubmittedTime}<br>");
            RenderRecipientsInHeader(message.Recipients[RecipientType.To], "To", sb);
            RenderRecipientsInHeader(message.Recipients[RecipientType.Cc], "Cc", sb);
            RenderRecipientsInHeader(message.Recipients[RecipientType.Bcc], "Bcc", sb);
            sb.Append($"<b>Received:</b> {message.ReceivedTime}<br>");
            RenderRecipientsInHeader(message.Recipients[RecipientType.ReceivedBy], "Received By", sb);
            RenderRecipientsInHeader(message.Recipients[RecipientType.ReceivedRepresenting], "Received Representing", sb);
            sb.Append($"<b>Subject:</b> {message.Subject}<br>");

            RenderAttachmentsInHeader(message.Attachments.Where(a => !a.IsHidden && a.IsFile), "Attachments (files)", sb);
            RenderAttachmentsInHeader(message.Attachments.Where(a => !a.IsHidden && a.IsEmail), "Attachments (emails)", sb);
            if (ExportOptions.ShowHiddenAttachmentsInList)
            {
                RenderAttachmentsInHeader(message.Attachments.Where(a => a.IsHidden && a.IsFile), "Attachments (hidden files)", sb);
                RenderAttachmentsInHeader(message.Attachments.Where(a => a.IsHidden && a.IsEmail), "Attachments (hidden emails)", sb);
            }
            sb.AppendLine("</p>");
        }
        private void RenderRecipientsInHeader(IEnumerable<XstRecipient> recipients, string label, StringBuilder sb)
        {
            if (!recipients.Any())
                return;

            sb.Append($"<b>{label}:</b> ");
            bool first = true;
            foreach (var recipient in recipients)
            {
                if (!first) sb.Append(", ");
                if (ExportOptions.ShowDetails) sb.Append($"<a href=\"#{recipient.GetIdWithType()}\">");
                sb.Append(recipient.DisplayName);
                if (ExportOptions.ShowDetails) sb.Append("</a>");
                sb.Append($"&#60;<a href=\"mailto: {recipient.Address}\">{recipient.Address}</a>&#62;");
                first = false;
            }
            sb.AppendLine("<br>");
        }
        private void RenderAttachmentsInHeader(IEnumerable<XstAttachment> attachments, string label, StringBuilder sb)
        {
            if (!attachments.Any())
                return;

            sb.Append($"<b>{label}:</b> ");
            bool first = true;
            foreach (var attachment in attachments)
            {
                if (!first) sb.Append(", ");
                if (ExportOptions.ShowDetails) sb.Append($"<a href=\"#{attachment.ContentId}\">");
                sb.Append(attachment.DisplayName ?? attachment.FileName);
                if (ExportOptions.ShowDetails) sb.Append("</a>");
                sb.Append($" {attachment.SizeWithMagnitude()}");
                first = false;
            }
            sb.AppendLine("<br>");
        }

        private void RenderMessageDetails(XstMessage message, StringBuilder sb)
        {
            sb.AppendLine("<H3>Message</H3>");
            sb.Append("<div style='padding-left:1em'>");
            RenderHeaderMessage(message, sb);
            sb.Append("<p>");
            sb.Append($"<b>Sent Representing other:</b> {message.IsSentRepresentingOther}<br>");
            sb.Append($"<b>Received Representing other:</b> {message.IsReceivedRepresentingOther}<br>");

            sb.Append($"<b>Importance:</b> {message.Importance}<br>");
            sb.Append($"<b>Priority:</b> {message.Priority}<br>");
            sb.Append($"<b>Sensitivity:</b> {message.Sensitivity}<br>");
            sb.Append($"<b>Status:</b> {message.Status}<br>");

            sb.Append($"<b>Is Draft:</b> {message.IsDraft}<br>");
            sb.Append($"<b>Is Read:</b> {message.IsRead}<br>");
            sb.Append($"<b>Is Encrypted or Signed:</b> {message.IsEncryptedOrSigned}<br>");
            sb.Append($"<b>Format:</b> {message.Body?.Format}<br>");
            sb.Append("</p>");
            sb.Append("</div>");

            sb.Append("<details><summary><b>Properties</b></summary><div>");
            RenderPropertiesInDetails(message.Properties, sb);
            sb.AppendLine("</div></details>");
        }

        private void RenderRecipientsDetails(XstMessage message, StringBuilder sb)
        {
            sb.AppendLine("<H3>Recipients</H3>");
            foreach (var recipient in message.Recipients.Items.OrderBy(r => r.RecipientType))
            {
                sb.Append("<details><summary>");
                sb.Append($"<b>{recipient.RecipientType}:</b> {recipient.DisplayName}");
                sb.Append($"&#60;<a href=\"mailto: {recipient.Address}\">{recipient.Address}</a>&#62;");
                sb.Append("</summary><div>");
                RenderRecipientInDetails(recipient, sb);
                sb.AppendLine("</div></details>");
            }
        }
        private void RenderRecipientInDetails(XstRecipient recipient, StringBuilder sb)
        {
            sb.AppendLine($"<a name=\"{recipient.GetIdWithType()}\"><H4>{recipient.DisplayName}</H4></a>");
            sb.Append("<p>");
            sb.Append($"<b>Display Name:</b> {recipient.DisplayName}<br>");
            sb.Append($"<b>Address:</b> <a href=\"mailto: {recipient.Address}\">{recipient.Address}</a><br>");
            sb.Append($"<b>Recipient Type:</b> {recipient.RecipientType}<br>");
            sb.Append("</p>");

            sb.Append("<details><summary><b>Properties</b></summary><div>");
            RenderPropertiesInDetails(recipient.Properties, sb);
            sb.AppendLine("</div></details>");

            sb.AppendLine("<br>");
        }

        private void RenderAttachmentsDetails(XstMessage message, StringBuilder sb)
        {
            sb.AppendLine($"<H3>Attachments</H3>");
            foreach (var attachment in message.Attachments.OrderBy(a => a.IsHidden).ThenBy(a => a.DisplayName ?? a.FileName))
            {
                sb.Append("<details><summary>");
                if (attachment.IsFile) sb.Append("<b>File:</b> "); else sb.Append("<b>Email:</b> ");
                sb.Append($"{attachment.DisplayName ?? attachment.FileName} ({attachment.SizeWithMagnitude()})");
                if (attachment.IsHidden) sb.Append(" <i>(Hidden)</i>");
                sb.Append("</summary><div>");
                RenderAttachmentInDetails(attachment, sb);
                sb.AppendLine("</div></details>");
            }

        }

        private void RenderPropertiesInDetails(XstPropertySet properties, StringBuilder sb)
        {
            sb.Append("<p>");
            sb.Append("<table width='100%' border='1' style='border-collapse:collapse'>");
            sb.Append("<thead><tr><th>Area</th><th>Name</th><th>Id</th><th>Value</th></tr></thead>");
            sb.Append("<tbody>");
            foreach (var property in properties.Items.OrderBy(p => p.Tag.PropertyArea()?.FriendlyName() ?? ""))
            {
                AddUsedProperty(property);
                sb.Append("<tr>");

                sb.Append("<td>");
                sb.Append(property.Tag.PropertyArea()?.FriendlyName() ?? "General");
                sb.Append("</td>");

                sb.Append("<td>");
                if (ExportOptions.ShowPropertiesDescriptions && !string.IsNullOrWhiteSpace(property.HtmlDescription))
                    sb.Append($"<a href=\"#Property_{property.Tag.CanonicalName()}\">");
                sb.Append(property.Name);
                if (ExportOptions.ShowPropertiesDescriptions && !string.IsNullOrWhiteSpace(property.HtmlDescription))
                    sb.Append("</a>");
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(property.Id);
                sb.Append("</td>");

                sb.Append("<td>");
                if (property.Value is byte[])
                    sb.Append($"<a href='data:application/octet-stream;base64,{Convert.ToBase64String(property.Value)}' download='{property.CanonicalName}'>Binary content</a>");
                else
                    sb.Append(property.DisplayValue);
                sb.Append("</td>");

                sb.AppendLine("</tr>");
            }
            sb.Append("</tbody></table>");
            sb.AppendLine("</p>");
        }

        private void RenderAttachmentInDetails(XstAttachment attachment, StringBuilder sb)
        {
            sb.AppendLine($"<a name=\"{attachment.ContentId}\"><H4>{attachment.DisplayName ?? attachment.FileName}</H4></a>");
            sb.Append("<p>");
            sb.Append($"<b>Display Name:</b> {attachment.DisplayName}<br>");
            sb.Append("<b>Type:</b> "); if (attachment.IsFile) sb.Append("File"); else sb.Append("Email"); sb.Append("<br>");
            sb.Append("<b>Visibility:</b> "); if (attachment.IsHidden) sb.Append("Hidden"); else sb.Append("Visible"); sb.Append("<br>");
            sb.Append($"<b>Size: </b> {attachment.SizeWithMagnitude()}<br>");
            sb.Append($"<b>MD5: </b> {attachment.Md5}<br>");
            sb.Append($"<b>File Name:</b> {attachment.FileName}<br>");
            sb.Append($"<b>Long File Name:</b> {attachment.LongFileName}<br>");
            sb.Append($"<b>Last Modification:</b> {attachment.LastModificationTime}<br>");
            sb.Append("</p>");

            sb.Append("<details><summary><b>Properties</b></summary><div>");
            RenderPropertiesInDetails(attachment.Properties, sb);
            sb.AppendLine("</div></details>");

            sb.Append("<details><summary><b>Content</b></summary><div>");
            sb.Append("<p>");
            if (ExportOptions.EmbedAttachmentsInFile)
            {
                string contentType = (attachment.Properties[PropertyCanonicalName.PidTagAttachMimeTag]?.Value as string ?? "application/octet-stream").ToLowerInvariant();
                string href = $"data:{contentType};base64,{attachment.GetContentAsBase64()}";

                if (contentType.StartsWith("image/")) sb.Append($"<img src=\"{href}\" alt=\"{attachment.Description}\"/>");
                else if (contentType.StartsWith("audio/")) sb.Append($"<audio controls src=\"{href}\"></audio>");
                else if (contentType.StartsWith("video/")) sb.Append($"<video controls><source type=\"{contentType}\" src=\"{href}\"></video>");
                else if (contentType == "application/pdf" || contentType.StartsWith("text/")) sb.Append($"<iframe src=\"{href}\" height=\"70%\" width=\"100%\" frameBorder=\"0\" scrolling=\"no\"></iframe>");
                else if (attachment.IsFile) sb.Append($"<a href=\"{href}\" download=\"{attachment.LongFileName}\">Download</a>");
                else if (attachment.IsEmail) RenderMessage(attachment.AttachedEmailMessage, sb);
            }
            sb.Append("</p>");
            sb.AppendLine("</div></details>");
            sb.AppendLine("<br>");
        }

        private void RenderKnownCanonicalNameProperties(StringBuilder sb)
        {
            sb.AppendLine("<H3>Description of properties</H3>");

            foreach (var property in UsedProperties.Values.OrderBy(p => p.Area.FriendlyName()))
            {
                sb.Append($"<details><summary><b>{property.CanonicalName}</b> {string.Format("0x{0:x4}", property.Id)}</summary><div>");

                sb.AppendLine($"<a name=\"Property_{property.CanonicalName}\"><H4>{property.CanonicalName}</H4></a>");
                sb.Append($"<p><b>Area:</b> {property.Area.FriendlyName()}</p>");
                sb.Append($"<p><b>Category:</b> {property.Category}</p>");
                sb.Append($"<p><b>Set:</b> {property.Set.Description()} - {property.Set.Guid()}</p>");
                sb.Append(property.HtmlDescription);

                sb.AppendLine("</div></details>");
            }
        }

        private void AddUsedProperty(XstProperty property)
        {
            if (!UsedProperties.ContainsKey(property.Tag))
            {
                var prop = KnownCanonicalNames.KnonwnProperties.FirstOrDefault(p => p.CanonicalName == property.Tag.CanonicalName());
                if (prop != null)
                    UsedProperties.Add(property.Tag, prop);
            }
        }
    }
}
