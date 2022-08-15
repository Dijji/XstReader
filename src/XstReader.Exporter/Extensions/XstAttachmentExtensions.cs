// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

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
