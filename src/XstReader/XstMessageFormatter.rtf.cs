// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2022, iluvadev, and released under Ms-PL License.

using System;
using System.IO;
using System.Linq;
#if !NETCOREAPP
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
#endif

namespace XstReader
{
    internal partial class XstMessageFormatter
    {
#if !NETCOREAPP
        public FlowDocument GetBodyAsFlowDocument()
        {

            FlowDocument doc = new FlowDocument();
            if (Message.Body.Bytes != null)
                using (var ms = new MemoryStream(Message.Body.Bytes))
                {
                    ms.Position = 0;
                    TextRange selection = new TextRange(doc.ContentStart, doc.ContentEnd);
                    selection.Load(ms, DataFormats.Rtf);
                }

            // For debug, a way to look at the document
            //var infoString = System.Windows.Markup.XamlWriter.Save(doc);
            return doc;
        }

        public void EmbedRtfHeader(FlowDocument doc, bool showEmailType = false)
        {
            if (doc == null)
                return;

            //omit MyName and the line under it for now, as we have no reliable source for it
            //Paragraph p = new Paragraph(new Run(MyName));
            //p.FontSize = 14;
            //p.FontWeight = FontWeights.Bold;
            //p.TextDecorations = TextDecorations.Underline;

            // Create the Table...
            var table1 = new Table();

            table1.Columns.Add(new TableColumn { Width = new GridLength(150) });
            table1.Columns.Add(new TableColumn { Width = new GridLength(500) });
            table1.RowGroups.Add(new TableRowGroup());

            AddRtfTableRow(table1, showEmailType ? "RTF From:" : "From:", Message.From);

            AddRtfTableRow(table1, "Sent:", String.Format("{0:dd MMMM yyyy HH:mm}", Message.Date));
            AddRtfTableRow(table1, "To:", ToFormatted);
            if (CcRecipients.Any())
                AddRtfTableRow(table1, "Cc:", CcFormatted);
            if (BccRecipients.Any())
                AddRtfTableRow(table1, "Bcc:", BccFormatted);
            AddRtfTableRow(table1, "Subject:", Message.Subject);
            if (Message.HasAttachmentsFiles)
                AddRtfTableRow(table1, "Attachments:", AttachmentsVisibleFilesFormatted);

            // Cope with the empty document case
            if (doc.Blocks.Count == 0)
                doc.Blocks.Add(new Paragraph(new Run("")));
            else
                doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, new Paragraph(new Run("")));
            doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, new Paragraph(new Run("")));
            doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, table1);

            //omit MyName and the line under it for now, as we have no reliable source for it
            //doc.Blocks.InsertBefore(doc.Blocks.FirstBlock, p);
        }

        private void AddRtfTableRow(Table table, string c0, string c1)
        {
            var currentRow = new TableRow();
            table.RowGroups[0].Rows.Add(currentRow);

            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(c0))
            { FontFamily = new FontFamily("Arial"), FontSize = 12, FontWeight = FontWeights.Bold }));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(c1))
            { FontFamily = new FontFamily("Arial"), FontSize = 12 }));
        }


        private void SaveMessageRft(string fullFileName)
        {
            var doc = GetBodyAsFlowDocument();
            EmbedRtfHeader(doc);
            TextRange content = new TextRange(doc.ContentStart, doc.ContentEnd);
            using (var stream = new FileStream(fullFileName, FileMode.Create))
            {
                content.Save(stream, DataFormats.Rtf);
            }
            if (Message.Date != null)
                File.SetCreationTime(fullFileName, (DateTime)Message.Date);
        }

#endif
    }
}
