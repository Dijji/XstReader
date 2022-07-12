using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstReader.Exporter
{
    public class XstExportOptions
    {
        [Category("Messages")]
        [DisplayName("Show Headers in messages")]
        [Description("Show message headers: Subject, To, From...")]
        public bool ShowHeadersInMessages { get; set; } = true;

        [Category("Attachments")]
        [DisplayName("Show Hidden Attachments in Header")]
        [Description("Show hidden attachments in the list of Attachments in the message header")]
        public bool ShowHiddenAttachmentsInList { get; set; } = false;

        [Category("Attachments")]
        [DisplayName("Embed Attachments")]
        [Description("Embed all attachments of messages in exported file")]
        public bool EmbedAttachmentsInFile { get; set; } = true;


        [Category("Messages")]
        [DisplayName("Show Details")]
        [Description("Show section Details in exported file")]
        public bool ShowDetails { get; set; } = true;

        [Category("Properties")]
        [DisplayName("Show Properties description")]
        [Description("Show section with all description of used known properties")]
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
