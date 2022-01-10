using System;
using System.Collections.Generic;
using System.Linq;

namespace XstReader
{
    public class XstExportOptions
    {
        public string UnknownValueText { get; set; }

        public Func<XstRecipient, string> RecipientFormatter { get; set; }
        public Func<IEnumerable<XstRecipient>, string> RecipientListFormatter { get; set; }

        public Func<DateTime, string> DateFormatter { get; set; }

        public Func<XstAttachment, string> AttachmentFormatter { get; set; }
        public Func<IEnumerable<XstAttachment>, string> AttachmentListFormatter { get; set; }

        public Func<XstMessage, bool> ShowMessageHeader { get; set; }


        /// <summary>
        /// Ctor
        /// </summary>
        public XstExportOptions()
        {
            RecipientFormatter = (r) => r?.DisplayName + (string.IsNullOrEmpty(r?.EmailAddress) ? "" : $" <{r.EmailAddress}>");
            RecipientListFormatter = (rl) => String.Join("; ", rl.Select(r => Format(r)));

            DateFormatter = (d) => d.ToString("f");

            UnknownValueText = "<unknown>";

            AttachmentFormatter = (a) => a == null ? "" : $"{a.DisplayName} ({a.Size}Kb)";
            AttachmentListFormatter = (al) => String.Join("; ", al.Select(a => Format(a)));

            ShowMessageHeader = (m) => true;
        }

        public string Format(XstRecipient recipient)
            => RecipientFormatter?.Invoke(recipient) ?? recipient.ToString();

        public string Format(IEnumerable<XstRecipient> recipientList)
            => RecipientListFormatter?.Invoke(recipientList ?? new List<XstRecipient>()) ?? String.Join("; ", recipientList.Select(r => Format(r)));

        public string Format(DateTime dateTime)
            => DateFormatter(dateTime);

        public string Format(DateTime? dateTime)
            => dateTime == null ? UnknownValueText : Format(dateTime.Value);

        public string Format(XstAttachment attachment)
            => attachment == null ? "" : AttachmentFormatter?.Invoke(attachment) ?? attachment.ToString();

        public string Format(IEnumerable<XstAttachment> attachmentList)
            => AttachmentListFormatter?.Invoke(attachmentList ?? new List<XstAttachment>()) ?? String.Join("; ", attachmentList.Select(a => Format(a)));
    }
}
