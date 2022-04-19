using XstReader.ElementProperties;

namespace XstReader
{
    public static class XstMessageExtensions
    {
        public static string GetId(this XstMessage message)
            => message?.Properties[PropertyCanonicalName.PidTagInternetMessageId]?.Value ??
               message?.DisplayName ??
               message?.Subject ??
               string.Empty;

        public static DateTime GetDate(this XstMessage message)
            => message?.Date ??
               message?.LastModificationTime ??
               message?.ParentFolder.GetDate() ??
               DateTime.MinValue.ToUniversalTime();
    }
}
