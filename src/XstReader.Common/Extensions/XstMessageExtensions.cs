// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

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
