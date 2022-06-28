using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstReader.App.Common
{
    public class XstExportOptions
    {
        public bool ShowHeadersInMessages { get; set; } = true;

        public bool ShowHiddenAttachmentsInList { get; set; } = false;

        public bool EmbedAttachmentsInFile { get; set; } = true;

        public bool ShowDetails { get; set; } = true;

        public bool ShowPropertiesDescriptions { get; set; } = true;

        public XstExportOptions Clone()
            => new XstExportOptions
            {
                ShowHeadersInMessages = ShowHeadersInMessages,
                ShowHiddenAttachmentsInList = ShowHiddenAttachmentsInList,
                EmbedAttachmentsInFile = EmbedAttachmentsInFile,
                ShowPropertiesDescriptions = ShowPropertiesDescriptions,
            };
    }
}
