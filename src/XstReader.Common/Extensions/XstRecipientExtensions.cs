namespace XstReader
{
    public static class XstRecipientExtensions
    {
        public static string GetId(this XstRecipient recipient)
            => recipient?.Address ?? string.Empty;

        public static DateTime GetDate(this XstRecipient recipient)
            => recipient?.Message?.GetDate() ??
               recipient?.LastModificationTime ??
               DateTime.MinValue.ToUniversalTime();

    }
}
