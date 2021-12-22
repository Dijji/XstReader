using System;
using System.Collections.Generic;
using System.Text;

namespace XstReader
{
    public static class IEnumerableXstAttachmentExtensions
    {
        public static void SaveToFolder(this IEnumerable<XstAttachment> attachments, string fullFolderName, DateTime? creationTime)
        {
            foreach (var attachment in attachments)
                attachment.SaveToFolder(fullFolderName, creationTime);
        }

    }
}
