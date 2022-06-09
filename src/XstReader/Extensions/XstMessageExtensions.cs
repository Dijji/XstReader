using System.Text;
using XstReader.ElementProperties;

namespace XstReader.App
{
    public static class XstMessageExtensions
    {
        public static string? GetHtmlVisualization(this XstMessage? message)
        {
            if (message?.Body == null)
                return null;

            string htmlText = (message?.Body.Format) switch
            {
                XstMessageBodyFormat.Rtf => RtfPipe.Rtf.ToHtml(message.Body.Text),
                _ => message?.Body.Text ?? "",
            };

            htmlText = $"<html><body><hr>{GetHtmlHeader(message)}<hr><p/><p/>{htmlText}</body></html>";
            return htmlText;
        }

        private static string? GetHtmlHeader(this XstMessage? message)
        {
            if (message == null)
                return null;

            var sb = new StringBuilder();

            sb.AppendLine("<p class=\"MsoNormal\">");

            sb.Append("<b>From:</b> ");
            sb.Append(XstFormatter.Format(message.Recipients[RecipientType.Sender]).AppendNewLine().TextToHtml());

            if (message.IsSentRepresentingOther)
            {
                sb.Append("<b>Representing:</b> ");
                sb.Append(XstFormatter.Format(message.Recipients[RecipientType.OriginalSentRepresenting]).AppendNewLine().TextToHtml());
            }

            if (message.SubmittedTime != null)
            {
                sb.Append("<b>Sent:</b> ");
                sb.Append(XstFormatter.Format(message.SubmittedTime).AppendNewLine().TextToHtml());
            }

            sb.Append("<b>To:</b> ");
            sb.Append(XstFormatter.Format(message.Recipients[RecipientType.To]).AppendNewLine().TextToHtml());

            if (message.Recipients[RecipientType.Cc].Any())
            {
                sb.Append("<b>Cc:</b> ");
                sb.Append(XstFormatter.Format(message.Recipients[RecipientType.Cc]).AppendNewLine().TextToHtml());
            }

            if (message.Recipients[RecipientType.Bcc].Any())
            {
                sb.Append("<b>Bcc:</b> ");
                sb.Append(XstFormatter.Format(message.Recipients[RecipientType.Bcc]).AppendNewLine().TextToHtml());
            }

            if (message.ReceivedTime != null)
            {
                sb.Append("<b>Received:</b> ");
                sb.Append(XstFormatter.Format(message.ReceivedTime).AppendNewLine().TextToHtml());
            }

            if (message.Recipients[RecipientType.ReceivedBy].Any())
            {
                sb.Append("<b>Received by:</b> ");
                sb.Append(XstFormatter.Format(message.Recipients[RecipientType.ReceivedBy]).AppendNewLine().TextToHtml());
            }

            if (message.IsReceivedRepresentingOther)
            {
                sb.Append("<b>Received representing:</b> ");
                sb.Append(XstFormatter.Format(message.Recipients[RecipientType.ReceivedRepresenting]).AppendNewLine().TextToHtml());
            }

            sb.Append("<b>Subject:</b> ");
            sb.Append(message.Subject.AppendNewLine().TextToHtml());

            if (message.Attachments.Any(a => !a.IsHidden && a.IsFile))
            {
                sb.Append("<b>Attachments (files):</b> ");
                sb.Append(XstFormatter.Format(message.Attachments.Where(a=> !a.IsHidden && a.IsFile)).AppendNewLine().TextToHtml());
            }

            if (message.Attachments.Any(a => !a.IsHidden && a.IsEmail))
            {
                sb.Append("<b>Attachments (emails):</b> ");
                sb.Append(XstFormatter.Format(message.Attachments.Where(a => !a.IsHidden && a.IsEmail)).AppendNewLine().TextToHtml());
            }

            sb.AppendLine("</p>");
            return sb.ToString();
        }
    }
}
