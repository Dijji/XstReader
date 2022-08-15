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
    internal class XstReaderOptions
    {
        public bool ShowHiddenAttachments { get; set; } = false;

        public XstExportOptions ExportOptions { get; set; } = new XstExportOptions();
    }
}
