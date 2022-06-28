using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XstReader.App.Common;

namespace XstReader.App
{
    internal class XstReaderOptions
    {
        public bool ShowHiddenAttachments { get; set; } = false;

        public XstExportOptions ExportOptions { get; set; } = new XstExportOptions();
    }
}
