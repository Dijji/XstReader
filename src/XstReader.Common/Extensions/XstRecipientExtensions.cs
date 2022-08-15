// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

namespace XstReader
{
    public static class XstRecipientExtensions
    {
        public static string GetIdWithType(this XstRecipient recipient)
            => $"{recipient.Message.GetId()}_{recipient.RecipientType}_{recipient.GetId()}";

        public static string GetId(this XstRecipient recipient)
            => recipient?.Address ?? string.Empty;

        public static DateTime GetDate(this XstRecipient recipient)
            => recipient?.Message?.GetDate() ??
               recipient?.LastModificationTime ??
               DateTime.MinValue.ToUniversalTime();

    }
}
