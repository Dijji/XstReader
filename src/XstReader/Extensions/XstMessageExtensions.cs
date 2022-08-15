// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.

using XstReader.Exporter;

namespace XstReader.App
{
    public static class XstMessageExtensions
    {
        public static string RenderAsHtml(this XstMessage? message, bool isInApp)
        {
            if (message == null)
                return string.Empty;

            var exporter = new ExporterHtml();
            exporter.ExportOptions.EmbedAttachmentsInFile = !isInApp;
            exporter.ExportOptions.ShowDetails = !isInApp;

            return exporter.Render(message);
        }

        public static string GetFilenameForExport(this XstMessage? message)
            => message?.DisplayName.ReplaceInvalidFileNameChars("_") ?? "No_Name";

    }
}
