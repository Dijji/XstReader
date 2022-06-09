using XstReader.ElementProperties;

namespace XstReader.App
{
    public static class XstAttachmentExtensions
    {
        public static bool CanBeOpenedInApp(this XstAttachment attachment)
        {
            if (attachment == null)
                return false;

            if (attachment.IsEmail)
                return true;

            var contentType = attachment.Properties[PropertyCanonicalName.PidTagAttachMimeTag];
            if (contentType?.Value is string value && !string.IsNullOrEmpty(value))
            {
                return value.ToLowerInvariant().StartsWith("image/") ||
                       value.ToLowerInvariant().StartsWith("audio/") ||
                       value.ToLowerInvariant().StartsWith("video/") ||
                       value.ToLowerInvariant().StartsWith("text/") ||
                       value.ToLowerInvariant() == "application/pdf";
            }

            var extension = Path.GetExtension(attachment.LongFileName).ToLowerInvariant().Replace(".", "");
            return extension == "png" ||
                   extension == "jpg" ||
                   extension == "jpeg" ||
                   extension == "bmp" ||
                   extension == "gif" ||
                   extension == "pdf";

        }

        public static string? SaveToTempFile(this XstAttachment attachment)
        {
            if (attachment == null)
                return null;

            var tempFileName = Path.GetTempFileName().Replace(".","") + Path.GetExtension(attachment.FileName);
            attachment?.SaveToFile(tempFileName);

            return tempFileName;
        }

    }
}
