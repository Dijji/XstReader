using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstReader.Exporter
{
    public static class XstAttachmentExtensions
    {
        public static string SizeWithMagnitude(this XstAttachment attachment)
        {
            if (attachment.Size > 1000000)
                return $"{attachment.Size / 1000000}Mb";
            else if (attachment.Size > 1000)
                return $"{attachment.Size / 1000}Kb";
            else
                return $"{attachment.Size}b";
        }
    }
}
