using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XstReader.ElementProperties;

namespace XstReader
{
    public static class IEnumerableXstRecipientExtensions
    {
        public static IEnumerable<XstRecipient> Filter(this IEnumerable<XstRecipient> recipients, RecipientTypes recipientTypes)
            => recipients?.Where(r => r.RecipientType == recipientTypes);

        public static IEnumerable<XstRecipient> To(this IEnumerable<XstRecipient> recipients)
            => recipients.Filter(RecipientTypes.To);

        public static IEnumerable<XstRecipient> Cc(this IEnumerable<XstRecipient> recipients)
            => recipients.Filter(RecipientTypes.Cc);

        public static IEnumerable<XstRecipient> Bcc(this IEnumerable<XstRecipient> recipients)
            => recipients.Filter(RecipientTypes.Bcc);

        public static IEnumerable<XstRecipient> Originator(this IEnumerable<XstRecipient> recipients)
            => recipients.Filter(RecipientTypes.Originator);
    }
}
