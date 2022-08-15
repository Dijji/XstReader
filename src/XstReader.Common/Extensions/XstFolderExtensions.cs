// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using XstReader;

namespace XstReader
{
    public static class XstFolderExtensions
    {
        public static string GetId(this XstFolder folder)
            => folder?.Path ?? string.Empty;

        public static DateTime GetDate(this XstFolder folder)
            => folder?.LastModificationTime ??
               DateTime.MinValue.ToUniversalTime();

        public static int CountTreeSubfolders(this XstFolder folder)
            => folder == null ? 0
               : folder.Folders.Count() + folder.Folders.Sum(f => f.CountTreeSubfolders());
    }
}
