using XstReader.ElementProperties;

namespace XstReader.App.Common
{
    public static class XstAttachmentExtensions
    {
        public static byte[] GetContent(this XstAttachment attachment)
        {
            using (var ms = new MemoryStream())
            {
                attachment.SaveToStream(ms);
                return ms.ToArray();
            }
        }

        public static string GetContentAsBase64(this XstAttachment attachment)
            => Convert.ToBase64String(GetContent(attachment));

    }
}
