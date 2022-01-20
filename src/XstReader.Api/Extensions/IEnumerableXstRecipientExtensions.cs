using System.Collections.Generic;
using System.Linq;
using XstReader.ElementProperties;

namespace XstReader
{
    public static class IEnumerableXstRecipientExtensions
    {
        public static IEnumerable<XstRecipient> Filter(this IEnumerable<XstRecipient> recipients, RecipientType recipientType)
            => recipients?.Where(r => (r.RecipientType == recipientType));

        public static IEnumerable<XstRecipient> To(this IEnumerable<XstRecipient> recipients)
            => recipients.Filter(RecipientType.To);

        public static IEnumerable<XstRecipient> Cc(this IEnumerable<XstRecipient> recipients)
            => recipients.Filter(RecipientType.Cc);

        public static IEnumerable<XstRecipient> Bcc(this IEnumerable<XstRecipient> recipients)
            => recipients.Filter(RecipientType.Bcc);

        public static IEnumerable<XstRecipient> Originator(this IEnumerable<XstRecipient> recipients)
            => recipients.Filter(RecipientType.Originator);
    }
}
